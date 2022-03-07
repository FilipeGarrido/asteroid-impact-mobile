using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Rigidbody2D EnemyRigidbody;//Variável para armazenar o rigidbody do player
    public AsteroidFX destroyFXPrefab;
    public Bullet BulletPrefab;//Variavel para criar o peojetil
    public ExtraLife ExtraLifePrefab;
    [SerializeField] private AudioSource _enemyShootSoudFX;
    public float Speed = 50.0f; //Velocidade
    public float maxLifeTime = 20.0f;
    private Vector3 _enemyDirection;
    public static System.Action EnemyDeathEvent = null;
    public int ShootTime;
    public int ShootTimeRate;


    void Awake()
    {
        EnemyRigidbody = GetComponent<Rigidbody2D>();//Armazena o rigidbody do player
    }

    void Start(){
        Destroy (this.gameObject, this.maxLifeTime);
        InvokeRepeating(nameof(Shoot),ShootTime,ShootTimeRate);

    }

     public void SetTrajectory(Vector2 direction)
    {   
        EnemyRigidbody.AddForce(direction * this.Speed);
        transform.up = direction;
        Debug.Log((direction * this.Speed).ToString());
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        //Se a colisão for com um projétil, toca o audio do asteróide destruindo
        if(other.gameObject.tag == "Bullet"){
            float lifeDropChance = Random.Range(0.0f , 100.0f);
            if(lifeDropChance > 90.0f){
                ExtraLife newLife = Instantiate(ExtraLifePrefab,this.transform.position,Quaternion.identity);
            }

            Instantiate(destroyFXPrefab,this.EnemyRigidbody.position,Quaternion.identity);
            if(EnemyDeathEvent != null){
                EnemyDeathEvent();
            }
            Destroy(this.gameObject);
        }
        else if(other.gameObject.tag == "Player"){
            Instantiate(destroyFXPrefab,this.EnemyRigidbody.position,Quaternion.identity);
            Destroy(this.gameObject);
        }
        else if(other.gameObject.tag == "BulletPowerUp"){
            float lifeDropChance = Random.Range(0.0f , 100.0f);
            if(lifeDropChance > 90.0f){
                ExtraLife newLife = Instantiate(ExtraLifePrefab,this.transform.position,Quaternion.identity);
            }

            Instantiate(destroyFXPrefab,this.EnemyRigidbody.position,Quaternion.identity);
            if(EnemyDeathEvent != null){
                EnemyDeathEvent();
            }
            Destroy(this.gameObject);
        }
        
    }

    public void Shoot(){
        
        _enemyShootSoudFX.Play();
        Bullet bullet = Instantiate(this.BulletPrefab, this.transform.position, this.transform.rotation);//cria um projétil
        bullet.Project(this.transform.up);//Define a direção do projetil
        
    }
}
