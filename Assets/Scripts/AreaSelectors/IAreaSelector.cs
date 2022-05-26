using UnityEngine;

public interface IAreaSelector
{
    Cell[] GetAvailableCells(Vector2Int position);
}