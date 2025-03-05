using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public float speed;

    public bool isZombieTouch = false;

    private Rigidbody2D rigid;

    private float jumpPower = 4.5f;

    private float pushPower = 1f;
    private bool isPush = false;
    private bool isJumpZombie = false;
    private bool isJump = false;
    private bool isTower = false;
    private bool shouldJump;
    private bool isGround;

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

        //int orderPlus = 0;
        //switch (zombieOrder)
        //{
        //    case ZombieOrder.First:
        //        orderPlus = 2;
        //        break;
        //    case ZombieOrder.Second:
        //        orderPlus = 1;
        //        break;
        //    case ZombieOrder.Third:
        //        orderPlus = 0;
        //        break;
        //    default:
        //        break;
        //}
        //foreach (SpriteRenderer sprite in sprites)
        //{
        //    sprite.sortingOrder += orderPlus;
        //}

        GameManager.Instance.PushLast(this);        
    }

    private void Update()
    {

        //transform.Translate(Vector3.left * Time.deltaTime * speed);
    }
    private void FixedUpdate()
    {
        // �̵�        
        rigid.velocity = new Vector2(-speed, rigid.velocity.y);

        // ���� ����
        if (shouldJump)
        {
            Debug.Log("���� ����!");

            rigid.velocity = new Vector2(rigid.velocity.x, 0f); // ���� �ӵ� �ʱ�ȭ (�� �ڿ�������)
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);

            shouldJump = false; // ���� �� �ʱ�ȭ
        }
        if (isPush)
        {
            rigid.mass += 0.5f;
            //GameManager.Instance.zombiesList.First.Value.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            GameManager.Instance.zombiesList.First.Value.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 130.5f * (GameManager.Instance.zombiesList.Count - 1));

            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hero") && isJump)
        {            
            Debug.Log("Ÿ�� �ε�����.");
            StartCoroutine(PushAction());
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJump = false;
            isGround = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("���� ��� ���� ��");
        if (isJump)        
            return;
        
        if (collision.gameObject.CompareTag("Zombie") && (GetInstanceID() == GameManager.Instance.zombiesList.Last.Value.GetInstanceID()))
        {
            StartCoroutine(JumpAction());
            Debug.Log(GameManager.Instance.zombiesList.Last.Value.name);
            Debug.Log(name);
            Debug.Log($"�����Ѵ�.!!");
        }

    }

    IEnumerator PushAction()
    {
        isPush = true;
        pushPower = 1f;

        yield return new WaitUntil(() => isGround);

        isPush = false;
        
        rigid.mass = 1;
        GameManager.Instance.Delete();
        GameManager.Instance.Push(this);

    }

    IEnumerator JumpAction()
    {
        isJump = true;
        isJumpZombie = true;
        shouldJump = true;
        isGround = false;

        yield return new WaitForSeconds(0.5f);
    }
}
