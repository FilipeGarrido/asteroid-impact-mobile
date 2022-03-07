using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public  Rigidbody2D playerRigidbody;//Variável para armazenar o rigidbody do player
    public ShootButton ShootButtonEnable;
    public Bullet bulletPrefab;//Variavel para criar o peojetil
    public Bullet bulletPowerUpPrefab;//Variavel para criar o peojetil
    public Joystick PlayerJoystick;
    public AsteroidFX destroyFXPrefab;
    private CircleCollider2D _playerCollider;
    public SpriteRenderer _playerSprite;
    [SerializeField] private AudioSource shootSoudFX;
    [SerializeField] private AudioSource _powerUpShootSoudFX;
    public float MaxSpeed = 5.0f;
    public float Acelerate = 5.0f;
    public float PowerAngle = 45.0f;
    public int powerBulletsAmount = 100;
    public bool DoubleShoot = false;
    private bool _autoShoot = false;
    private Vector3 playerDirection;
    public static System.Action playerLivesEvent = null;
    private readonly string FirstPlay = "FirstPlay";


    void Awake()
    {   
        int _firstPlay = PlayerPrefs.GetInt(FirstPlay);
        if(_firstPlay == 0){
            _autoShoot = false;
        }
        else{
            LoadShootPrefs();
        }
        
        playerRigidbody = GetComponent<Rigidbody2D>();//Armazena o rigidbody do player
        _playerCollider = GetComponent<CircleCollider2D>();//Armazena o rigidbody do player
        _playerSprite = GetComponent<SpriteRenderer>();//Armazena o rigidbody do player
        this._playerCollider.enabled = false;
        this._playerSprite.enabled = false;
        ShootPowerUpScript.ShootPowerUpEvent += DoubleShootPowerUp; 
    }
    void OnDestroy()
    {
        ShootPowerUpScript.ShootPowerUpEvent -= DoubleShootPowerUp;
    }

    void Update(){
        
        if( this._playerSprite.enabled == true){
            playerDirection = new Vector3 (PlayerJoystick.Horizontal, PlayerJoystick.Vertical , 0.0f); 
            if(playerDirection != Vector3.zero){
                transform.up = playerDirection;
            }
        }
        else {
            playerDirection = Vector3.zero;
            PlayerJoystick.AxisOptions = 0.0f;
        }

    }
    void FixedUpdate()
    {
        Vector3 speed = playerDirection * Acelerate;
        playerRigidbody.AddForce(speed);

        if(playerRigidbody.velocity.magnitude > MaxSpeed){
            playerRigidbody.velocity = Vector2.ClampMagnitude(playerRigidbody.velocity,MaxSpeed);
        }
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        //Se a colisão for com um projétil, toca o audio do asteróide destruindo
        if((other.gameObject.tag == "Asteroid") || (other.gameObject.tag == "EnemyBullet") || (other.gameObject.tag == "Enemy") || (other.gameObject.tag == "MinionBoss") || (other.gameObject.tag == "Boss")){
            Death();
            if(playerLivesEvent != null){
                playerLivesEvent();
            }
        }

    }

    public void Death(){
        playerRigidbody.velocity = Vector3.zero;
        playerRigidbody.angularVelocity = 0.0f;
        Instantiate(destroyFXPrefab,playerRigidbody.position,Quaternion.identity);
        this._playerCollider.enabled = false;
        this._playerSprite.enabled = false;    
    }
    public void Respawn(){
        playerRigidbody.velocity = Vector3.zero;
        playerRigidbody.angularVelocity = 0.0f;
        this._playerCollider.enabled = true;
        this._playerSprite.enabled = true;
   
    }

    public void Shoot(){
        StartCoroutine(Shooting());
    }
    
    IEnumerator Shooting(){
        if(this._playerSprite.enabled){
            if(this.gameObject.layer == 7){    
                AudioSource Shoot = Instantiate(shootSoudFX);
                Shoot.Play();
                Bullet bullet = Instantiate(this.bulletPrefab, this.transform.position, this.transform.rotation);//cria um projétil
                bullet.Project(this.transform.up);//Define a direção do projetil
                if(DoubleShoot){
                    yield return new WaitForSeconds(0.05f);
                    AudioSource doubleShoot = Instantiate(shootSoudFX);
                    doubleShoot.Play();
                    Bullet secBullet = Instantiate(this.bulletPrefab, this.transform.position, this.transform.rotation);//cria um projétil
                    secBullet.Project(this.transform.up);//Define a direção do projetil
                }
            }
        }
    }

    

    public void PlayerPowerUp()
    {
        Vector2 spawnDirection = Random.insideUnitCircle.normalized;
        for(int i=0;i<powerBulletsAmount;i++)
        {
            AudioSource powerUpExplosion = Instantiate(_powerUpShootSoudFX);
            powerUpExplosion.Play();
            Vector2 spawnPoint = this.transform.position;
            Quaternion rotation = Quaternion.AngleAxis(PowerAngle * i, Vector3.forward);
            Vector2 Direction = rotation * spawnDirection;
            Bullet bullet = Instantiate(bulletPowerUpPrefab, spawnPoint, Quaternion.identity);
            bullet.Project(Direction);
        }
    }
    private void DoubleShootPowerUp()
    {
        DoubleShoot = true;
    }

    public void SaveShootPrefs()
    {   
        _autoShoot = !_autoShoot;
        PlayerPrefsX.SetBool("AutoShoot",_autoShoot);
        if(_autoShoot)
        {
            AutoShoot();
        }
        else{
            StopAutoShoot();
        }
    }
   private void LoadShootPrefs()
    {   
        _autoShoot = PlayerPrefsX.GetBool("AutoShoot");
        if(_autoShoot)
        {
            AutoShoot();
        }
        else{
            StopAutoShoot();
        }
    }

    private void AutoShoot()
    {
        InvokeRepeating(nameof(Shoot),0.1f,0.3f);
        ShootButtonEnable.gameObject.SetActive(false);
    }
    private void StopAutoShoot()
    {
        CancelInvoke(nameof(Shoot));
        ShootButtonEnable.gameObject.SetActive(true);
    }

}
