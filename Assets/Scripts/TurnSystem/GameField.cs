using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GameField : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int _width = 19;
    [SerializeField] private int _height = 7;
    [SerializeField] private float _tileSize = 1;

    public int Width => _width;
    public int Height => _height;
    public float TileSize => _tileSize;
    public Cell[,] Cells;
    public UnityEvent<Cell> onCellClicked;
    public UnityEvent<Cell> onCellRightClicked;

    void Awake()
    {
        Cells = new Cell[_width, _height];

        for (int i = 0; i < _width; i++)
            for (int j = 0; j < _height; j++)
                Cells[i, j] = new Cell(new Vector2Int(i, j));
    }

    public bool HasCellHere(int x, int y)
    {
        return x >= 0 && x < Width && y >= 0 && y < Height;
    }

    public bool HasCellHere(Vector2Int position)
    {
        return HasCellHere(position.x, position.y);
    }

    public Cell GetCell(Vector2Int position)
    {
        return Cells[position.x, position.y];
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        var center = new Vector3(transform.position.x + (Width >> 1) * _tileSize, transform.position.y + (Height >> 1) * _tileSize);
        Gizmos.DrawWireCube(center, new Vector3(Width * _tileSize, Height * _tileSize));
    }

    public Vector2Int WorldToGamePosition(Vector2 worldPosition)
    {
        worldPosition -= (Vector2)transform.position;
        worldPosition += Vector2.one * (_tileSize / 2);
        return new Vector2Int((int)Mathf.Floor(worldPosition.x / _tileSize), (int)Mathf.Floor(worldPosition.y / _tileSize));
    }

    public Vector2Int ScreenToGamePosition(Vector2 screenPosition)
    {
        var worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        return WorldToGamePosition(worldPosition);
    }

    public Vector2 GameToWorldPosition(Vector2Int gamePosition)
    {
        return gamePosition + (Vector2)transform.position;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var cellPosition = ScreenToGamePosition(eventData.position);
        if (eventData.button == PointerEventData.InputButton.Left)
            onCellClicked?.Invoke(Cells[cellPosition.x, cellPosition.y]);
        else if (eventData.button == PointerEventData.InputButton.Right)
            onCellRightClicked?.Invoke(Cells[cellPosition.x, cellPosition.y]);
    }
}