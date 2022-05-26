using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Entity))]
public class Health : MonoBehaviour, IHealthSystem
{
    public UnityEvent onHealthChanged;
    public UnityEvent onDeath;
    [SerializeField] private int _amount = 300;
    [SerializeField] private int _maxAmount = 300;
    [SerializeField] private bool _isVulnerable = true;
    UnityEvent IHealthSystem.OnHealthChanged => onHealthChanged;
    UnityEvent IHealthSystem.OnDeath => onDeath;

    public int Amount
    {
        get => _amount;
        private set
        {
            if (!_isVulnerable)
                return;

            if (value <= 0)
            {
                _amount = 0;
                onDeath?.Invoke();
            }
            else if (value > _maxAmount)
                _amount = _maxAmount;
            else
                _amount = value;

            onHealthChanged?.Invoke();
        }
    }

    public int MaxAmount => _maxAmount;

    public void GetHit(int amount)
    {
        Amount -= amount;
    }

    public void Heal(int amount)
    {
        Amount += amount;
    }
}