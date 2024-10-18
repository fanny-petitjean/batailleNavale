using BattleShip.App;
using BattleShip.Models;
using Microsoft.AspNetCore.Mvc;
using BattleShip.App.Service;
using System;
using System.Collections.Generic;
using BattleShip.API;
using Grpc.AspNetCore.Web;
using System.Text.Json;
using System.ComponentModel;
using System.Numerics;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);
var gameService = new GameService();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorApp",
        builder =>
        {
            builder.WithOrigins("https://localhost:5254")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
        });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, c =>
    {
        c.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";
        c.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidAudience = builder.Configuration["Auth0:Audience"],
            ValidIssuer = $"https://{builder.Configuration["Auth0:Domain"]}"
        };
    });

builder.Services.AddAuthorization(); 
builder.Services.AddGrpc();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<GameService>();

var app = builder.Build();
app.UseCors("AllowAll");
app.UseCors("AllowBlazorApp");
app.UseAuthentication();
app.UseAuthorization();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });


app.MapGrpcService<BattleshipGrpcService>().EnableGrpcWeb();


var converter = new Converter();

app.MapPost("/newGame/{gameType}", async ([FromRoute] string gameType, [FromQuery] int? difficulty, [FromQuery] int gridSize, [FromBody] List<ShipN> gameSetupRequest) =>
{
    List<Ship> shipList = new List<Ship>();
    foreach (var row in gameSetupRequest)
    {
        shipList.Add(new Ship(row.Letter, row.Length, row.IsHorizontal, row.StartX, row.StartY));
    }
    var placeGrid = new PlaceShipGrid(gridSize, shipList);
    
    var player = new Player("Player 1", false, placeGrid);

    Player opponent;
    if (difficulty.HasValue)
    {
        opponent = new Player("ia", true, gridSize, difficulty.Value);
    }
    else
    {
        opponent = new Player("Player 2", true, gridSize);
    }

    var players = new List<Player> { player, opponent };
    var game = new Game(players, 0);

    Guid guid = gameService.AddGame(game);

    var PlayerGrid = converter.ConvertCharArrayToList(player.PlaceShipGrid.Grid);
    var OpponentGrid = converter.ConvertBoolArrayToList(game.displayOpponentGrid(opponent.PlaceShipGrid));

    var response = new
    {
        GameId = guid.ToString(),
        Player = player.Name,
        Opponent = opponent.Name,
        PlayerGrid = PlayerGrid,
        OpponentGrid = OpponentGrid
    };

    return Results.Ok(response);
});


app.MapPost("/attack/{gameId}", (Guid gameId, [FromQuery] int x, [FromQuery] int y) =>
{
    var game = gameService.GetGame(gameId);
    if (game == null)
    {
        return Results.NotFound("Game not found");
    }

    var player = game.Players.First(); 
    var ia = game.Players.Last();

    string responseChar = game.attack(player, ia, x, y);

    bool gameOver = false;
    string winner = game.Winner?.Name;
    if (responseChar == "gagnant")
    {
        gameOver = game.checkWinner();
        winner = game.Winner?.Name; 
    }

    char IAH = '.';
    int iaX = -1;
    int iaY = -1;
    Move moveIa = null;
    Move move = game.History.LastMove();
    x = move.X;
    y = move.Y;
    string playerId = move.Attacker.Id.ToString();
    string iaId = "";
    string iaTouch = "";

    if (game.History.LastMoveName() == "ia")
    {
        IAH = move.IsHit ? 'X' : 'O';
        iaTouch = move.Touch;
        iaX = move.X;
        iaY = move.Y;
        gameOver = game.checkWinner();
        winner = game.Winner?.Name;
        iaId = move.Attacker.Id.ToString();
        Move move1 = game.History.SecondLastMove();
        x = move1.X;
        y = move1.Y;
        playerId = move1.Attacker.Id.ToString();
    }

    var response = new
    {
        PlayerHit = ia.PlaceShipGrid.Grid[x, y] == 'X',
        PlayerX = x,
        PlayerY = y,
        PlayerId = playerId,
        IaId = iaId,
        IAHit = IAH,
        IATouch = iaTouch,
        IAX = iaX,
        IAY = iaY,
        GameOver = gameOver,
        Winner = winner
    };

    return Results.Ok(response);
});

app.MapGet("/history/{gameId}", (Guid gameId) =>
{
    var game = gameService.GetGame(gameId);

    if (game == null)
    {
        Console.WriteLine($"Game with ID {gameId} not found.");
        return Results.NotFound("Game not found");
    }

    var moves = game.History.GetMoves();
    if (moves == null || !moves.Any())
    {
        Console.WriteLine($"No moves found for game with ID {gameId}");
        return Results.Ok(new List<Move>());
    }

    var history = moves.Select(m => new
    {
        AttackerName = m.Attacker.Name, 
        DefenderName= m.Defender.Name,
        X = m.X,
        Y = m.Y,
        IsHit = m.IsHit,
        Touch = m.Touch,
        PreviousLetter = m.PreviousValue
    }).ToList();

    return Results.Ok(history);
});

app.MapPost("/undo/{gameId}", (Guid gameId) =>
{

    var game = gameService.GetGame(gameId);
 
    if (game == null)
    {
        return Results.NotFound("Game not found");
    }

    var lstMove = game.History.LastMove();
    var x = lstMove.X;
    var y = lstMove.Y;
    var previousValue = lstMove.PreviousValue.ToString();

    var success = game.History.RemoveMove();
    lstMove = game.History.LastMove();
    x = lstMove.X;
    y = lstMove.Y;
    previousValue = lstMove.PreviousValue.ToString();


    if (!success)
    {
        return Results.BadRequest("No moves to undo");
    }

    return Results.Ok();
});

app.MapGet("/restart/{gameId}", (Guid gameId) =>
{

    var game = gameService.GetGame(gameId);

    if (game == null)
    {
        return Results.NotFound("Game not found");
    }
    var success = game.History.RemoveMoveAll();


    var PlayerGrid = converter.ConvertCharArrayToList(game.Players[0].PlaceShipGrid.Grid);
    var OpponentGrid = converter.ConvertBoolArrayToList(game.displayOpponentGrid(game.Players[1].PlaceShipGrid));

    var response = new
    {
        GameId = gameId,
        PlayerGrid = PlayerGrid,
        OpponentGrid = OpponentGrid
    };

    return Results.Ok(response);
});
app.Run();

public class GameSetupRequest
{
    public List<List<char>> Grid { get; set; }
    public List<ShipN> Ships { get; set; }
}

public class ShipN
{
    public char Letter { get; set; }
    public int Length { get; set; }
    public int StartX { get; set; }
    public int StartY { get; set; }
    public bool IsHorizontal { get; set; }
}

