namespace BattleShip.API
{
    using System;
    using System.Collections.Generic;

    public class Converter
    {
        /// <summary>
        /// Convertit un tableau multidimensionnel de caractères en une liste de listes de caractères.
        /// </summary>
        /// <param name="array">Le tableau de caractères à convertir.</param>
        /// <returns>Une liste de listes de caractères.</returns>
        public List<List<char>> ConvertCharArrayToList(char[,] array)
        {
            var list = new List<List<char>>();
            int rows = array.GetLength(0); // Nombre de lignes
            int cols = array.GetLength(1); // Nombre de colonnes

            for (int i = 0; i < rows; i++)
            {
                var rowList = new List<char>(); // Nouvelle liste pour chaque ligne
                for (int j = 0; j < cols; j++)
                {
                    rowList.Add(array[i, j]); // Ajouter l'élément à la liste de la ligne
                }
                list.Add(rowList); // Ajouter la liste de la ligne à la liste principale
            }

            return list; // Retourner la liste de listes
        }

        /// <summary>
        /// Convertit un tableau multidimensionnel de booléens nullable en une liste de listes de booléens nullable.
        /// </summary>
        /// <param name="array">Le tableau de booléens nullable à convertir.</param>
        /// <returns>Une liste de listes de booléens nullable.</returns>
        public List<List<bool?>> ConvertBoolArrayToList(bool?[,] array)
        {
            var list = new List<List<bool?>>();
            int rows = array.GetLength(0); // Nombre de lignes
            int cols = array.GetLength(1); // Nombre de colonnes

            for (int i = 0; i < rows; i++)
            {
                var rowList = new List<bool?>(); // Nouvelle liste pour chaque ligne
                for (int j = 0; j < cols; j++)
                {
                    rowList.Add(array[i, j]); // Ajouter l'élément à la liste de la ligne
                }
                list.Add(rowList); // Ajouter la liste de la ligne à la liste principale
            }

            return list; // Retourner la liste de listes
        }
    }

}
