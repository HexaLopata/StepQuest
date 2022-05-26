using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cell
{
    public List<Entity> Entities { get; } = new List<Entity>();
    public Vector2Int GamePosition { get; private set; }
    public bool CanBePassable => Entities.All((e) => e.Passability == Passability.Passable);

    public Cell(Vector2Int position)
    {
        GamePosition = position;
    }
}