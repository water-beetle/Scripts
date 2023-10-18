using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractEffect : MonoBehaviour
{
    protected int nums;
    
    public virtual void Init(int _nums)
    {
        this.nums = _nums;

        if(nums <= 0)
        {
            Destroy(gameObject);
        }
    }

}
