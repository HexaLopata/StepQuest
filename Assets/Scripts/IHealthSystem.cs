using UnityEngine.Events;

public interface IHealthSystem
{
    UnityEvent OnHealthChanged { get; }
    UnityEvent OnDeath { get; }
    void GetHit(int amount);
    void Heal(int amount);
}