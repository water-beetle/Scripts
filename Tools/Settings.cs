using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    public struct AnimatorParams
    {
        public static int isMove = Animator.StringToHash("isMove");
        public static int isAttack = Animator.StringToHash("isAttack");
        public static int isOpen = Animator.StringToHash("isOpen");
    }

    public struct Tags
    {
        public static string Player = "Player";
        public static string enemyBodyCollider = "EnemyBody";
    }

    public struct Astar
    {
        public static int PursueDistance = 20;

    }

    #region PotionEffect
    public static float fireSpreadTime = 2f;
    public static float fireExtinguishTime = 15f;
    #endregion

    public struct Agent
    {
        public static float acceleration = 200;
        public static float deacceleration = 200;
        public static float rushDuration = 2f;
        public static float rushPower = 5f;
    }


}
