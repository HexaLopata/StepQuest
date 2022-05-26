using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Health))]
public class Player : Entity
{
    private Weapon _weapon;
    public UnityEvent<Weapon> onWeaponChanged;

    public Weapon Weapon => _weapon;

    protected override void OnStart()
    {
        base.OnStart();
        Store.Instance.GameField.onCellClicked.AddListener(Attack);
        GetComponent<Health>().onDeath.AddListener(() => SceneManager.LoadScene(0));
    }

    public override void UpdateChildrenComponents()
    {
        var weapon = GetComponentInChildren<Weapon>();
        if (weapon is not null)
        {
            _weapon = weapon;
            onWeaponChanged?.Invoke(_weapon);
        }
    }

    protected override void OnUpdate()
    {
        if (Store.Instance.TurnManager.Turns.Last.Value == this)
        {
            if (Input.GetButtonDown("MoveUp"))
                Move(new Vector2Int(0, 1), true);
            if (Input.GetButtonDown("MoveDown"))
                Move(new Vector2Int(0, -1), true);
            if (Input.GetButtonDown("MoveLeft"))
                Move(new Vector2Int(-1, 0), true);
            if (Input.GetButtonDown("MoveRight"))
                Move(new Vector2Int(1, 0), true);
        }
    }

    void Attack(Cell cell)
    {
        var cells = _weapon.GetAvailableCells();
        if (cells.Contains(cell))
        {
            _weapon.Attack(cell);
        }
    }
}