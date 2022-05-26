using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{
    private LinkedList<GameAction> _actionQueue = new LinkedList<GameAction>();
    private LinkedList<Entity> _turns = new LinkedList<Entity>();
    private bool _isActionLoopRunning = false;
    private bool _isLastAction = false;
    public LinkedList<Entity> Turns => _turns;
    public bool IsActionLoopRunning => _isActionLoopRunning;

    [SerializeField] private float _delayTime = 1;

    public UnityEvent<Entity> onChangeTurn;

    void Start()
    {
        if (Turns.Count > 0)
            onChangeTurn?.Invoke(Turns.Last.Value);
    }

    public void AddActionToQueue(GameAction action)
    {
        if (!action.Performer.isActiveAndEnabled)
        {
            SetNextTurn();
            return;
        }

        if (_turns.Last.Value != action.Performer || _isLastAction)
            return;

        _actionQueue.AddFirst(action);

        if (action.EndTurn)
            _isLastAction = true;

        if (!_isActionLoopRunning)
        {
            StartCoroutine(PerformActions());
        }
    }

    public void AddEntityToTurnQueue(Entity entity)
    {
        _turns.AddFirst(entity);
        if (_turns.Count == 1)
        {
            onChangeTurn?.Invoke(entity);
        }
    }

    public void SetNextTurn()
    {
        var entity = _turns.Last.Value;
        _turns.RemoveLast();
        _turns.AddFirst(entity);
        onChangeTurn?.Invoke(Turns.Last.Value);
    }

    public IEnumerator PerformActions()
    {
        _isActionLoopRunning = true;
        while (_actionQueue.Count > 0)
        {
            var action = _actionQueue.Last.Value;
            _actionQueue.RemoveLast();
            yield return action.Routine();
            yield return new WaitForSeconds(_delayTime);
            if (action.EndTurn)
            {
                _isActionLoopRunning = false;
                _isLastAction = false;
                SetNextTurn();
                break;
            }
            _isActionLoopRunning = false;
        }
    }
}