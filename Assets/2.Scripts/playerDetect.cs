using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerDetect : MonoBehaviour

{
    monsterController theMonster;
    PlayerController thePlayer;
    Vector3 playerPos = new Vector3();

    private void Start()
    {
        thePlayer = FindObjectOfType<PlayerController>();
    }
    public void setMonster(monsterController monster)
    {
        theMonster = monster;
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerPos = other.transform.position;
            theMonster.setGoalPos(playerPos);
            theMonster.tracePlayer();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            theMonster.stopTrace();
        }
    }


}
