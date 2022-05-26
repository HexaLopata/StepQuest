using UnityEngine;
using System;

[Serializable]
public class Drop 
{
    [SerializeField] private Item _item;
    [SerializeField] private int _dropChance;

    public Item Item => _item;
    public int DropChance => _dropChance;
}