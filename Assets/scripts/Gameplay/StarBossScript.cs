using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBossScript : MonoBehaviour
{
    private Rigidbody2D _bossRigidbody;//Variável para armazenar o rigidbody
    private SpriteRenderer _sprite;
    [SerializeField] private AudioSource _bossGrunt;
    [SerializeField] private StarBossMinion _starMinionPrefab;
    private Vector3 _currentEulerAngle;
    private Quaternion _currentRotation;
    public ExplosionPowerUpScript PowerUpPrefab;
    public AsteroidFX destroyFXPrefab;
    public float MinionAngle = 45.0f;
    public float TimeAttackRate = 5;
    public int AttackAmount = 5;
    public float Speed = 20.0f; //Velocidade
    public float RotationSpeed = 1.0f;
    public int RotationTime = 5;
    private int _lastRotationTime;
    public float maxLifePoints = 100.0f;
    public static System.Action BossDeathEvent = null;
    public int ShootTime;
    public int ShootTimeRate;
    private bool _init = true;


    void Awake()
    {
        _bossRigidbody = GetComponent<Rigidbody2D>();//Armazena o rigidbody
        _sprite = GetComponent<SpriteRenderer>();
        _lastRotationTime = RotationTime;
        _bossRigidbody.AddForce(new Vector2(2 , 2)*this.Speed);
        InvokeRepeating(nameof(Attack),1,TimeAttackRate);
    }

    void Update(){

        if(Mathf.Abs(RotationTime)>0){
            if(Mathf.Sign(RotationTime)>0){
                RotationTime --;
            }
            else{
                RotationTime ++;
            }
            _currentEulerAngle += new Vector3(0, 0, 1) * Time.deltaTime * RotationSpeed * Mathf.Sign(RotationTime);
        }
        else{
            if(_init){
                RotationTime = -_lastRotationTime * 2;
                _lastRotationTime = RotationTime/2;
                _init = !_init;
            }
            else{
                RotationTime = -_lastRotationTime;
                _lastRotationTime = RotationTime;   
            }
            
        }
        //moving the value of the Vector3 into Quanternion.eulerAngle format
        _currentRotation.eulerAngles = _currentEulerAngle;
        //apply the Quaternion.eulerAngles change to the gameObject
        transform.rotation = _currentRotation;
        
        Camera _cam = CameraGameplay._instance.myCam;
        
        var maxX = _cam.orthographicSize * _cam.aspect;
        var maxY = _cam.orthographicSize;

        var leftLimit = -maxX;
        var rightLimit = maxX;
        var upLimit = maxY;
        var downLimit = - maxY;
        var margin = 1.2f;

        Vector2 pos = _bossRigidbody.position;
        Vector2 spd = _bossRigidbody.velocity;

        if(pos.x <= leftLimit + margin){
            spd.x = -spd.x;
            pos.x += 0.5f;
        }
        else if(pos.x >= rightLimit - margin){
            spd.x = -spd.x;
            pos.x -= 0.5f;
        }
        else if(pos.y >= upLimit - margin){
            spd.y = -spd.y;
            pos.y -= 0.5f;
        }
        else if(pos.y <= downLimit + margin){
            spd.y = -spd.y;
            pos.y += 0.1f;
        }
        _bossRigidbody.velocity = spd;
        _bossRigidbody.position = pos;

        if(maxLifePoints <= 0)
        {   
            if(BossDeathEvent!=null)
            {
                BossDeathEvent();
            }
            ExplosionPowerUpScript powerUpDrop = Instantiate(PowerUpPrefab, this.transform.position, Quaternion.identity);
            AsteroidFX destroyFX = Instantiate(destroyFXPrefab,this.transform.position,Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
    private void Attack()
    {
        AudioSource bossAttack = Instantiate(_bossGrunt);
        bossAttack.Play();
        Vector3 spawnDirection = Random.insideUnitCircle.normalized;
        for(int i=0;i<AttackAmount;i++)
        {
            Vector3 spawnPoint = this.transform.position;
            Quaternion rotation = Quaternion.AngleAxis(MinionAngle * i, Vector3.forward);
            Vector3 Direction = rotation * spawnDirection;
            StarBossMinion minion = Instantiate(_starMinionPrefab, spawnPoint, rotation);
            minion.ThrowMinions(Direction);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Bullet")
        {   
            StartCoroutine(BlinkEffect());
            maxLifePoints--;
        }
    }

    IEnumerator BlinkEffect(){
        _sprite.color = Color.black;
        yield return new WaitForSeconds(0.01f);
        _sprite.color = Color.white;
    }
}
