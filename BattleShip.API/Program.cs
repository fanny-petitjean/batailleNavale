using BattleShip.App;
using BattleShip.Models;
using Microsoft.AspNetCore.Mvc;
using BattleShip.App.Service;
using System;
using System.Collections.Generic;
using BattleShip.API;

var builder = WebApplication.CreateBuilder(args);
var gameService = new GameService();
// Configuration CORS pour Blazor
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorApp",
        builder =>
        {
            builder.WithOrigins("https://localhost:7087")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
        });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<GameService>();

var app = builder.Build();

app.UseCors("AllowBlazorApp");
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
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
        Console.WriteLine("Jeu non trouvé pour l'ID : " + gameId);
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
    if (game.history.LastMoveName()=="ia")
    {
        Console.WriteLine("Dernier coup joué par l'IA");
        IAH = game.history.LastMove().isHit ? 'X' : 'O';
        iaX = game.history.LastMove().x;
        iaY = game.history.LastMove().y;
        Console.WriteLine($"Dernier coup joué par l'IA : {iaX}, {iaY}");
        gameOver = game.checkWinner();
        winner = game.winner?.name;
    }



    var response = new
    {
        PlayerHit = ia.placeShipGrid.Grid[x, y] == 'X',
        IAHit = IAH,
        IAX = iaX,
        IAY = iaY,
        GameOver = gameOver,
        Winner = winner,

    };

    Console.WriteLine("Réponse de l'attaque : " + response);
    Console.WriteLine($"Sortie de la méthode /attack avec gameId: {gameId}.");

    return Results.Ok(response);
});


app.Run();



