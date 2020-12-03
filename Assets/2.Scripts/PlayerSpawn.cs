using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject prefab = Resources.Load("prefab/Unit/Player") as GameObject;
        GameObject go = Instantiate(prefab, transform.position, transform.rotation);
    }

  
}
