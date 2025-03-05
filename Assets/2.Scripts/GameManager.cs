using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject zombiePrefab;
    public Transform firstZombieTrans;

    public List<Zombie> zombies;    // test
    public LinkedList<Zombie> zombiesList = new LinkedList<Zombie>();
    public LinkedList<Zombie> zombieWaitingList = new LinkedList<Zombie>();

    public bool isPushActive = false;
        
    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(zombiePrefab, new Vector3(6, -3f, 0), Quaternion.identity);                        
        }
    }

    public Zombie FindCloseZombie()
    {
        Zombie closestZombie = null;
        float closestDistance = Mathf.Infinity;  // 초기값은 무한대로 설정 (가장 작은 거리를 찾기 위함)

        // OverlapCircle로 범위 내의 모든 좀비를 검색
        Collider2D[] colliders = Physics2D.OverlapCircleAll(firstZombieTrans.position, 5f, LayerMask.GetMask("Zombie")); // 범위(반지름: 5, "Zombie" 레이어의 콜라이더만 검색)

        foreach (Collider2D collider in colliders)
        {
            // Zombie 객체만 필터링
            Zombie zombie = collider.GetComponent<Zombie>();
            if (zombie != null)
            {
                // 현재 좀비와 첫 번째 좀비의 위치 간의 거리 계산
                float distance = Vector2.Distance(firstZombieTrans.position, zombie.transform.position);
                if (distance < closestDistance)  // 가장 가까운 좀비를 찾으면
                {
                    closestDistance = distance;
                    closestZombie = zombie;
                }
            }
        }

        return closestZombie;  // 가장 가까운 좀비 객체 반환
    }
    public int GetWaitCount()
    {
        return zombieWaitingList.Count;
    }
    public void DeleteWaiting(Zombie zombie)
    {
        if (zombieWaitingList.Find(zombie) == null)        
            return;
        zombieWaitingList.Remove(zombie);
    }
    public void AddWaiting(Zombie zombie)
    {
        if (zombieWaitingList.Find(zombie) != null)
            return;
        zombieWaitingList.AddLast(zombie);
    }

    public void Delete(Zombie zombie)
    {
        if (zombiesList.Find(zombie) == null)        
            return;
        
        zombiesList.Remove(zombie);                
    }
    public void PushFirst(Zombie zombie)
    {
        if (zombiesList.Find(zombie) != null)
            return;

        zombiesList.AddFirst(zombie);
    }
    public void PushLast(Zombie zombie)
    {
        if (zombiesList.Find(zombie) != null)
            return;

        zombiesList.AddLast(zombie);
    }

    public bool IsPossibleJump(Zombie zombie)
    {
        if (zombiesList.Count == 1)
            return false;

        
        float rayDistance = 2f;
        //Debug.DrawRay(new Vector2(zombie.transform.position.x, zombie.transform.position.y + 0.1f), Vector2.right * rayDistance, Color.red);
        //Debug.DrawRay(new Vector2(zombie.transform.position.x + 0.3f, zombie.transform.position.y + 0.5f), Vector2.up * rayDistance, Color.red);
        RaycastHit2D hitRight = Physics2D.Raycast(new Vector2(zombie.transform.position.x + 0.3f, zombie.transform.position.y + 0.5f), Vector2.right, 2f);
        RaycastHit2D hitUp = Physics2D.Raycast(new Vector2(zombie.transform.position.x + 0.3f, zombie.transform.position.y + 0.5f), Vector2.up, 2);
        RaycastHit2D hitLeft = Physics2D.Raycast(new Vector2(zombie.transform.position.x, zombie.transform.position.y + 1.3f), Vector2.left, 0.5f);      
        
        if (hitRight.collider == null && hitUp.collider == null && hitLeft.collider == null)
        {
            return true;
        }
        return false;
    }
}
