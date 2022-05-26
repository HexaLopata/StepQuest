using UnityEngine;

public class Item : GameComponent
{
    [SerializeField] private Sprite _image;

    public Sprite Image => _image;
    
}