using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Rifle Sounds")]
    public AudioSource rifleShoot;
    public AudioSource rifleReload;

    [Header("Pistol sounds")]
    public AudioSource pistolShoot;
    public AudioSource pistolReload;

    [Header("Bullet sounds")]
    public AudioSource bulletFizzle;
    public AudioSource dropCasing;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
