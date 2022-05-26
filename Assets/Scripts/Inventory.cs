using UnityEngine.Events;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : GameComponent
{
    private Player _parent;
    private LinkedList<Weapon> _weaponHand = new LinkedList<Weapon>();
    private Weapon _weapon;
    private LinkedList<ActiveItem> _activeItemHand = new LinkedList<ActiveItem>();
    private ActiveItem[] _activeItems;
    private bool _isBuildingAHand = true;
    [SerializeField] private Collectable _collectablePrefab;

    public int WeaponsCount => _weaponHand.Count;
    public Weapon TopWeapon => _weaponHand.First.Value;
    public Weapon EquipedWeapon => _weapon;
    public UnityEvent<Weapon> onChangedWeapon;
    public UnityEvent onChangedInventory;

    protected override void OnStart()
    {
        _parent = GetComponentInParent<Player>();
        _parent.onWeaponChanged.AddListener(UpdateWeapon);
        _weapon = _parent.GetComponentInChildren<Weapon>();
        Store.Instance.TurnManager.onChangeTurn.AddListener(OnTurn);
        onChangedInventory?.Invoke();
    }

    private void UpdateWeapon(Weapon weapon)
    {
        _weapon = weapon;
    }

    private void OnTurn(Entity entity)
    {
        if (entity == _parent)
        {
            _isBuildingAHand = true;
            Clear();
            var collectables = GetCollectables();
            foreach (var c in collectables)
            {
                if (c.Item is Weapon)
                    AddInHand((Weapon)c.Item);
            }
            _isBuildingAHand = false;
        }
    }

    private IEnumerable<Collectable> GetCollectables()
    {
        var cell = Store.Instance.GameField.GetCell(_parent.GamePosition);
        var entities = cell.Entities;
        var collectables = entities.Where(e => e is Collectable).Select(c => (Collectable)c);
        return collectables;
    }

    private void UpdateCollectables()
    {
        var collectables = GetCollectables();
        foreach (var c in collectables)
            Destroy(c.gameObject);

        foreach (var weapon in _weaponHand)
        {
            var collectable = Instantiate(_collectablePrefab, _parent.transform.position, new Quaternion());
            var name = weapon.name;
            collectable.Item = Instantiate(weapon, collectable.transform);
            collectable.Item.name = name;
            collectable.name = name + " (Collectable)";
        }
    }

    public override void UpdateChildrenComponents()
    {
        var weapons = GetComponentsInChildren<Weapon>();
        _weaponHand = new LinkedList<Weapon>(weapons);
        onChangedInventory?.Invoke();
    }

    public void AddInHand(Weapon weapon)
    {
        var name = weapon.name;
        var newWeapon = Instantiate(weapon, transform);
        newWeapon.name = name;
    }

    public void ChangeWeapon()
    {
        if (_weaponHand.Count > 0 && !Store.Instance.TurnManager.IsActionLoopRunning)
        {
            ChangeParent(_weapon, transform);
            var newWeapon = ChangeParent(TopWeapon, _parent.transform);
            _weaponHand.Remove(TopWeapon);
            newWeapon.transform.SetAsFirstSibling();
            UpdateCollectables();
        }
    }

    public void Clear()
    {
        var items = GetComponentsInChildren<Item>();
        foreach (var item in items)
        {
            Destroy(item.gameObject);
        }
        onChangedInventory?.Invoke();
    }
}