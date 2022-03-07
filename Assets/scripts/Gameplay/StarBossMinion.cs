using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBossMinion : MonoBehaviour
{
    private Rigidbody2D BossMinionRigidbody;//Variável para armazenar o rigidbody do player
    public float Speed = 50.0f; //Velocidade
    public float RotationSpeed = 5.0f;
    public float MaxLifeTime;
    // Start is called before the first frame update
    void Awake()
    {
        BossMinionRigidbody = GetComponent<Rigidbody2D>();
        BossMinionRigidbody.AddTorque(this.RotationSpeed);
        Destroy(this.gameObject,2.0f);
    }

    // Update is called once per frame
    public void ThrowMinions(Vector3 _direction)
    {
        BossMinionRigidbody.AddForce(_direction*this.Speed);
        transform.up = _direction;
    }
}
