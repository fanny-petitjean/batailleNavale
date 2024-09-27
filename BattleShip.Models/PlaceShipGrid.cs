namespace BattleShip.App;
using System;
public class PlaceShipGrid
{   
    private const int GridSize = 10;
    private const char EmptyCell = '\0';
    public List<Ship> ships = new List<Ship>
    {
        new Ship('A', 4, true,0,1),
        new Ship('B', 4, true, 0, 1),
        new Ship('C', 3, true, 0, 4),
        new Ship('C', 3, true,4,0),
        new Ship('D', 2,false,6,1),
        new Ship('E', 2, true, 0, 1),
        new Ship('F', 1, false, 0, 1)
    };
    
    public char[,] Grid { get; private set; }
    public PlaceShipGrid(char[,] grid)
    {
        InitializeGrid(grid);
        PlaceAllShips(grid);
    }
    private void InitializeGrid(char[,] grid)
    {
        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize; j++)
            {
                grid[i, j] = EmptyCell;
            }
        }
    }
    private void PlaceAllShips(char[,] grid)
    {
        foreach (var ship in ships)
        {
            PlaceShip(grid, ship);
        }
    }
    
    private void  PlaceShip(char[,] grid, Ship ship)
    {
        bool placed = false;
        Random randomplace = Random.Shared;

        while (!placed)
        {
            ship.isHorizontal = randomplace.Next(2) == 0;
            int x = randomplace.Next(GridSize);
            int y = randomplace.Next(GridSize);

            if (ship.isHorizontal)
            {
                if (y + ship.size <= GridSize && IsSpaceAvailable(grid, x, y, ship.size, ship.isHorizontal))
                {
                    for (int i = 0; i < ship.size; i++)
                    {
                        grid[x,y+i] = ship.letter;
                    }
                    placed = true;
                    ship.positionX = x;
                    ship.positionY = y;
                }
            }
            else
            {
                if (x + ship.size <= GridSize && IsSpaceAvailable(grid, x, y, ship.size, ship.isHorizontal))
                {
                    for (int i = 0; i < ship.size; i++)
                    {
                        grid[x+i,y]=ship.letter;
                    }
                    placed = true;
                    ship.positionX = x;
                    ship.positionY = y;
                }
            }
        }
    }
    private bool IsSpaceAvailable(char[,] grid, int x, int y, int shipSize, bool isHorizontal)
    {
        for (int i = 0; i < shipSize; i++)
        {
            if (isHorizontal)
            {
                if (y + i >= GridSize || grid[x, y + i] != EmptyCell)
                    return false;  
            }
            else
            {
                if (x + i >= GridSize || grid[x + i, y] != EmptyCell)
                    return false;  
            }
        }
        return true;  
    }
// char [,] grid generatedhrid = PlaceShipGrid.Grid
} 