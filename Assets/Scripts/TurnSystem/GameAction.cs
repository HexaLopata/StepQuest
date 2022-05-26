using System;
using System.Collections;

public class GameAction
{
    public Func<IEnumerator> Routine { get; }
    public bool EndTurn { get; }
    public Entity Performer { get; }

    public GameAction(Func<IEnumerator> routine, bool endTurn, Entity performer)
    {
        Routine = routine;
        EndTurn = endTurn;
        Performer = performer;
    }

    public override string ToString()
    {
        return $"EndTurn: {EndTurn}, Performer: {Performer.name}";
    }
}