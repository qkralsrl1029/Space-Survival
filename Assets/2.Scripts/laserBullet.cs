using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserBullet : MonoBehaviour
{
    [SerializeField] float force = 1000;
    Rigidbody rigid;

    [SerializeField] int powAtt = 2;
    int baseDamage;

    public int finalDamage { get { return baseDamage + powAtt; } }

    // Start is called before the first frame update
    void Start()
    {
        //생성 후 일정 힘으로 물리힘
        rigid = GetComponent<Rigidbody>();
        rigid.AddForce(FindObjectOfType<PlayerController>().transform.forward * force);
    }

   public void intitData(int damage)
    {
        baseDamage = damage;
    }
}
