using System.Collections.Generic;
using UnityEngine;

public class DropSystem : GameComponent
{
    [SerializeField] private Collectable _collectablePrefab;
    [SerializeField] private bool _absolutePositioning = false;
    [SerializeField] private Vector2Int _dropPosition = Vector2Int.zero;
    [SerializeField] private List<Drop> _drop;
    private Entity _owner;

    public Vector2Int DropPosition { get; set; }

    protected override void OnStart()
    {
        _owner = GetComponent<Entity>();
    }

    public void Drop()
    {
        var dropPosition = Store.Instance.GameField.GameToWorldPosition(_dropPosition);
        if (!_absolutePositioning)
        {
            dropPosition = Store.Instance.GameField.GameToWorldPosition(_owner.GamePosition + _dropPosition);
        }

        foreach (var dropItem in _drop)
        {
            if (dropItem.DropChance >= Random.Range(1, 101))
            {
                var collectable = Instantiate(
                    _collectablePrefab,
                    dropPosition,
                    new Quaternion()
                );
                var name = dropItem.Item.name;
                collectable.Item = Instantiate(dropItem.Item, collectable.transform);
                collectable.Item.name = name;
                collectable.name = name + " (Collectable)";
            }
        }

    }
}
