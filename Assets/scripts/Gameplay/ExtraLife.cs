using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraLife : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    [SerializeField] private AudioSource Take;
    private float leftLimit;
    private float rightLimit;
    private float upLimit;
    private float downLimit;
    public int speed = 1; 
    public int spawn = 30;
    public float LifeTime;
    public static System.Action TakeNewLifeEvent = null;
 
    void Start(){
        _rigidbody = GetComponent<Rigidbody2D>();
        Destroy(this.gameObject,LifeTime);
        Vector2 _dirVector = new Vector2 (Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f));
        SetDrop(_dirVector);
    }
    void Update()
    {
        if(spawn<=0){

            Camera _cam = CameraGameplay._instance.myCam;
            
            var maxX = _cam.orthographicSize * _cam.aspect;
            var maxY = _cam.orthographicSize;

            leftLimit = -maxX;
            rightLimit = maxX;
            upLimit = maxY;
            downLimit = - maxY;

            Vector2 pos = _rigidbody.position;
            Vector2 spd = _rigidbody.velocity;

            if(pos.x <= leftLimit ){
                spd.x = -spd.x;
                pos.x += 0.1f;
            }
            else if(pos.x >= rightLimit ){
                spd.x = -spd.x;
                pos.x -= 0.1f;
            }
            else if(pos.y >= upLimit ){
                spd.y = -spd.y;
                pos.y -= 0.1f;
            }
            else if(pos.y <= downLimit ){
                spd.y = -spd.y;
                pos.y += 0.1f;
            }
            _rigidbody.position = pos;
            _rigidbody.velocity = spd;

        }
        else
        {
            spawn --;
        }
    }

    public void SetDrop(Vector2 _direction)
    {   
        _rigidbody.AddForce(_direction*this.speed);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player"){
            if(TakeNewLifeEvent != null){
                TakeNewLifeEvent();
            }
            AudioSource extraLifeTake = Instantiate(Take);
            extraLifeTake.Play();
            Destroy(this.gameObject);
        }
    }
}

