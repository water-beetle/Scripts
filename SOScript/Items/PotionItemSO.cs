using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Item/Potion", fileName = "Potion_")]
public class PotionItemSO : ItemSO
{
    [SerializeField] private List<ItemEffectSO> itemEffectsList;

    public override void UseItem(Transform transform)
    {
        
        for(int i=0; i < itemEffectsList.Count; i++)
        {
            itemEffectsList[i].UseEffect(transform);
        }
    }
}
