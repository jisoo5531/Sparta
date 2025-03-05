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
        // �̵�        
        rigid.velocity = new Vector2(-speed, rigid.velocity.y);
    }
    protected override void Jump()
    {
        Debug.Log("���� ����!");

        rigid.velocity = new Vector2(rigid.velocity.x, 0f); // ���� �ӵ� �ʱ�ȭ
        rigid.AddForce(Vector2.up * jumpPower / 2f, ForceMode2D.Impulse);
        isJump = false; // ���� �� �ʱ�ȭ      
    }
    protected override void Push()
    {
        pushZombie.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 120f * Mathf.Max(1, GameManager.Instance.GetWaitCount()));
    }
}
