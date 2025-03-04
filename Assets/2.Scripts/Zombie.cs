using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public float speed;

    public bool isZombieTouch = false;

    private bool isFirstCreate = true;
    private bool isJump = false;

    public enum ZombieOrder
    {
        First,
        Second,
        Third
    }
    public ZombieOrder zombieOrder;

    private void Awake()
    {
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        int orderPlus = 0;
        switch (zombieOrder)
        {
            case ZombieOrder.First:
                orderPlus = 2;
                break;
            case ZombieOrder.Second:
                orderPlus = 1;
                break;
            case ZombieOrder.Third:
                orderPlus = 0;
                break;
            default:
                break;
        }
        foreach (SpriteRenderer sprite in sprites)
        {
            sprite.sortingOrder += orderPlus;
        }

        GameManager.Instance.PushLast(this);
        isFirstCreate = true;
    }

    private void Update()
    {
        transform.Translate(Vector3.left * Time.deltaTime * speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hero") && false == isFirstCreate)
        {
            Debug.Log("Å¸¿ö ºÎµúÇû´Ù.");
            StartCoroutine(PushAction());
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJump = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        isZombieTouch = true;

        if (isJump)
        {
            return;
        }
        if (collision.gameObject.CompareTag("Zombie") && (GetInstanceID() == GameManager.Instance.zombiesList.Last.Value.GetInstanceID()))
        {
            StartCoroutine(JumpAction());
            Debug.Log(GameManager.Instance.zombiesList.Last.Value.name);
            Debug.Log(name);
            Debug.Log($"Á¡ÇÁÇÑ´Ù.!!");
        }
        //if (collision.gameObject.CompareTag("Hero") && false == isFirst)
        //{
        //    isJump = false;
        //}
        
    }

    IEnumerator PushAction()
    {
        GetComponent<Rigidbody2D>().mass *= 5f;

        GameManager.Instance.zombiesList.First.Value.GetComponent<Rigidbody2D>().AddForce(Vector3.right * 5f, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.1f);

        GetComponent<Rigidbody2D>().mass /= 5f;

        GameManager.Instance.Push(this);
    }

    IEnumerator JumpAction()
    {
        isJump = true;
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();

        rigid.AddForce(Vector3.up * 5f, ForceMode2D.Impulse);        

        yield return new WaitForSeconds(0.5f);

        isFirstCreate = false;
        GameManager.Instance.Delete();
    }
}
