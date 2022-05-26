using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Collectable : Entity
{
    [SerializeField] private Item _item;
    private SpriteRenderer _sr;


    public Item Item
    {
        get => _item;
        set
        {
            if(_sr is not null)
                _sr.sprite = value?.Image;
            _item = value;
        }
    }

    protected override void OnStart()
    {
        base.OnStart();
        _sr = GetComponent<SpriteRenderer>();
        if(_item is not null)
            _sr.sprite = _item.Image;
    }
}