using BattleShip.Models;
using System.Net.Http.Json;
using static BattleShip.App.Pages.GameIA;

namespace BattleShip.App.Service;

public class GameState
{
    public char[,] PlayerGrid { get; private set; }
    public static string[,] PlayerGridImagesOld { get; set; }
    public event Action OnChange;
    private void NotifyStateChanged() => OnChange?.Invoke();
    public string[,] PlayerGridImages { get; private set; }
    public bool?[,] OpponentGrid { get; private set; }
    public string GameId { get; set; }
    public bool IsPlayerTurn { get; set; }
    public string WinnerName { get; set; }
    public int GridSize { get; set; }
    public DateTime? StartTime;
    public DateTime? EndTime;
    public int TotalGames { get; private set; }

    public Dictionary<string, int> PlayerLosses { get; set; } = new Dictionary<string, int>();

    private Dictionary<string, int> playerVictories = new Dictionary<string, int>();
    public Dictionary<string, int> ShipsSunkByPlayer { get; private set; } = new Dictionary<string, int>();

    public List<MoveDto> Moves { get; set; } = new List<MoveDto>();
    public void AddMove(MoveDto move)
    {
        Moves.Add(move);
    }

    public bool RemoveMove(MoveDto move)
    {
        return Moves.Remove(move);
    }
    public void RemoveMoveAll()
    {
        Moves = new List<MoveDto>();
    }

    public void InitializeNewGame(char[,] playerGrid, string gameId, int gridSize, string[,] playerGridImage, bool?[,] opponentGrid)
    {
        NotifyStateChanged(); 

        GridSize = gridSize;
        PlayerGrid = playerGrid;
        OpponentGrid = opponentGrid;
        PlayerGridImages = playerGridImage;

        PlayerGridImagesOld = new string[gridSize, gridSize];
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                PlayerGridImagesOld[i, j] = playerGridImage[i, j];
            }
        }

        GameId = gameId;
        Console.WriteLine("GameId test: " + GameId);

    }
    public void InitializePlayerGrid(char[,] grid)
    {
        PlayerGrid = grid;
    }
    public void InitializeOpponentGrid(bool?[,] grid)
    {
        OpponentGrid = grid;
    }

    public void UpdateOpponentGrid(int x, int y, bool? hit)
    {
        OpponentGrid[x, y] = hit;
    }
    public void RestartGridImage()
    {
        PlayerGridImages = new string[GridSize, GridSize];
        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize; j++)
            {
                PlayerGridImages[i, j] = PlayerGridImagesOld[i, j];
            }
        }
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
    public List<List<char>> ConvertCharArrayToList(char[,] array)
    {
        var list = new List<List<char>>();
        int rows = array.GetLength(0);
        int cols = array.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            var rowList = new List<char>();
            for (int j = 0; j < cols; j++)
            {
                rowList.Add(array[i, j]);
            }
            list.Add(rowList);
        }

        return list;
    }
    public void UpdatePlayerGridImage(int x, int y, string image)
    {
        PlayerGridImages[x, y] = image;
    }
    public void RestartGame()
    {
  
        NotifyStateChanged(); 
    }

    public void IncrementPlayerVictory(string playerName)
    {
        if (playerVictories.ContainsKey(playerName))
        {
            playerVictories[playerName]++;
        }
        else
        {
            playerVictories[playerName] = 1;
        }
    }

    public Dictionary<string, int> GetPlayerVictories()
    {
        return playerVictories;
    }
    
    public Dictionary<string, int> GetPlayerLosses()
    {
        
        return PlayerLosses;
    }

    public void StartGame()
    {
        StartTime = DateTime.Now;
        EndTime = null;
    }
    public void EndGame()
    {
        TotalGames++;
        EndTime = DateTime.Now;
    }
    public TimeSpan GetGameDuration()
    {
        if (StartTime.HasValue && EndTime.HasValue)
        {
            return EndTime.Value - StartTime.Value;
        }
        return TimeSpan.Zero; 
    }
    
    public void IncrementShipsSunk(string playerName)
    {
        if (ShipsSunkByPlayer.ContainsKey(playerName))
        {
            ShipsSunkByPlayer[playerName]++;
        }
        else
        {
            ShipsSunkByPlayer[playerName] = 1;
        }
    }

    public string GetTopSinkingPlayer()
    {
        return ShipsSunkByPlayer
            .OrderByDescending(kvp => kvp.Value)
            .FirstOrDefault().Key;
    }

   
    

}