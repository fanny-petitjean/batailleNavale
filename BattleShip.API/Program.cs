using BattleShip.App;
using BattleShip.Models;
using Microsoft.AspNetCore.Mvc;
using BattleShip.App.Service;
using System;
using System.Collections.Generic;
using BattleShip.API;
using Grpc.AspNetCore.Web;
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

app.MapPost("/newGame", () =>
{
    var player = new Player("Player 1", false);
    var ia = new Player("ia", true);

    var players = new List<Player> { player, ia };
    var game = new Game(players, 0);

    Guid guid = gameService.AddGame(game);

    var PlayerGrid = converter.ConvertCharArrayToList(player.placeShipGrid.Grid);
    var OpponentGrid = converter.ConvertBoolArrayToList(game.displayOpponentGrid(ia.placeShipGrid));
    var response = new
    {
        GameId = guid.ToString(),
        Player = player.name,
        Opponent = ia.name,
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

    char responseChar = game.attack(player, ia, x, y);

    bool gameOver = false;
    string winner = game.winner?.name;
    if (responseChar == 'W')
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

    if (game.history.LastMoveName() == "ia")
    {
        IAH = game.history.LastMove().isHit ? 'X' : 'O';
        iaX = game.history.LastMove().x;
        iaY = game.history.LastMove().y;
        gameOver = game.checkWinner();
        winner = game.winner?.name;
        moveIa = game.history.LastMove();
        move = game.history.LastMove();
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
        IAX = iaX,
        IAY = iaY,
        GameOver = gameOver,
        Winner = winner
    };

    return Results.Ok(response);
});

app.Run();