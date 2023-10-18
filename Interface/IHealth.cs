using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    public int health { get; }

    public void Death();
    public void TakeDamage(GameObject dealer, int damage);

}
