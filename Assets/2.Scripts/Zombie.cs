using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected float jumpPower;
    protected Rigidbody2D rigid;

    protected bool isPush = false;
    protected bool isJumpZombie = false;
    protected bool isJump = false;

    protected Zombie pushZombie;

    private void Awake()
    {
        Init();
        GameManager.Instance.PushLast(this);
    }
    protected virtual void Init()
    {

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
        Move();

        if (isJump)
        {
            Jump();
        }
        if (isPush && GameManager.Instance.isPushActive)
        {
            Push();
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
            GameManager.Instance.AddWaiting(this);
            StartCoroutine(PushAction());
        }
        if (collision.gameObject.CompareTag("Ground") || collision.contacts[0].normal.y > 0.5f)
        {
            isJump = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isJump)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Zombie") && GameManager.Instance.IsPossibleJump(this))
        {
            isJump = true;
            isJumpZombie = true;
        }
    }

    protected virtual void Move()
    {

    }
    protected virtual void Jump()
    {

    }
    protected virtual void Push()
    {

    }
    IEnumerator PushAction()
    {
        pushZombie = GameManager.Instance.FindCloseZombie();
        GameManager.Instance.isPushActive = true;
        isPush = true;

        yield return new WaitForSeconds(0.5f);

        isPush = false;

        rigid.mass = 1;
        GameManager.Instance.isPushActive = false;
    }
}
