using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHoles : MonoBehaviour
{
    public GameObject bulletHoleObj;
    public GameObject[] bulletHoles;
    void Start()
    {
        bulletHoles = new GameObject[50];
        for (int i = 0; i < bulletHoles.Length; i++)
        {
            bulletHoles[i] = Instantiate(bulletHoleObj, new Vector3(0, -50, 0), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
