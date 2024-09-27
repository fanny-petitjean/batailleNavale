namespace BattleShip.App.Service;

public class GameState
{
    public char[,] PlayerGrid { get; private set; } = new char[10, 10];
    public bool?[,] OpponentGrid { get; private set; } = new bool?[10, 10];
    public string GameId { get; set; }

    public void InitializePlayerGrid(char[,] grid)
    {
        PlayerGrid = grid;
    }

    public void UpdateOpponentGrid(int x, int y, bool isHit)
    {
        OpponentGrid[x, y] = isHit;
    }
}