using BattleShip.App;
using BattleShip.Models;
using Microsoft.AspNetCore.Mvc;
using BattleShip.App.Service;
using System;
using System.Collections.Generic;
using BattleShip.API;
using Grpc.AspNetCore.Web;
using System.Text.Json;

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


builder.Services.AddGrpc();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<GameService>();

var app = builder.Build();
app.UseCors("AllowAll");
app.UseCors("AllowBlazorApp");

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
        Console.WriteLine($"test : {row.Letter}, {row.Length}, {row.IsHorizontal}, {row.StartX}, {row.StartY}) ");
        shipList.Add(new Ship(row.Letter, row.Length, row.IsHorizontal, row.StartX, row.StartY));
    }
    var placeGrid = new PlaceShipGrid(gridSize, shipList);
    for(int i = 0; i < gridSize; i++)
    {
        for (int j = 0; j < gridSize; j++)
        {
            Console.Write($"t{placeGrid.Grid[i, j]}  ");
        }
        Console.WriteLine();
    }
    var player = new Player("Player 1", false, placeGrid);

    Player opponent;
    if (difficulty.HasValue)
    {
        // Cr�er un joueur IA avec la difficult� sp�cifi�e
        opponent = new Player("ia", true, gridSize, difficulty.Value);
        Console.WriteLine($"Cr�ation de l'IA avec une difficult� de niveau {difficulty.Value}");
    }
    else
    {
        // Cr�er un joueur IA normal sans difficult�
        opponent = new Player("Player 2", true, gridSize);
        Console.WriteLine("Cr�ation d'une partie contre un autre joueur");
    }

    Console.WriteLine($"gridSize : {gridSize}");
    var players = new List<Player> { player, opponent };
    Console.WriteLine($"players : {players.Count}");
    var game = new Game(players, 0);
    Console.WriteLine($"game : {game.players.Count}");

    Guid guid = gameService.AddGame(game);
    Console.WriteLine($"guid : {guid}");

    var PlayerGrid = converter.ConvertCharArrayToList(player.placeShipGrid.Grid);
    Console.WriteLine($"PlayerGrid : {PlayerGrid.Count}");
    var OpponentGrid = converter.ConvertBoolArrayToList(game.displayOpponentGrid(opponent.placeShipGrid));
    Console.WriteLine($"OpponentGrid : {OpponentGrid.Count}");

    var response = new
    {
        GameId = guid.ToString(),
        Player = player.name,
        Opponent = opponent.name,
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

    var player = game.players.First(); 
    var ia = game.players.Last();

    string responseChar = game.attack(player, ia, x, y);

    bool gameOver = false;
    string winner = game.winner?.name;
    if (responseChar == "gagnant")
    {
        gameOver = game.checkWinner();
        winner = game.winner?.name; 
    }

    char IAH = '.';
    int iaX = -1;
    int iaY = -1;
    Move moveIa = null;
    Move move = game.history.LastMove();
    x = move.x;
    y = move.y;
    string playerId = move.attacker.id.ToString();
    string iaId = "";
    string iaTouch = "";

    if (game.history.LastMoveName() == "ia")
    {
        IAH = move.isHit ? 'X' : 'O';
        iaTouch = move.touch;
        iaX = move.x;
        iaY = move.y;
        gameOver = game.checkWinner();
        winner = game.winner?.name;
        iaId = move.attacker.id.ToString();
        Move move1 = game.history.SecondLastMove();
        x = move1.x;
        y = move1.y;
        playerId = move1.attacker.id.ToString();
    }

    var response = new
    {
        PlayerHit = ia.placeShipGrid.Grid[x, y] == 'X',
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

    var moves = game.history.GetMoves();
    if (moves == null || !moves.Any())
    {
        Console.WriteLine($"No moves found for game with ID {gameId}");
        return Results.Ok(new List<Move>());
    }
    Console.WriteLine($"History for game with ID {gameId} retrieved successfully.");

    var history = moves.Select(m => new
    {
        AttackerName = m.attacker.name, 
        DefenderName= m.defender.name,
        X = m.x,
        Y = m.y,
        IsHit = m.isHit,
        Touch = m.touch,
        PreviousLetter = m.previousValue
    }).ToList();
    Console.WriteLine($"History for game with ID {history.ToString} retrieved successfully.");
    foreach (var move in history)
    {
        Console.WriteLine($"Move: {JsonSerializer.Serialize(move)}");
    }




    return Results.Ok(history);
});

app.MapPost("/undo/{gameId}", (Guid gameId) =>
{

    var game = gameService.GetGame(gameId);
 
    if (game == null)
    {
        return Results.NotFound("Game not found");
    }

    var lstMove = game.history.LastMove();
    var x = lstMove.x;
    var y = lstMove.y;
    var previousValue = lstMove.previousValue.ToString();
    Console.WriteLine($"x:  {x} et y : {y} ");
    Console.WriteLine($"previous : {previousValue}");
    var success = game.history.RemoveMove();
    lstMove = game.history.LastMove();
    x = lstMove.x;
    y = lstMove.y;
    previousValue = lstMove.previousValue.ToString();
    Console.WriteLine($" AFTER x:  {x} et y : {y} ");
    Console.WriteLine($"AFTER previous : {previousValue}");

    if (!success)
    {
        return Results.BadRequest("No moves to undo");
    }

    return Results.Ok();
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

