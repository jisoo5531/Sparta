using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public float speed;

    public bool isZombieTouch = false;

    private Rigidbody2D rigid;

    private float pushPower = 1f;
    private bool isPush = false;
    private bool isJumpZombie = false;
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
        rigid = GetComponent<Rigidbody2D>();
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
        isJumpZombie = true;
    }

    private void Update()
    {

        transform.Translate(Vector3.left * Time.deltaTime * speed);
    }
    private void FixedUpdate()
    {        
        if (false == isPush || (GetInstanceID() != GameManager.Instance.zombiesList.Last.Value.GetInstanceID()))
            return;
        Debug.Log("밀어내기 실행");
        rigid.mass *= 0.5f;
        GameManager.Instance.zombiesList.First.Value.GetComponent<Rigidbody2D>().AddForce(Vector3.right * 10f);        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hero") && isJump)
        {
            Debug.Log("타워 부딪혔다.");
            StartCoroutine(PushAction());
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJump = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {        
        if (isJump)        
            return;
        
        if (collision.gameObject.CompareTag("Zombie") && (GetInstanceID() == GameManager.Instance.zombiesList.Last.Value.GetInstanceID()))
        {
            StartCoroutine(JumpAction());
            Debug.Log(GameManager.Instance.zombiesList.Last.Value.name);
            Debug.Log(name);
            Debug.Log($"점프한다.!!");
        }
        //if (collision.gameObject.CompareTag("Hero") && false == isFirst)
        //{
        //    isJump = false;
        //}
        
    }

    IEnumerator PushAction()
    {
        isPush = true;
        pushPower = 1f;

        yield return new WaitUntil(() => isJump == false);

        isPush = false;
        rigid.mass = 1;
        GameManager.Instance.zombiesList.First.Value.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GameManager.Instance.Delete();
        GameManager.Instance.Push(this);
    }

    IEnumerator JumpAction()
    {
        isJump = true;
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();

        rigid.AddForce(Vector3.up * 5f, ForceMode2D.Impulse);        

        yield return new WaitForSeconds(0.5f);
    }
}
