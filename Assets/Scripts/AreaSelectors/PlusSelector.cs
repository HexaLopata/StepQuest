using System;
using System.Collections.Generic;
using UnityEngine;

using static System.Math;

[Serializable] 
public class PlusSelector : IAreaSelector
{
    [SerializeField] private int _range = 5;

    public Cell[] GetAvailableCells(Vector2Int position)
    {
        var result = new List<Cell>();
        for (int i = 0; i < Store.Instance.GameField.Cells.GetLength(0); i++)
        {
            for (int j = 0; j < Store.Instance.GameField.Cells.GetLength(1); j++)
            {
                var cell = Store.Instance.GameField.Cells[i, j];
                var y = position.y - cell.GamePosition.y;
                var x = position.x - cell.GamePosition.x;
                if((x == 0 && _range >= Abs(y)) || y == 0 && _range >= Abs(x))
                    result.Add(cell);
            }
        }
        return result.ToArray();
    }
}