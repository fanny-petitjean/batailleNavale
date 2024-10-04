namespace BattleShip.API
{
    using System;
    using System.Collections.Generic;

    public class Converter
    {
 
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

        public List<List<bool?>> ConvertBoolArrayToList(bool?[,] array)
        {
            var list = new List<List<bool?>>();
            int rows = array.GetLength(0); 
            int cols = array.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                var rowList = new List<bool?>(); 
                for (int j = 0; j < cols; j++)
                {
                    rowList.Add(array[i, j]); 
                }
                list.Add(rowList); 
            }

            return list; 
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
    }

}
