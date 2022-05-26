using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class Enemy : Entity
{
    private Health _health;
    private Weapon _weapon;
    [SerializeField] private int _skipChance = 30;

    protected override void OnStart()
    {
        base.OnStart();
        Store.Instance.TurnManager.onChangeTurn.AddListener(OnTurn);
        _health = GetComponent<Health>();
        _health.onDeath.AddListener(() => StartCoroutine(Remove()));
    }

    public override void UpdateChildrenComponents()
    {
        _weapon = GetComponentInChildren<Weapon>();
    }

    private void OnTurn(Entity entity)
    {
        if (entity == this)
            OnMyTurn();
    }

    protected virtual void OnMyTurn()
    {
        var directions = new List<Vector2Int>();
        var rightPos = GamePosition + Vector2Int.right;
        var leftPos = GamePosition + Vector2Int.left;
        var upPos = GamePosition + Vector2Int.up;
        var downPos = GamePosition + Vector2Int.down;

        if (Store.Instance.GameField.HasCellHere(rightPos) && Store.Instance.GameField.Cells[rightPos.x, rightPos.y].CanBePassable)
            directions.Add(Vector2Int.right);
        if (Store.Instance.GameField.HasCellHere(leftPos) && Store.Instance.GameField.Cells[leftPos.x, leftPos.y].CanBePassable)
            directions.Add(Vector2Int.left);
        if (Store.Instance.GameField.HasCellHere(downPos) && Store.Instance.GameField.Cells[downPos.x, downPos.y].CanBePassable)
            directions.Add(Vector2Int.down);
        if (Store.Instance.GameField.HasCellHere(upPos) && Store.Instance.GameField.Cells[upPos.x, upPos.y].CanBePassable)
            directions.Add(Vector2Int.up);

        if (directions.Count == 0)
        {
            Store.Instance.TurnManager.AddActionToQueue(
                new GameAction(() => SkipTurn(0), true, this)
            );
        }
        else
        {
            if (TryAttackPlayer(_weapon.GetAvailableCells()))
                return;

            if (Random.Range(0, 101) <= _skipChance)
                Store.Instance.TurnManager.AddActionToQueue(
                new GameAction(() => SkipTurn(0), true, this)
            );
            else
                Move(directions[Random.Range(0, directions.Count)]);
        }
    }

    public bool TryAttackPlayer(List<Vector2Int> positions)
    {
        foreach (var position in positions)
        {
            if (Store.Instance.GameField.HasCellHere(position))
            {
                var entities = Store.Instance.GameField.Cells[position.x, position.y].Entities;
                foreach (var entity in entities)
                {
                    if (entity is Player)
                    {
                        while (_weapon.RemainingAttacks > 0)
                            _weapon.Attack(Store.Instance.GameField.Cells[position.x, position.y]);
                    }
                }
            }
        }

        return _weapon.RemainingAttacks == 0;
    }

    public bool TryAttackPlayer(Cell[] cells)
    {
        foreach (var cell in cells)
        {
            var entities = cell.Entities;
            foreach (var entity in entities)
            {
                if (entity is Player)
                {
                    while (_weapon.RemainingAttacks > 0)
                        _weapon.Attack(cell);
                }
            }
        }

        return _weapon.RemainingAttacks == 0;
    }
}