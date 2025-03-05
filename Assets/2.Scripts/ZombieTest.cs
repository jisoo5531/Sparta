using System.Collections;
using UnityEngine;

public class ZombieTest : MonoBehaviour
{
    private Rigidbody2D rigid;

    public float moveSpeed = 1.5f;
    private bool shouldJump = false;
    private bool isGround = true; // ó������ ���� ����
    private bool isJump;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // �̵�        
        rigid.velocity = new Vector2(-moveSpeed, rigid.velocity.y);

        // ���� ����
        if (shouldJump)
        {
            Debug.Log("���� ����!");

            rigid.velocity = new Vector2(rigid.velocity.x, 0f); // ���� �ӵ� �ʱ�ȭ (�� �ڿ�������)
            rigid.AddForce(Vector2.up * 15f, ForceMode2D.Impulse);

            shouldJump = false; // ���� �� �ʱ�ȭ
        }
        if (!isGround && rigid.velocity.y <= 0)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, -2f); // �Ʒ��� ������ �������� ����
        }
        //if (isPush)
        //{
        //    //GameManager.Instance.zombiesList.First.Value.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        //    GameManager.Instance.zombiesList.First.Value.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 100.5f, ForceMode2D.Impulse);

        //    //GameManager.Instance.Delete();
        //    //GameManager.Instance.Push(this);

        //    isPush = false;
        //}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true; // ���� ������Ƿ� ���� ����
            shouldJump = false; // ���� ����
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("���� ��� ���� ��");
        if (isJump)
            return;

        if (collision.gameObject.CompareTag("Zombie") && (GetInstanceID() == GameManager.Instance.zombiesList.Last.Value.GetInstanceID()))
        {
            //StartCoroutine(JumpAction());
            isJump = true;

            shouldJump = true;
            Debug.Log(GameManager.Instance.zombiesList.Last.Value.name);
            Debug.Log(name);
            Debug.Log($"�����Ѵ�.!!");
        }

    }


    IEnumerator JumpAction()
    {
        isJump = true;
        
        shouldJump = true;

        yield return new WaitForSeconds(0.5f);
    }
}
