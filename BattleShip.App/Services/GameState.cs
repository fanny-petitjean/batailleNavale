public class GameState
{
    public char[,] PlayerGrid { get; private set; }  // Grille du joueur
    public bool?[,] OpponentGrid { get; private set; }  // Grille de l'adversaire (masquée)
    public string GameId { get; private set; }

    public void InitializeNewGame(char[,] playerGrid, string gameId)
    {
        PlayerGrid = playerGrid;
        OpponentGrid = new bool?[10, 10];  // Grille de l'adversaire masquée
        GameId = gameId;
    }

    public void UpdateOpponentGrid(int x, int y, bool hit)
    {
        OpponentGrid[x, y] = hit;  // Mise à jour de la grille avec touché ou raté
    }

    public void UpdatePlayerGrid(int x, int y, bool hit)
    {
        PlayerGrid[x, y] = hit ? 'X' : 'O';  // Mise à jour de la grille du joueur
    }
}