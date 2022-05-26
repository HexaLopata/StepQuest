using UnityEngine;

public class MonoStore : MonoBehaviour
{
    public GameField GameField => _gameField;
    public TurnManager TurnManager => _turnManager;
    public EffectManager EffectManager => _effectManager;

    [SerializeField] private GameField _gameField;
    [SerializeField] private TurnManager _turnManager;
    [SerializeField] private EffectManager _effectManager;

    private Store _store;

    void Awake()
    {
        _store = Store.Instance;
        _store.MonoStore = this;
        Debug.Log("Core inited");
    }
}