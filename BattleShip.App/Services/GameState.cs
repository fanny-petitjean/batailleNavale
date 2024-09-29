using BattleShip.Models;

namespace BattleShip.App.Service;

public class GameState
{
    public char[,] PlayerGrid { get; private set; } = new char[10, 10];
    public bool?[,] OpponentGrid { get; private set; } = new bool?[10, 10];
    public string GameId { get; set; }
    public bool IsPlayerTurn { get; set; } 
    public string WinnerName { get; set; }


    public void InitializePlayerGrid(char[,] grid)
    {
        PlayerGrid = grid;
    }
    public void InitializeOpponentGrid(bool?[,] grid)
    {
        OpponentGrid = grid;
    }

    public void UpdateOpponentGrid(int x, int y, bool isHit)
    {
        OpponentGrid[x, y] = isHit;
    }
    public void UpdatePlayerGrid(int x, int y, char isHit)
    {
        PlayerGrid[x, y] = isHit;
    }

    private char[,] ConvertListToArray(List<List<char>> list)
    {
        if (list == null || list.Count == 0)
            return new char[0, 0]; 

        int rows = list.Count;
        int cols = list[0].Count;

        char[,] array = new char[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                array[i, j] = list[i][j]; 
            }
        }

        return array;
    }

    public char[,] ConvertListToCharArray(List<List<char>> list)
    {
        if (list == null || list.Count == 0)
            return new char[0, 0]; 

        int rows = list.Count; 
        int cols = list[0].Count; 
        char[,] array = new char[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                array[i, j] = list[i][j]; 
            }
        }

        return array; 
    }

    public bool?[,] ConvertListToBoolArray(List<List<bool?>> list)
    {
        if (list == null || list.Count == 0)
            return new bool?[0, 0]; 

        int rows = list.Count;
        int cols = list[0].Count; 
        bool?[,] array = new bool?[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                array[i, j] = list[i][j];
            }
        }

        return array; 
    }


}