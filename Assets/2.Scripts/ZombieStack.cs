using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieStack : MonoBehaviour
{
    public Zombie[] zombie;

    private Stack<Zombie> zombies;

    public void PushStack(Zombie zombie)
    {
        zombies.Push(zombie);
    }
    
}
