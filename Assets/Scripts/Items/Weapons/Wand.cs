using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wand : Weapon
{
    [SerializeField] private float _attackTime;
    [SerializeField] private GameObject _magicBallPrefab;
    [SerializeField] private ManhattanDistanceSelector _areaSelector;

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
        var magicBall = Instantiate(_magicBallPrefab, Owner.transform.position, Quaternion.FromToRotation(Vector2.right, distanceVector));
        var totalTime = _attackTime * distanceVector.magnitude;
        float startTime = Time.realtimeSinceStartup;
        float t = 0f;

        while (t < 1f)
        {
            t = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / totalTime);
            magicBall.transform.position = Vector2.Lerp(magicBall.transform.position, targetPosition, t);
            yield return null;
        }
        Destroy(magicBall.gameObject);
    }
}