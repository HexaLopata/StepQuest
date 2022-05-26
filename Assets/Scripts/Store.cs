using UnityEngine;

public class Store
{
    private static Store _instance;
    private static MonoStore _monoCore;
    private static EffectManager _effectManager;

    public static Store Instance 
    {
        get 
        {
            if(_instance is null)
                _instance = new Store();

            return _instance;
        }
    }

    public MonoStore MonoStore
    {
        get => _monoCore;
        set
        {
            _monoCore = value;
            UpdateFields();
        }
    }

    public GameField GameField { get; private set; }
    public TurnManager TurnManager { get; private set; }
    public EffectManager EffectManager { get; private set; }

    private void UpdateFields()
    {
        GameField = _monoCore.GameField;
        TurnManager = _monoCore.TurnManager;
        EffectManager = _monoCore.EffectManager;
    }


    private Store() { }
}