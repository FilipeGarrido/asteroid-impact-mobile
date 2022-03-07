using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidFX : MonoBehaviour
{  
    public ParticleSystem _particleSystem;
    public AudioSource destroyAudio;
    public float timeToDestroy = 2.0f;
    
    void Start()
    {
        destroyAudio.Play();
        _particleSystem.Play(false);
        Destroy(gameObject,timeToDestroy);
    }
}
