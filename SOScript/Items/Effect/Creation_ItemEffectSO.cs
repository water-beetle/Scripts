using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Item/Effect/Create", fileName = "ItemEffect_Create_")]
public class Creation_ItemEffectSO : ItemEffectSO
{
    [SerializeField] private AbstractEffect creation;
    [SerializeField] private int num;


    public override void UseEffect(Transform transform)
    {
        AbstractEffect created = Instantiate(creation);
        created.transform.position = transform.position;
        created.Init(num);

    }
}
