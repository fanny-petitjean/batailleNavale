using BattleShip.App;
using BattleShip.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
var games = new Dictionary<Guid, Game>();

app.MapPost("/newGame", () =>
{
    List<Ship> ships = new List<Ship>();
    ships.Add(new Ship('A', 5, true,0,2));
    ships.Add(new Ship('B', 4, true, 0, 2));
    var player = new Player("Player 1", ships);
    List<Player> players = new List<Player>();
    players.Add(player);
    games.Add(new Guid(), new Game(players));
    //retourner la grille affichée
})
.WithName("StartNewGame")
.WithOpenApi();

app.Run();

app.MapPost("/{idGame}/attack", (Guid idGame, Command command) =>
{
    //tester l'attaque
    //retourner état du jeu 
    //etat du tir (touché, raté)
    // si gagné, retourner le gagnant

    //retourner la grille affichée
});