using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected float jumpPower;
    protected Rigidbody2D rigid;

    // ������ ������ �о �� �ְԲ� �ϴ� �÷��� ����
    protected bool isPush = false;
    // ������ �������� Ȯ��
    protected bool isJumpZombie = false;
    protected bool isJump = false;
    protected bool isGround = false;

    // ���� �а� �ִ� ����
    protected Zombie currPushZombie;

    private void Awake()
    {
        Init();
        GameManager.Instance.PushLast(this);
    }
    protected virtual void Init()
    {

    }    
    private void FixedUpdate()
    {
        Move();

        if (isJump)
        {
            Jump();
        }
        if (GameManager.Instance.isPushActive)
        {
            Push();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //if (collision.gameObject.CompareTag("Zombie"))
        //{
        //    GameManager.Instance.DeleteWaiting(this);
        //}
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.CompareTag("Zombie"))
        //{
        //    GameManager.Instance.AddWaiting(this);
        //}
        if (collision.gameObject.CompareTag("Hero") && isJumpZombie)
        {            
            StartCoroutine(PushAction());            
        }
        if (collision.contacts[0].normal.y > 0.5f)
        {
            isJump = false;
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJump = false;
            isGround = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isJump || isPush)
        {
            return;
        }
        if (collision.gameObject.CompareTag("Ground"))
        {            
            isGround = true;
        }
        RaycastHit2D hitDown = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.down, 0.15f);        
        if (collision.gameObject.CompareTag("Zombie") && isGround)
        {
            GameManager.Instance.AddWaiting(this);
        }     
        if (collision.gameObject.CompareTag("Zombie") && (hitDown.collider != null))
        {
            GameManager.Instance.AddWaiting(this);
        }
        if (collision.gameObject.CompareTag("Zombie"))
        {            
            if (GameManager.Instance.IsPossibleJump(this))
            {
                GameManager.Instance.DeleteWaiting(this);
                isJump = true;
                isJumpZombie = true;
            }
        }
    }

    protected virtual void Move()
    {
        rigid.velocity = new Vector2(-speed, rigid.velocity.y);
    }
    protected virtual void Jump()
    {
        Debug.Log("���� ����!");
        isGround = false;
        rigid.velocity = new Vector2(rigid.velocity.x, 0f); // ���� �ӵ� �ʱ�ȭ
        rigid.AddForce(Vector2.up * jumpPower / 2f, ForceMode2D.Impulse);
        isJump = false; // ���� �� �ʱ�ȭ      
    }
    protected virtual void Push()
    {
        if (currPushZombie == null)
        {
            return;
        }

        Debug.Log($"��ٸ��� ���� ��? : {GameManager.Instance.GetWaitCount()}");

        float waitCount = GameManager.Instance.GetWaitCount() - 1;
        float forceMultiple = 1f + Mathf.Log(1f + waitCount) * 0.25f;

        Rigidbody2D rb = currPushZombie.GetComponent<Rigidbody2D>();

        // ���� �ӵ� ����: �ʹ� ������ �ִ� �ӵ��� ����
        float maxSpeed = 5f; // ���ϴ� �ִ� �ӵ�
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        // AddForce ��� velocity�� ���� ����
        rb.velocity = new Vector2(forceMultiple, rb.velocity.y);
    }

    IEnumerator PushAction()
    {
        GameManager.Instance.DeleteWaiting(this);
        currPushZombie = GameManager.Instance.FindCloseZombie();

        GameManager.Instance.isPushActive = true;
        isPush = true;

        yield return new WaitUntil(() => (false == currPushZombie.Equals(GameManager.Instance.FindCloseZombie())) && GameManager.Instance.FindCloseZombie().isGround);

        currPushZombie = GameManager.Instance.FindCloseZombie();
        GameManager.Instance.FindCloseZombie().GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        isPush = false;                
        GameManager.Instance.isPushActive = false;
    }
}
