using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Passability
{
    Passable,
    Impassable
}

public class Entity : GameComponent
{
    [SerializeField] protected Vector2Int _gamePosition = Vector2Int.zero;
    [SerializeField] protected float _moveTime = 1;
    [SerializeField] private bool _isInEntityQueue = true;
    [SerializeField] private bool _gamePositionByWorldPosition = true;
    [SerializeField] private Passability _passability = Passability.Impassable;
    public UnityEvent<Vector2> onStartMoving;
    public UnityEvent onEndMoving;
    public UnityEvent<Entity> onClicked;

    public Passability Passability => _passability;
    public List<Func<IEnumerator>> OnDeathCoroutines { get; } = new List<Func<IEnumerator>>();
    public Vector2Int GamePosition => _gamePosition;

    protected override void OnStart()
    {
        if (_gamePositionByWorldPosition)
        {
            var gamePosition = Store.Instance.GameField.WorldToGamePosition(transform.position);
            if (Store.Instance.GameField.HasCellHere(gamePosition))
                _gamePosition = gamePosition;
        }

        StartCoroutine(Move(Store.Instance.GameField.Cells[_gamePosition.x, _gamePosition.y]));
        if (_isInEntityQueue)
            Store.Instance.TurnManager.AddEntityToTurnQueue(this);
    }

    public void Move(Vector2Int direction, bool endTurn = true)
    {
        direction.Clamp(new Vector2Int(-1, -1), new Vector2Int(1, 1));
        if (direction.x != 0 && direction.y != 0)
            Debug.LogError("Invalid direction");

        var newPos = _gamePosition + direction;

        if (Store.Instance.GameField.HasCellHere(newPos) && Store.Instance.GameField.Cells[newPos.x, newPos.y].CanBePassable)
        {
            Store.Instance.TurnManager.AddActionToQueue(
                new GameAction(
                    () => Move(Store.Instance.GameField.Cells[newPos.x, newPos.y]),
                    endTurn,
                    this
                )
            );
        }
    }

    public IEnumerator Move(Cell cell)
    {
        onStartMoving?.Invoke(cell.GamePosition - _gamePosition);

        Store.Instance.GameField.Cells[_gamePosition.x, _gamePosition.y]?.Entities.Remove(this);
        _gamePosition = cell.GamePosition;
        cell.Entities.Add(this);

        var targetPosition = Store.Instance.GameField.TileSize * (Vector2)cell.GamePosition +
                             (Vector2)Store.Instance.GameField.transform.position;
        float startTime = Time.realtimeSinceStartup;
        float t = 0f;

        while (t < 1f)
        {
            t = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / _moveTime);
            transform.position = Vector2.Lerp(transform.position, targetPosition, t);
            yield return null;
        }

        onEndMoving?.Invoke();
    }

    public IEnumerator Remove()
    {
        yield return ExecuteRemoveCoroutines();
        Destroy(gameObject);
    }

    public IEnumerator ExecuteRemoveCoroutines()
    {
        foreach (var coroutine in OnDeathCoroutines)
        {
            yield return coroutine();
        }
        yield return null;
    }

    protected override void OnRemove()
    {
        if (Store.Instance.TurnManager.Turns.Contains(this))
            Store.Instance.TurnManager.Turns.Remove(this);

        Store.Instance.GameField.Cells[_gamePosition.x, _gamePosition.y]?.Entities.Remove(this);
    }

    public IEnumerator SkipTurn(float delay)
    {
        yield return new WaitForSeconds(delay);
    }
}
