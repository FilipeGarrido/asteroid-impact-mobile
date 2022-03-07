using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{   
    //Variável para armazenar o rigidboddy do asteroid
    private Rigidbody2D _rigidbody;
    public AsteroidFX destroyFXPrefab;//Variável para receber o som e o efeito do asteroid destruído
    public int DestroyAmount = 2;
    public float size = 1.0f; //tamanho do asteróide (default)
    public float minSize = 0.5f; //Tamanho minimo
    public float maxSize = 2.0f;//Tamanho máximo
    public float speed = 10.0f; //Velocidade
    public float maxLifeTime = 30.0f; //Tempo em que o asteroide permanece "vivo", sem ser atingido por um projétil
    public static System.Action asteroidDestoyEvent = null;
    void Awake()
    {   
        _rigidbody = GetComponent<Rigidbody2D>();//Recebe o Rigidbody do asteroid
    }
    void Start()
    {   
        //Ao spawnar, o asteroid inicia com um angulo e um tamanho aleatório
        this.transform.eulerAngles = new Vector3 (0.0f , 0.0f , Random.value * 360.0f);
        this.transform.localScale = Vector3.one * this.size;
        //A massa terá a mesma escala do tamanho
        _rigidbody.mass = this.size;
        //O Asteroid é destruido após um tempo, para não ficar muito tempo carregado no jogo, caso o player não o destrua
        Destroy (this.gameObject, this.maxLifeTime);
    }

    //Função para lançar o Asteroid ao spawnar
    public void SetTrajectory(Vector2 direction)
    {   
        _rigidbody.AddForce(direction * this.speed);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        //Se a colisão for com um projétil, toca o audio do asteróide destruindo
        if(other.gameObject.tag == "Bullet"){
            //Se o tamanho do asteróide, dividido por 2 for mais ou igual a meio (tamanho mínimo), roda a função CreateSplit
            if((this.size / 2.0f ) >= 0.5f){
                for(int i = 0; i< DestroyAmount; i++){
                    CreateSplit();
                }
            }
            if(asteroidDestoyEvent != null){
                //Se for um asteroid pequeno, chama o evento 3 vezes, totalizando 3 pontos no score;
                if(this.size >= 1.75f){
                    asteroidDestoyEvent();
                }
                //Se for um asteroid médio, chama o evento 2 vezes, totalizando 2 pontos no score;
                else if(this.size >= 1.25f){
                    for( int i=0; i <2 ; i++){
                        asteroidDestoyEvent();
                    }
                }
                //Se for um asteroid Grande
                else if(this.size >= 0.65f){
                    for( int i=0; i < 3 ; i++){
                        asteroidDestoyEvent();
                    }
                }
                else{
                    for( int i=0; i < 5 ; i++){
                        asteroidDestoyEvent();
                    }
                }
            }
            Instantiate(destroyFXPrefab,this._rigidbody.position,Quaternion.identity);
            Destroy(this.gameObject);
        }
        if((other.gameObject.tag == "MinionBoss") || (other.gameObject.tag == "Boss") || (other.gameObject.tag == "Player"))
        {
            Instantiate(destroyFXPrefab,this._rigidbody.position,Quaternion.identity);
            Destroy(this.gameObject);
        }
        if(other.gameObject.tag == "BulletPowerUp"){
            Instantiate(destroyFXPrefab,this._rigidbody.position,Quaternion.identity);
            Destroy(this.gameObject);
            if(asteroidDestoyEvent != null){
                asteroidDestoyEvent();
            }
        }
        
    }

    //Função que gera 2 asteróides menores
    void CreateSplit()
    {
        Vector2 position = this.transform.position; //Armazena a posição do asteróide anterior
        position += Random.insideUnitCircle * 0.05f;//Posição inicial do novo asteróide menor
        Asteroid smallAsteroid = Instantiate(this,position,this.transform.rotation); //Cria um asteróide menor
        smallAsteroid.size = this.size * 0.5f;//Define o tamanho do asteróide menor como sendo o tamanhdo do anterior vezes meio
        smallAsteroid.SetTrajectory(Random.insideUnitCircle.normalized *this.speed);//Define uma trajetória para o novo asteróide
    }
}
