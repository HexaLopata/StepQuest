using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Слегка измененный MonoBehaviour
/// </summary>
public class GameComponent : MonoBehaviour
{
    private void Awake() 
    {
        OnAwake();
    }

    private void Start()
    {
        UpdateChildrenComponents();
        OnStart();
    }

    private void Update()
    {
        OnUpdate();
    }

    private void FixedUpdate()
    {
        OnFixedUpdate();   
    }

    private void OnDestroy() 
    {
        OnRemove();
    }

    public GameComponent ChangeParent(GameComponent component, Transform newParent)
    {
        var name = component.name;
        var newComponent = Instantiate(component, newParent);
        newComponent.name = name;
        Destroy(component.gameObject);
        return newComponent;
    }

    protected virtual void OnAwake() { }
    protected virtual void OnStart() { }
    protected virtual void OnUpdate() { }
    protected virtual void OnFixedUpdate() { }
    protected virtual void OnRemove() { }
    public virtual void UpdateChildrenComponents() { }

    private void OnTransformChildrenChanged() 
    {
        UpdateChildrenComponents();
    }
}
