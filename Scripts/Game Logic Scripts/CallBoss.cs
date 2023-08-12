using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallBoss : MonoBehaviour
{
    public GameLogic gameLogic;
    public BossType bossType;

    private bool spawned = false;

    private void OnTriggerEnter(Collider collison)
    {
        if(collison.CompareTag("Player") && !spawned)
        {
            switch (bossType)
            {
                case BossType.Pig:
                    gameLogic.spawnPigboss();
                    break;
                case BossType.Priest:
                    gameLogic.spawnPriestboss();
                    break;
                case BossType.Paladin:
                    gameLogic.spawnPaladinboss();
                    break;
            }
            spawned = true;
        }
        
    }
}

public enum BossType { Pig,Priest,Paladin}