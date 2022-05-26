using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Item, IGameArea
{
    [SerializeField] protected int _damage;
    [SerializeField] protected uint _attacksInTurn = 1;
    [SerializeField] protected uint _currentAttackInTurn = 1;
    private Entity _owner;
    protected Entity Owner
    {
        get
        {
            if (_owner is null)
                _owner = GetComponentInParent<Entity>(); ;
            return _owner;
        }
        set => _owner = value;

    }
    public uint RemainingAttacks => _currentAttackInTurn;


    protected override void OnStart()
    {
        Store.Instance.TurnManager.onChangeTurn.AddListener(OnChangeTurn);
        _owner = GetComponentInParent<Entity>();
    }

    public void OnChangeTurn(Entity entity)
    {
        if (entity == _owner)
        {
            _currentAttackInTurn = _attacksInTurn;
        }
    }

    public abstract Cell[] GetAvailableCells();
    public abstract IEnumerator AttackRoutine(Cell cell);

    private IEnumerator AttackRoutineTemplate(Cell cell)
    {
        yield return AttackRoutine(cell);
        yield return null;
        var entities = new Entity[cell.Entities.Count];
        cell.Entities.CopyTo(entities);
        foreach (var entity in entities)
        {
            IHealthSystem health = null;
            if (entity.TryGetComponent<IHealthSystem>(out health))
            {
                health.GetHit(_damage);
            }
        }
    }

    public virtual void Attack(Cell cell)
    {
        Store.Instance.TurnManager.AddActionToQueue(
            new GameAction(
                () => AttackRoutineTemplate(cell),
                _currentAttackInTurn == 1,
                _owner
            )
        );
        _currentAttackInTurn--;
    }
}