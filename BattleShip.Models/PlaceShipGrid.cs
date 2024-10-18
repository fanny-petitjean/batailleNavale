using System;
namespace BattleShip.Models
{
    public class PlaceShipGrid
    {
        private int GridSize { get; set; }
        private const char EmptyCell = '\0';
        public List<Ship> Ships = new List<Ship>
    {
        new Ship('A', 4),
        new Ship('B', 4),
        new Ship('C', 3),
        new Ship('D', 2),
        new Ship('E', 2),
        new Ship('F', 1)
    };

        public char[,] Grid { get; private set; }
        public PlaceShipGrid()
        {

            GridSize = 10;
            Grid = new char[GridSize, GridSize];
            InitializeGrid();
            PlaceAllShips();
        }
        public PlaceShipGrid(int gridSize)
        {

            GridSize = gridSize;
            Grid = new char[GridSize, GridSize];
            InitializeGrid();
            PlaceAllShips();
        }
        public PlaceShipGrid(int gridSize, List<Ship> ships)
        {

            GridSize = gridSize;
            Grid = new char[GridSize, GridSize];
            this.Ships = ships;
            foreach (var ship in ships)
            {
                PlaceShip(Grid, ship, ship.PositionX, ship.PositionY, ship.IsHorizontal);
            }
        
        }
        public PlaceShipGrid(int gridSize, char[,] grid, List<Ship> ships)
        {
            GridSize = gridSize;
            Grid = grid;
            this.Ships = ships;
        }
        private void InitializeGrid()
        {
            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    Grid[i, j] = EmptyCell;
                }
            }
        }
        private void PlaceAllShips()
        {

            foreach (var ship in Ships)
            {
                bool placed = false;
                while (!placed)
                {
                    int x = Random.Shared.Next(GridSize);
                    int y = Random.Shared.Next(GridSize);
                    bool isHorizontal = Random.Shared.Next(2) == 0;
                    if (IsSpaceAvailable(Grid, x, y, ship.Size, isHorizontal))
                    {
                        PlaceShip(Grid, ship, x, y, isHorizontal);
                        placed = true;
                    }
                }
            }
        }
        public void PlaceShip(char[,] grid, Ship ship, int x, int y, bool isHorizontal)
        {
            bool placed = false;
            ship.IsHorizontal = isHorizontal;
            if (ship.IsHorizontal)
            {
                if (y + ship.Size <= GridSize && IsSpaceAvailable(grid, x, y, ship.Size, ship.IsHorizontal))
                {
                    for (int i = 0; i < ship.Size; i++)
                    {
                        grid[x, y + i] = ship.Letter;
                    }
                    ship.PositionX = x;
                    ship.PositionY = y;
                }
            }
            else
            {
                if (x + ship.Size <= GridSize && IsSpaceAvailable(grid, x, y, ship.Size, ship.IsHorizontal))
                {
                    for (int i = 0; i < ship.Size; i++)
                    {
                        grid[x + i, y] = ship.Letter;
                    }
                    ship.PositionX = x;
                    ship.PositionY = y;
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

        public int[][] GetAvailableMoves()
        {
            List<int[]> availableMovesList = new List<int[]>();

            for (int x = 0; x < this.Grid.GetLength(0); x++)
            {
                for (int y = 0; y < this.Grid.GetLength(1); y++)
                {
                    if (this.Grid[x, y] != 'X' && this.Grid[x,y]!='O')
                    {
                        availableMovesList.Add(new int[] { x, y });
                    }
                }
            }
            return availableMovesList.ToArray();
        }
        public int[][] GetPossibleMovesAround(int x, int y, int perimeter)
        {
            List<int[]> possibleMovesList = new List<int[]>();

            int startX = Math.Max(x - perimeter, 0);
            int endX = Math.Min(x + perimeter, GridSize - 1);
            int startY = Math.Max(y - perimeter, 0);
            int endY = Math.Min(y + perimeter, GridSize - 1);

            for (int i = startX; i <= endX; i++)
            {
                for (int j = startY; j <= endY; j++)
                {
                    if (this.Grid[i, j] != 'X' && this.Grid[i, j] != 'O')
                    {
                        possibleMovesList.Add(new int[] { i, j });
                    }
                }
            }

            return possibleMovesList.ToArray();
        }

    }

} 