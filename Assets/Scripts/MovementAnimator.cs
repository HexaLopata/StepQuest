using UnityEngine;

[RequireComponent(typeof(Entity))]
[RequireComponent(typeof(SpriteRenderer))]
public class MovementAnimator : MonoBehaviour
{
    private Entity _entity;
    private SpriteRenderer _sp;
    [SerializeField] private Sprite _down;
    [SerializeField] private Sprite _up;
    [SerializeField] private Sprite _left;
    [SerializeField] private Sprite _right;

    void Start()
    {
        _entity = GetComponent<Entity>();
        _entity.onStartMoving.AddListener(Animate);
        _sp = GetComponent<SpriteRenderer>();
    }

    void Animate(Vector2 direction)
    {
        if (direction.x > 0)
            _sp.sprite = _right;
        else if (direction.x < 0)
            _sp.sprite = _left;
        else if (direction.y > 0)
            _sp.sprite = _up;
        else
            _sp.sprite = _down;
    }
}