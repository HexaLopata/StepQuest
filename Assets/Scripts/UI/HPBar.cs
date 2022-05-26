using UnityEngine;

public class HPBar : MonoBehaviour
{
    private Transform _healthSprite;
    private Transform _borderSprite;
    private Health _health;
    private float _lenght = 1;

    void Start()
    {
        var children = GetComponentsInChildren<Transform>();
        for(int i = 0; i < children.Length; i++)
        {
            if(children[i].name == "Border")
                _borderSprite = children[i].transform;
            else if(children[i].name == "Health")
                _healthSprite = children[i].transform;
        }
        _health = GetComponentInParent<Health>();
        _lenght = _healthSprite.localScale.x;
        _health.onHealthChanged.AddListener(UpdateUI);
        UpdateUI();
    }

    void UpdateUI()
    {
        var ratio = (float)_health.Amount / _health.MaxAmount;
        _healthSprite.localScale = new Vector2(_lenght * ratio, _healthSprite.localScale.y);
    }
}
