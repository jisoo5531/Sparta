using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected float jumpPower;
    protected Rigidbody2D rigid;

    // 점프한 좀비만이 밀어낼 수 있게끔 하는 플래그 변수
    protected bool isPush = false;
    // 점프한 좀비인지 확인
    protected bool isJumpZombie = false;
    protected bool isJump = false;
    protected bool isGround = false;

    // 현재 밀고 있는 좀비
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
        Debug.Log("점프 실행!");
        isGround = false;
        rigid.velocity = new Vector2(rigid.velocity.x, 0f); // 기존 속도 초기화
        rigid.AddForce(Vector2.up * jumpPower / 2f, ForceMode2D.Impulse);
        isJump = false; // 점프 후 초기화      
    }
    protected virtual void Push()
    {
        if (currPushZombie == null)
        {
            return;
        }

        Debug.Log($"기다리는 좀비 수? : {GameManager.Instance.GetWaitCount()}");

        float waitCount = GameManager.Instance.GetWaitCount() - 1;
        float forceMultiple = 1f + Mathf.Log(1f + waitCount) * 0.25f;

        Rigidbody2D rb = currPushZombie.GetComponent<Rigidbody2D>();

        // 기존 속도 제한: 너무 빠르면 최대 속도로 제한
        float maxSpeed = 5f; // 원하는 최대 속도
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        // AddForce 대신 velocity를 직접 변경
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
