using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable] 
public class ManhattanDistanceSelector : IAreaSelector
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
                var range = Mathf.Abs(position.x - cell.GamePosition.x) +
                            Mathf.Abs(position.y - cell.GamePosition.y);
                if (range <= _range && range != 0)
                    result.Add(cell);
            }
        }
        return result.ToArray();
    }
}