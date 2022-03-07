using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public Asteroid asteroidPrefab;//Variável para criar um novo asteroide
    public EnemyScript EnemyPrefab;//Variável para criar um novo asteroide
    public StarBossScript StarBossPrefab;
    public AlienBoss AlienBossPrefab;
    public float trajectoryVariance = 15.0f ; //angulo limite de variação
    private float spawnDistance; //Distancia do centro
    public float spawnRateTime = 2.0f;//Numero de repetições do spawn
    public float spawnTime = 1.0f;//Tempo entre repetições
    public int spawnAmount = 3;//Quantidade de asteroids a serem criados
    public float padding = 0.5f; //Limite para spawn fora da tela
    void Awake()
    {
        this.gameObject.SetActive(false);
    }
    
    //Fução de spawn dos asteroids
    public void Spawn()
    {
        Camera _cam = CameraGameplay._instance.myCam; //Recebe a camera do cenário atual
        
        var maxX = _cam.orthographicSize * _cam.aspect;//Recebe a largura da camera
        var maxY = _cam.orthographicSize;//Recebe a altura da camera

        float leftLimit = -maxX;//limite da esquerda
        float rightLimit = maxX;//limite da direita
        float upLimit = maxY;//limite superior
        float downLimit = - maxY;//limite inferior

        spawnDistance = Mathf.Max(maxX,maxY);

        for(int i = 0; i<this.spawnAmount ; i++){
           
            Vector3 spawnDirection = Random.insideUnitCircle.normalized * (this.spawnDistance + padding); //Direção de spawn
            
            //Verifica se o spawn está dentro dos limites da camera;
            if (spawnDirection.x < leftLimit){
                spawnDirection.x = leftLimit - padding;
            }
            if (spawnDirection.x > rightLimit){
                spawnDirection.x = rightLimit + padding;
            }
            if (spawnDirection.y < downLimit){
                spawnDirection.y = downLimit - padding;
            }
            if (spawnDirection.y > upLimit){
                spawnDirection.y = upLimit + padding;
            }

            Vector3 spawnPoint = this.transform.position + spawnDirection; //Posição inicial

            float variance = Random.Range(-this.trajectoryVariance,this.trajectoryVariance);//variação do angulo
            Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward); //angulo inicial
            Vector3 Direction = rotation * -spawnDirection;
            
            Asteroid asteroid = Instantiate(asteroidPrefab, spawnPoint, rotation);//Cria um asteróide
            asteroid.size = Random.Range(asteroid.minSize, asteroid.maxSize);//Define um tamanho aleatório entre o tamanho máximo e mínimo
            asteroid.SetTrajectory(Direction);//Define a trajetória conforme o angulo inicial, no sentido contrário ao do ponto inicial
            
        }
    }
    public void SpawnEnemyShip()
    {
        Camera _cam = CameraGameplay._instance.myCam; //Recebe a camera do cenário atual
        
        var maxX = _cam.orthographicSize * _cam.aspect;//Recebe a largura da camera
        var maxY = _cam.orthographicSize;//Recebe a altura da camera

        float leftLimit = -maxX;//limite da esquerda
        float rightLimit = maxX;//limite da direita
        float upLimit = maxY;//limite superior
        float downLimit = - maxY;//limite inferior

        spawnDistance = Mathf.Max(maxX,maxY);
          
        Vector3 spawnDirection = Random.insideUnitCircle.normalized * (this.spawnDistance + padding); //Direção de spawn
        
        //Verifica se o spawn está dentro dos limites da camera;
        if (spawnDirection.x < leftLimit){
            spawnDirection.x = leftLimit - padding;
        }
        if (spawnDirection.x > rightLimit){
            spawnDirection.x = rightLimit + padding;
        }
        if (spawnDirection.y < downLimit){
            spawnDirection.y = downLimit - padding;
        }
        if (spawnDirection.y > upLimit){
            spawnDirection.y = upLimit + padding;
        }

        Vector3 spawnPoint = this.transform.position + spawnDirection; //Posição inicial

        float variance = Random.Range(-this.trajectoryVariance,this.trajectoryVariance);//variação do angulo
        Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward); //angulo inicial
        Vector3 Direction = rotation * -spawnDirection;
            
        EnemyScript Enemy= Instantiate(EnemyPrefab, spawnPoint, rotation);//Cria um asteróide
        Enemy.SetTrajectory(Direction);//Define a trajetória conforme o angulo inicial, no sentido contrário ao do ponto inicial
            
        
    }

    public void SpawnStarBoss()
    {
        Camera _cam = CameraGameplay._instance.myCam; //Recebe a camera do cenário atual
        
        var maxX = _cam.orthographicSize * _cam.aspect;//Recebe a largura da camera
        var maxY = _cam.orthographicSize;//Recebe a altura da camera

        float leftLimit = -maxX;//limite da esquerda
        float rightLimit = maxX;//limite da direita
        float upLimit = maxY;//limite superior
        float downLimit = - maxY;//limite inferior

        spawnDistance = Mathf.Max(maxX,maxY);
          
        Vector3 spawnDirection = Random.insideUnitCircle.normalized * (this.spawnDistance + padding); //Direção de spawn
        
        //Verifica se o spawn está dentro dos limites da camera;
        if (spawnDirection.x < leftLimit){
            spawnDirection.x = leftLimit - padding;
        }
        if (spawnDirection.x > rightLimit){
            spawnDirection.x = rightLimit + padding;
        }
        if (spawnDirection.y < downLimit){
            spawnDirection.y = downLimit - padding;
        }
        if (spawnDirection.y > upLimit){
            spawnDirection.y = upLimit + padding;
        }

        Vector3 spawnPoint = this.transform.position + spawnDirection; //Posição inicial

        float variance = Random.Range(-this.trajectoryVariance,this.trajectoryVariance);//variação do angulo
        Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward); //angulo inicial
        Vector3 Direction = rotation * -spawnDirection;
        StarBossScript StarBoss = Instantiate(StarBossPrefab,Direction,Quaternion.identity);

    } 
    public void SpawnAlienBoss()
    {
        Camera _cam = CameraGameplay._instance.myCam; //Recebe a camera do cenário atual
        
        var maxX = _cam.orthographicSize * _cam.aspect;//Recebe a largura da camera

        AlienBoss AlienBoss = Instantiate(AlienBossPrefab,new Vector2 (maxX + 4 , 0.0f ),Quaternion.identity);

    } 
    

    public void KillBoss(){
        if(GameObject.Find("StarBoss(Clone)") != null){
            Destroy(GameObject.Find("StarBoss(Clone)"));
        }
        if(GameObject.Find("ExplosionPowerUp(Clone)") != null){
            Destroy(GameObject.Find("PowerUp(Clone)"));
        }
        if(GameObject.Find("AlienBoss(Clone)") != null){
            Destroy(GameObject.Find("AlienBoss(Clone)"));
        }
        if(GameObject.Find("ShootPowerUp(Clone)") != null){
            Destroy(GameObject.Find("ShootPowerUp(Clone)"));
        }
       
    }

    public void StopSpawn(){
        CancelInvoke(nameof(Spawn));
    }
    public void StartSpawn(){
        InvokeRepeating(nameof(Spawn), this.spawnTime, this.spawnRateTime);
    }
}
