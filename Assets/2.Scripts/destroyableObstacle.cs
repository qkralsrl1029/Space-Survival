using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyableObstacle : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Laser"))
        {
            Destroy(this.gameObject);
        }
    }
}
