using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D _rigidbody;//Variável para armazenar o rigidbody do projetil
    public float bulletLifeTime = 2.0f;//Tempo de "vida" do projetil caso não atinja nenhum asteroid
    public float speed = 500.0f;//Velocidade do projetil
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();//Armazena o rigidbody do projetil
        Destroy(this.gameObject,bulletLifeTime);//Assim que o projetil é lançado, ele terá um tempo de vida útil caso não atinja um asteroid

    }

    public void Project (Vector2 direction)
    {
        _rigidbody.AddForce(direction * this.speed);//Recebe do player a direção e acrescenta a velocidade
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(this.gameObject);//Destroy ao colidir
    }
}
