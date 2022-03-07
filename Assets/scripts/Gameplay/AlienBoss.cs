using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienBoss : MonoBehaviour
{
    private Rigidbody2D _bossRigidbody;//Variável para armazenar o rigidbody
    private PolygonCollider2D _collider;//Variável para armazenar o a "caixa" de colisão
    private SpriteRenderer _sprite;//Variável para armazenar a sprite
    [SerializeField] private AudioSource _bossGrunt;//Variável para armazenar o audio do ataque do boss
    [SerializeField] private StarBossMinion _fireBallPrefab;//variável para armazenar os prefabs do ataque do boss
    private Vector3 _currentEulerAngle;//Variavel que armazena o angulo atual em vetor
    private Quaternion _currentRotation;//variável que armazena o angulo atual em Quaternion
    public ShootPowerUpScript ShootPowerUpPrefab;//Variável que armazena o prefab do drop do powerup
    public AsteroidFX destroyFXPrefab;
    public float MinionAngle = 45.0f;
    public float TimeAttackRate = 10;
    public int AttackAmount = 1;
    public float Speed = 20.0f; //Velocidade
    public float RotationSpeed = 1.0f;
    public int RotationTime = 50;
    private int _lastRotationTime;
    public float maxLifePoints = 500.0f;
    private float _actualLifePoints;
    public static System.Action BossDeathEvent = null;
    public int ShootTime;
    public int ShootTimeRate;
    public float AttackHeight;
    private bool _movingBack = false;
    private bool _attack = false;
    private bool _goBack = false;
    private float horSpriteSize;
    private float verSpriteSize;


    void Awake()
    {
        _bossRigidbody = GetComponent<Rigidbody2D>();//Armazena o rigidbody
        _sprite = GetComponent<SpriteRenderer>();
        _collider = GetComponent<PolygonCollider2D>();
        _lastRotationTime = RotationTime;
        _bossRigidbody.AddForce(new Vector2(-5 , 0)*this.Speed);
        horSpriteSize = _sprite.size.x;
        verSpriteSize = _sprite.size.y;
        _actualLifePoints = maxLifePoints;
        this._collider.enabled = false;
    }

    void Update(){
          
        Camera _cam = CameraGameplay._instance.myCam;
        
        var maxX = _cam.orthographicSize * _cam.aspect;
        var maxY = _cam.orthographicSize;

        var leftLimit = -maxX;
        var rightLimit = maxX;
        var upLimit = maxY;
        var downLimit = - maxY;

        Vector2 pos = _bossRigidbody.position;
        Vector2 spd = _bossRigidbody.velocity;

        //Recuo do boss
        if(_movingBack){
            if(pos.x >= (rightLimit + 4)){
                _bossRigidbody.velocity = Vector3.zero;
                this.transform.position = new Vector2 (leftLimit - 4 , pos.y);
                this.transform.localScale = new Vector3 (-this.transform.localScale.x , this.transform.localScale.y , this.transform.localScale.z);
                StartCoroutine(BackToFight());
            }
            else if(pos.x <= (leftLimit - 4)){
                _bossRigidbody.velocity = Vector3.zero;
                this.transform.position = new Vector2 (rightLimit + 4 , pos.y);
                this.transform.localScale = new Vector3 (-this.transform.localScale.x , this.transform.localScale.y , this.transform.localScale.z);
                StartCoroutine(BackToFight());
            }
        }

        //Avança para atacar
        else {
            if((pos.x >= (rightLimit - 3)) && (pos.x <= (rightLimit - 2.5))){
                _bossRigidbody.velocity = Vector3.zero;
                pos.x -= 0.5f;
                this._collider.enabled = true;
                InvokeRepeating(nameof(Attack),1,TimeAttackRate);
            }

            if((pos.x <= (leftLimit + 3)) && (pos.x >= (leftLimit + 2.5))){
                _bossRigidbody.velocity = Vector3.zero;
                pos.x += 0.5f;
                this._collider.enabled = true;
                InvokeRepeating(nameof(Attack),1,TimeAttackRate);
            }

        }

        _bossRigidbody.position = pos;

        // Mini recuo ao atacar
        if(_attack)
        {
            if(Mathf.Abs(RotationTime)>0){
                if(Mathf.Sign(RotationTime)>0){
                    RotationTime --;
                }
                else{
                    RotationTime ++;
                }
                _currentEulerAngle += new Vector3(0, 0, 1) * Time.deltaTime * RotationSpeed * Mathf.Sign(RotationTime)*(-this.transform.localScale.x);
            }
            else
            {
                RotationTime = -_lastRotationTime;
                _lastRotationTime = RotationTime;
                if(_goBack)
                {
                    _attack = false;
                    _goBack = false;
                    _currentEulerAngle.z = 0.0f;
                }
                else
                {
                    _goBack = true;
                }                  
            }

        }

        //moving the value of the Vector3 into Quanternion.eulerAngle format
        _currentRotation.eulerAngles = _currentEulerAngle;
        //apply the Quaternion.eulerAngles change to the gameObject
        transform.rotation = _currentRotation;

        //Se morrer
        if(_actualLifePoints <= 0)
        {   
            if(BossDeathEvent!=null)
            {
                BossDeathEvent();
            }
            ShootPowerUpScript powerUpDrop = Instantiate(ShootPowerUpPrefab, this.transform.position, Quaternion.identity);
            AsteroidFX destroyFX = Instantiate(destroyFXPrefab,this.transform.position,Quaternion.identity);
            Destroy(this.gameObject);
        }
        else if(_actualLifePoints != maxLifePoints){
            if(_actualLifePoints%100 == 0)
            {
                this._collider.enabled = false;
                CancelInvoke(nameof(Attack));
                Speed = -Speed;
                if(_bossRigidbody.velocity == Vector2.zero){
                    _bossRigidbody.AddForce(new Vector2(-5 , 0)*this.Speed);
                }
                _movingBack = true;
                _actualLifePoints --;
            }
        }
        Debug.Log(_movingBack.ToString());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Bullet")
        {   
            StartCoroutine(BlinkEffect());
            _actualLifePoints--;
        }
    }

    IEnumerator BlinkEffect(){
        _sprite.color = Color.black;
        yield return new WaitForSeconds(0.01f);
        _sprite.color = Color.white;
    }
    IEnumerator BackToFight(){
        _movingBack = false;
        yield return new WaitForSeconds(2);
        _bossRigidbody.AddForce(new Vector2(-5 , 0)*this.Speed);
    }
    private void Attack(){
        _attack = true;
        _bossGrunt.Play();
        GameObject player = GameObject.Find ("Player");
        Transform playerTransform = player.transform;
        Vector3 playerDirection = playerTransform.position;
        for(int i=0;i<AttackAmount;i++)
        {
            Vector3 spawnPoint = new Vector3 ( this.transform.position.x + 1*(-this.transform.localScale.x) + 1*(-this.transform.localScale.x)*i , this.transform.position.y + AttackHeight , 1.0f);
            Vector3 Direction = playerDirection - spawnPoint;
            StarBossMinion fireBall = Instantiate(_fireBallPrefab, spawnPoint, Quaternion.identity);
            fireBall.ThrowMinions(Direction.normalized);
        }    
    }
}
