using System.Collections;
using UnityEngine;

public class ZombieTest : MonoBehaviour
{
    private Rigidbody2D rigid;

    public float moveSpeed = 1.5f;
    private bool shouldJump = false;
    private bool isGround = true; // 처음에는 땅에 있음
    private bool isJump;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // 이동        
        rigid.velocity = new Vector2(-moveSpeed, rigid.velocity.y);

        // 점프 실행
        if (shouldJump)
        {
            Debug.Log("점프 실행!");

            rigid.velocity = new Vector2(rigid.velocity.x, 0f); // 기존 속도 초기화 (더 자연스러움)
            rigid.AddForce(Vector2.up * 15f, ForceMode2D.Impulse);

            shouldJump = false; // 점프 후 초기화
        }
        if (!isGround && rigid.velocity.y <= 0)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, -2f); // 아래로 강제로 떨어지게 설정
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
            isGround = true; // 땅에 닿았으므로 착지 상태
            shouldJump = false; // 점프 가능
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("무언가 계속 접촉 중");
        if (isJump)
            return;

        if (collision.gameObject.CompareTag("Zombie") && (GetInstanceID() == GameManager.Instance.zombiesList.Last.Value.GetInstanceID()))
        {
            //StartCoroutine(JumpAction());
            isJump = true;

            shouldJump = true;
            Debug.Log(GameManager.Instance.zombiesList.Last.Value.name);
            Debug.Log(name);
            Debug.Log($"점프한다.!!");
        }

    }


    IEnumerator JumpAction()
    {
        isJump = true;
        
        shouldJump = true;

        yield return new WaitForSeconds(0.5f);
    }
}
