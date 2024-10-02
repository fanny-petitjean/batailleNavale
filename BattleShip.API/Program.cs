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
app.MapPost("/newgame", () =>
{
    var player = new Player("Player 1", false);
    var ia = new Player("IA", true); 
    
    var players = new List<Player> { player, ia };
    var game = new Game(players);
    
    var gameId = Guid.NewGuid();
    games.Add(gameId, game);
    var response = new
    {
        GameId = gameId.ToString(),
        PlayerGrid = player.placeShipGrid.Grid
    };

    return Results.Ok(response);
});

app.MapPost("/attack/{gameId}/{x}/{y}", (Guid gameId, int x, int y) =>
{
    if (!games.ContainsKey(gameId))
    {
        return Results.NotFound("Game not found");
    }

    var game = games[gameId];
    var player = game.players.First(); // Le joueur
    var ia = game.players.Last(); // L'IA

    // Effectuer l'attaque du joueur sur l'IA
    //game.attack(player, ia, x, y);

    // L'IA effectue une attaque aléatoire sur le joueur
    Random random = new Random();
    int iaX = random.Next(10);
    int iaY = random.Next(10);
    //game.attack(ia, player, iaX, iaY);

    // Vérifier si le jeu est terminé
    bool gameOver = game.checkWinner();
    string winner = game.winner?.name;

    // Retourner les résultats
    var response = new
    {
        PlayerHit = ia.placeShipGrid.Grid[x, y] == 'X',
        IAHit = player.placeShipGrid.Grid[iaX, iaY] == 'X',
        IACoordinates = new { iaX, iaY },
        GameOver = gameOver,
        Winner = winner
    };

    return Results.Ok(response);
});
/*app.MapPost("/newGame", () =>
{
    List<Ship> ships = new List<Ship>();
    ships.Add(new Ship('A', 5, true,0,2));
    ships.Add(new Ship('B', 4, true, 0, 2));
    var player = new Player("Player 1" , false);
    List<Player> players = new List<Player>();
    players.Add(player);
    games.Add(new Guid(), new Game(players));
    //retourner la grille affich�e
})
.WithName("StartNewGame")
.WithOpenApi();

app.Run();

app.MapPost("/{idGame}/attack", (Guid idGame) =>
{
    //tester l'attaque
    //retourner �tat du jeu 
    //etat du tir (touch�, rat�)
    // si gagn�, retourner le gagnant

    //retourner la grille affich�e
});*/