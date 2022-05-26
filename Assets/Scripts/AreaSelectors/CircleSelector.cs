using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable] 
public class CircleSelector : IAreaSelector
{
    [SerializeField] private int _radius = 5;

    public Cell[] GetAvailableCells(Vector2Int position)
    {
        var result = new List<Cell>();
        for (int i = 0; i < Store.Instance.GameField.Cells.GetLength(0); i++)
        {
            for (int j = 0; j < Store.Instance.GameField.Cells.GetLength(1); j++)
            {
                var cell = Store.Instance.GameField.Cells[i, j];
                var x = position.x - cell.GamePosition.x;
                var y = position.y - cell.GamePosition.y;
                if(x * x + y * y < _radius * _radius)
                    result.Add(cell);
            }
        }
        return result.ToArray();
    }
}