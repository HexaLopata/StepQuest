using System.Collections.Generic;
using UnityEngine;

public class GameFieldView : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private GameObject _redCellPrefab;
    [SerializeField] private GameObject _grayCellPrefab;
    [SerializeField] private GameObject _yellowCellPrefab;
    private LinkedList<GameObject> _cells = new LinkedList<GameObject>();
    private LinkedList<GameObject> _yellowCells = new LinkedList<GameObject>();

    void Start()
    {
        Store.Instance.TurnManager.onChangeTurn.AddListener(OnPlayerTurn);
        Store.Instance.GameField.onCellRightClicked.AddListener(ShowInteractionArea);
        Store.Instance.GameField.onCellClicked.AddListener((cell) => ClearYellow());
        _player.onWeaponChanged.AddListener((weapon) => OnPlayerTurn(_player));
    }

    void OnPlayerTurn(Entity entity)
    {
        Clear();
        if (entity == _player)
        {
            var area = _player.GetComponentInChildren<IGameArea>();
            var cells = area.GetAvailableCells();
            foreach (var cell in cells)
            {
                var hasEnemy = false;
                foreach (var e in cell.Entities)
                {
                    if (e is Enemy)
                    {
                        MakeRed(cell);
                        hasEnemy = true;
                        break;
                    }
                }

                if (!hasEnemy)
                    MakeGray(cell);
            }
        }
    }

    public void MakeRed(Cell cell)
    {
        var position = Store.Instance.GameField.GameToWorldPosition(cell.GamePosition);
        _cells.AddLast(Instantiate(_redCellPrefab, position, new Quaternion()));
    }

    public void MakeGray(Cell cell)
    {
        var position = Store.Instance.GameField.GameToWorldPosition(cell.GamePosition);
        _cells.AddLast(Instantiate(_grayCellPrefab, position, new Quaternion()));
    }

    public void MakeYellow(Cell cell)
    {
        var position = Store.Instance.GameField.GameToWorldPosition(cell.GamePosition);
        _yellowCells.AddLast(Instantiate(_yellowCellPrefab, position, new Quaternion()));
    }

    public void ShowInteractionArea(Cell cell)
    {
        ClearYellow();
        foreach (var entity in cell.Entities)
        {
            var area = entity.GetComponentInChildren<IGameArea>();
            
            if (area != null)
            {
                var cells = area.GetAvailableCells();
                foreach (var c in cells)
                {
                    MakeYellow(c);
                }
                return;
            }
        }
    }

    public void ClearYellow()
    {
        while (_yellowCells.First != null)
        {
            Destroy(_yellowCells.First.Value);
            _yellowCells.RemoveFirst();
        }
    }

    public void Clear()
    {
        while (_cells.First != null)
        {
            Destroy(_cells.First.Value);
            _cells.RemoveFirst();
        }
        ClearYellow();
    }
}