using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class launcherController : MonoBehaviour //총알 생성
{
    [SerializeField] GameObject bullet;
    [SerializeField] Transform originPos;
     PlayerController player;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    string itemName="gun name";
    float damageRate=1.0f;

    public string weaponName { get { return itemName; } }
    public float weaponRate { get { return damageRate; } }

    public void initInfo(string name,float rate)
    {
        itemName = name;
        damageRate = rate;
    }

    public void bulletCreate()
    {
        var go = Instantiate(bullet,originPos.position,Quaternion.identity);
        laserBullet lb = go.GetComponent<laserBullet>();
        lb.intitData(player.finishdamage);
        Destroy(go, 3f);
    }

    public void InitData(PlayerController owner)
    {
        player = owner;

        initInfo("Laser Gun", 1.5f);
    }
}
