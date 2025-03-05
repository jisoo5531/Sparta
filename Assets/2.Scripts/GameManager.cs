using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject zombiePrefab;

    public List<Zombie> zombies;    // test
    public LinkedList<Zombie> zombiesList = new LinkedList<Zombie>();
        
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

    public void Action(Zombie zombie)
    {
    }
    public void Delete()
    {
        zombiesList.RemoveLast();
    }
    public void Push(Zombie zombie)
    {
        zombiesList.AddFirst(zombie);
    }
    public void PushLast(Zombie zombie)
    {
        zombiesList.AddLast(zombie);
    }
}
