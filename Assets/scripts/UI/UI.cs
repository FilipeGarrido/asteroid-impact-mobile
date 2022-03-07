using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class UI : MonoBehaviour
{
    public GetMenu StartMenu;
    public GetMenu PauseMenu;
    public SkinSelectorMenu SkinSelectorMenu;
    public ScoreBoard ScoreBoardMenu;
    public GetMenu GamePlayMenu;
    public SoundsManager SoundManager;
    public GetButtons PowerUpButton;
    public PlayerScript Player;
    public AsteroidSpawner Spawner;
    public StarBossScript StarBossPrefab;
    public TMP_Text TitleText;
    public TMP_Text scoreText;
    public TMP_Text livesText;
    public TMP_Text StartButtonText;
    public AudioSource gameMusic;
    public AudioSource gameOverSound;
    public AudioSource gameStartSound;
    public Background _background;
    public int score;
    public int lives = 3;
    public float initialRespawnTime = 3.0f;
    public float respawnTime = 1.0f;
    public float invulnTime = 3.0f;
    private bool _justShow = false;
    private string _lastState;
    private bool _bossState = false;
        
    void Awake()
    {
        _lastState = "GameInit";
        ScoreBoardMenu.gameObject.SetActive(true);
        SoundManager.gameObject.SetActive(true);
        SkinSelectorMenu.gameObject.SetActive(true);
        gameMusic.Play();
        Asteroid.asteroidDestoyEvent += AsteroidDestroy;
        PlayerScript.playerLivesEvent += PlayerLives;
        EnemyScript.EnemyDeathEvent += AsteroidDestroy;
        ExtraLife.TakeNewLifeEvent += AddLives;
        ExplosionPowerUpScript.ExplosionPowerUpEvent += PowerUp;
        StarBossScript.BossDeathEvent += StarBossDeath;
        AlienBoss.BossDeathEvent += AlienBossDeath;
    }
    void OnDestroy(){
       Asteroid.asteroidDestoyEvent -= AsteroidDestroy;
       PlayerScript.playerLivesEvent -= PlayerLives;
       EnemyScript.EnemyDeathEvent -= AsteroidDestroy;
       ExtraLife.TakeNewLifeEvent -= AddLives;
       ExplosionPowerUpScript.ExplosionPowerUpEvent -= PowerUp;
       StarBossScript.BossDeathEvent -= StarBossDeath;
       AlienBoss.BossDeathEvent -= AlienBossDeath;
    }
    void AsteroidDestroy(){
       score += 10;
       ScoreUpdate();
    }

    void ScoreUpdate()
    {
        scoreText.text = "Score: " + score.ToString();
        if(score >= 1000){
            if(score % 100 == 0){
                Spawner.SpawnEnemyShip();
            }
        }
        if(score == 10000){
            Spawner.SpawnStarBoss();
            Spawner.StopSpawn();
            if(GameObject.Find("Asteroid(Clone)")!=null){
                Destroy(GameObject.Find("Asteroid(Clone)"));
            }
            _bossState = true;
        }
        if(score == 5000){
            Spawner.SpawnAlienBoss();
            Spawner.StopSpawn();
            if(GameObject.Find("Asteroid(Clone)")!=null){
                Destroy(GameObject.Find("Asteroid(Clone)"));
            }
            _bossState = true;
        }
     }
    void StarBossDeath()
    {
        for(int i=0; i<200;i++)
        {
            score += 10;
        }
        ScoreUpdate();
        Spawner.StartSpawn();
        _bossState = false;
    }
    void AlienBossDeath()
    {
        for(int i=0; i<100;i++)
        {
            score += 10;
        }
        ScoreUpdate();
        Spawner.StartSpawn();
        _bossState = false;
    }
    void PlayerLives(){
        
        lives --;
        
        if(lives >= 0){
            LivesUpdate();
        }else{
            ScoreBoardScreen();
            PowerUpButton.gameObject.SetActive(false);
            CancelInvoke(nameof(PowerUp));
        }
       
    }
    void AddLives(){
        if(lives<3){
            lives++;
            livesText.text = "Lives: " + lives.ToString();
        }
    }
    void LivesUpdate(){
        livesText.text = "Lives: " + lives.ToString();
        Invoke(nameof(Respawn) , this.respawnTime);
    }

    void Respawn(){
        this.Player.transform.position = Vector3.zero;
        this.Player.gameObject.layer = LayerMask.NameToLayer("Invulnerability");
        this.Player.Respawn();
        respawnTime = 1.0f;
        Invoke(nameof(TurnOnCollision),invulnTime);
    }    

    void TurnOnCollision(){
        this.Player.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    public void GameStart(){
        StartMenu.gameObject.SetActive(false);
        GamePlayMenu.gameObject.SetActive(true);
        Spawner.gameObject.SetActive(true);
        Spawner.StartSpawn();
        gameStartSound.Play();
        respawnTime = initialRespawnTime;
        lives = 3;
        score = 0;
        LivesUpdate();
        ScoreUpdate();
        _lastState = "GameStart";
    }
    public void RestartGame(){
        Player.Death();
        PowerUpButton.gameObject.SetActive(false);
        Player.DoubleShoot = false;
        if(GameObject.Find("Asteroid(Clone)")!=null){
            Destroy(GameObject.Find("Asteroid(Clone)"));
        }
        PauseMenu.gameObject.SetActive(false);
        GamePlayMenu.gameObject.SetActive(true);
        Spawner.StartSpawn();
        Spawner.KillBoss();
        gameStartSound.Play();
        respawnTime = initialRespawnTime;
        lives = 3;
        score = 0;
        LivesUpdate();
        ScoreUpdate();
        _lastState = "GameStart";
    }

    public void ShowScoreBoardScreen()
    {
        _justShow = true;
        ScoreBoardScreen();
    }
    void ScoreBoardScreen()
    {   
        if(_lastState == "GameInit"){
            StartMenu.gameObject.SetActive(false);
        }

        ScoreBoardMenu.gameObject.SetActive(true);

        if(_justShow == false){
            ScoreBoardMenu.CheckRecords();
            gameOverSound.Play();
            _background.backgroundRigidbody.position = Vector3.zero;
            Debug.Log("Voltou");
        }
        else{
            ScoreBoardMenu.ShowScoreBoard();
            _justShow = false;
            Debug.Log("Foi");
        }

        StartMenu.gameObject.SetActive(false);
        PauseMenu.gameObject.SetActive(false);
        GamePlayMenu.gameObject.SetActive(false);
        Spawner.StopSpawn();
    }
    void GameOver()
    {   
        Player.DoubleShoot = false;
        PowerUpButton.gameObject.SetActive(false);
        if(GameObject.Find("Asteroid(Clone)")!=null){
            Destroy(GameObject.Find("Asteroid(Clone)"));
        }
        if(GameObject.Find("ShootPowerUp(Clone)")!=null){
            Destroy(GameObject.Find("ShootPowerUp(Clone)"));
        }
        if(GameObject.Find("ExplosionPowerUp(Clone)")!=null){
            Destroy(GameObject.Find("ExplosionPowerUp(Clone)"));
        }
        Spawner.KillBoss();
        Spawner.StopSpawn();
        StartMenu.gameObject.SetActive(true);
        StartButtonText.text = "Try Again";
        TitleText.text = "Game Over";
        _lastState = "GameOver";
    }

    public void PauseGame(){
        PauseMenu.gameObject.SetActive(true);
        GamePlayMenu.gameObject.SetActive(false);
        Spawner.StopSpawn();
        this.Player.gameObject.layer = LayerMask.NameToLayer("Invulnerability");
        _lastState = "GamePaused";
    }

    public void ResumeGame(){
        PauseMenu.gameObject.SetActive(false);
        GamePlayMenu.gameObject.SetActive(true);
        if(!_bossState){
            Spawner.StartSpawn();
        }
        Invoke(nameof(TurnOnCollision),invulnTime);
    }

    public void Configs(){
        this.SoundManager.gameObject.SetActive(true);
        StartMenu.gameObject.SetActive(false);
        ScoreBoardMenu.gameObject.SetActive(false);
        PauseMenu.gameObject.SetActive(false);
        GamePlayMenu.gameObject.SetActive(false);
        if(Player._playerSprite.enabled == true){
            Spawner.StopSpawn();
            this.Player.gameObject.layer = LayerMask.NameToLayer("Invulnerability");
        }       
    }

    public void SkinSelection(){
        StartMenu.gameObject.SetActive(false);
        SkinSelectorMenu.gameObject.SetActive(true);
    }
    public void Back()
    {
        this.SoundManager.gameObject.SetActive(false);
        SkinSelectorMenu.gameObject.SetActive(false);
        if(_lastState == "GameInit"){
            StartMenu.gameObject.SetActive(true);
            ScoreBoardMenu.gameObject.SetActive(false);
        }
        else if(_lastState == "GameStart"){
            GamePlayMenu.gameObject.SetActive(true);
            Spawner.StartSpawn();
            Invoke(nameof(TurnOnCollision),invulnTime);
        }
        else if(_lastState == "GameOver"){
            StartMenu.gameObject.SetActive(true);      
        }
        else if(_lastState == "ScoreBoard"){
            ScoreBoardMenu.gameObject.SetActive(true);  
        }
        else if(_lastState == "GamePaused"){
            PauseMenu.gameObject.SetActive(true);
        }
    }

    public void BackToStart(){
        if(_lastState == "GameInit"){
            StartMenu.gameObject.SetActive(true);
            ScoreBoardMenu.gameObject.SetActive(false);
        }
        else{
            GameOver();
            ScoreBoardMenu.gameObject.SetActive(false);
        }
    }

    private void PowerUp()
    {
        PowerUpButton.gameObject.SetActive(true);
    }

    public void PowerUpUsed()
    {
        PowerUpButton.gameObject.SetActive(false);
        Invoke(nameof(PowerUp),60.0f);
    }

    public void ExitGame(){
        Application.Quit();
    }
}
