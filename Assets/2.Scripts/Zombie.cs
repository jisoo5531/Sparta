using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public float speed;

    public bool isZombieTouch = false;

    private Rigidbody2D rigid;

    private float jumpPower = 4.5f;
    
    private bool isPush = false;
    private bool isJumpZombie = false;
    private bool isJump = false;
    private bool isTower = false;    
    private bool isGround;        
    private Vector2 lastVelocity;

    private Zombie pushZombie;

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

        GameManager.Instance.PushLast(this);
    }

    private void Update()
    {
        Debug.Log($"기다리는 좀비 수? : {GameManager.Instance.GetWaitCount()}");
        Debug.DrawRay(new Vector2(transform.position.x + 0.3f, transform.position.y + 0.5f), Vector2.right * 2f, Color.red);
        Debug.DrawRay(new Vector2(transform.position.x + 0.3f, transform.position.y + 0.5f), Vector2.up * 2f, Color.blue);
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + 1.3f), Vector2.left * 0.5f, Color.yellow);
    }

    private void FixedUpdate()
    {
        // 이동        
        rigid.velocity = new Vector2(-speed, rigid.velocity.y);        

        // 점프 실행
        if (isJump)
        {
            Debug.Log("점프 실행!");

            rigid.velocity = new Vector2(rigid.velocity.x, 0f); // 기존 속도 초기화 (더 자연스러움)
            rigid.AddForce(Vector2.up * jumpPower / 2f, ForceMode2D.Impulse);
            //StartCoroutine(SmoothJump());
            isJump = false; // 점프 후 초기화            
        }
        if (isPush && GameManager.Instance.isPushActive)
        {
            
            Debug.Log($"기다리는 좀비 수 : {GameManager.Instance.zombieWaitingList.Count}");
            pushZombie.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 110f * Mathf.Max(1, GameManager.Instance.GetWaitCount()));
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            GameManager.Instance.DeleteWaiting(this);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            GameManager.Instance.AddWaiting(this);            
        }
        if (collision.gameObject.CompareTag("Hero") && isJumpZombie)
        {
            Debug.Log("타워 부딪혔다.");
            GameManager.Instance.AddWaiting(this);
            StartCoroutine(PushAction());
        }        
        if (collision.gameObject.CompareTag("Ground") || collision.contacts[0].normal.y > 0.5f)
        {
            isJump = false;
            isGround = true;
            
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
           
        Debug.Log("무언가 계속 접촉 중");
        if (isJump)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Zombie") && GameManager.Instance.IsPossibleJump(this))
        {
            Jump();

            
            Debug.Log($"점프한다.!!");
        }

    }
    IEnumerator PushAction()
    {
        pushZombie = GameManager.Instance.FindCloseZombie();
        GameManager.Instance.isPushActive = true;
        isPush = true;        

        yield return new WaitForSeconds(0.7f);

        isPush = false;
        
        rigid.mass = 1;
        GameManager.Instance.isPushActive = false;
        //GameManager.Instance.Delete(this);
        //GameManager.Instance.PushFirst(this);        
    }

    private void Jump()
    {
        isJump = true;
        isJumpZombie = true;        
        isGround = false;
    }  
}
