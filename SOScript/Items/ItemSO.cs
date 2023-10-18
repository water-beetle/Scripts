using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class ItemSO : ScriptableObject
{
    public string itemName;
    [TextArea] public string itemDescription;
    public Sprite itemImage;
     
    public bool isStackable;
    public int maxStackSize = 1;

    public bool isThrowable = false;
    public ItemType itemType;

    public abstract void UseItem(Transform transform);

    public int ID => GetInstanceID();
}

