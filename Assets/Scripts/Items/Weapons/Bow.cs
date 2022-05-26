using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon
{
    [SerializeField] private float _attackTime;
    [SerializeField] private GameObject _arrowPrefab;
    [SerializeField] private CircleSelector _areaSelector;

    public override Cell[] GetAvailableCells()
    {
        if (Owner is null)
            return new Cell[0];

        return _areaSelector.GetAvailableCells(Owner.GamePosition);
    }

    public override IEnumerator AttackRoutine(Cell cell)
    {
        var targetPosition = Store.Instance.GameField.GameToWorldPosition(cell.GamePosition);
        var distanceVector = targetPosition - (Vector2)Owner.transform.position;
        var arrow = Instantiate(_arrowPrefab, Owner.transform.position, Quaternion.FromToRotation(Vector2.one, distanceVector));
        var totalTime = _attackTime * distanceVector.magnitude;
        float startTime = Time.realtimeSinceStartup;
        float t = 0f;

        while (t < 1f)
        {
            t = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / totalTime);
            arrow.transform.position = Vector2.Lerp(arrow.transform.position, targetPosition, t);
            yield return null;
        }
        Destroy(arrow.gameObject);
    }
}