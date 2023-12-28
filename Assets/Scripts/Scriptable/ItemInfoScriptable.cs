using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Kimmy/Item Info")]
public class ItemInfoScriptable : ScriptableObject
{
    public int itemId;
    public string itemName;
    public int itemCount;
    public string itemDescription;
    public bool stackable;
    public Sprite GetItemSprite() => Resources.Load<Sprite>($"Item/{this.itemId}") ?? null;
}
