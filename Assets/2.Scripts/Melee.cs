using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Zombie
{            

    //public enum ZombieOrder
    //{
    //    First,
    //    Second,
    //    Third
    //}
    //public ZombieOrder zombieOrder;

    //private void Awake()
    //{
        
    //    SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
    //}
    protected override void Init()
    {
        base.Init();
        rigid = GetComponent<Rigidbody2D>();
    }

    protected override void Move()
    {
        // ¿Ãµø        
        base.Move();
    }
    protected override void Jump()
    {
        base.Jump();
    }
    protected override void Push()
    {
        base.Push();
    }
}
