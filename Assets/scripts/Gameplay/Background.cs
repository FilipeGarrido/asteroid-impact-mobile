using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public Rigidbody2D backgroundRigidbody;
    public SpriteRenderer BGSprite;
    public PlayerScript _player;
    public float parallaxFX = 0.1f;
    public float MaxSpeed = 0.1f;

    void Awake()
    {
        backgroundRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update(){

        Vector3 PlayerPos = _player.playerRigidbody.position;
        Vector3 BackgroundPos = backgroundRigidbody.position;
        Vector3 GoTo = new Vector3 (PlayerPos.x-BackgroundPos.x, PlayerPos.y-BackgroundPos.y, 0.0f);
        float horizontalLimit = (BGSprite.size.x/3.0f);
        float verticalLimit = BGSprite.size.y/3.0f;
        
        //Se a srite do player estiver ativada, Executa:
        if(_player._playerSprite.enabled == true){  
            //Se a posição do Background estiver diferente da posição do player vezes 0.01,Executa
            if(BackgroundPos != PlayerPos*0.01f){
                backgroundRigidbody.velocity = GoTo*parallaxFX; 
                //Limita a velocidade
                if(backgroundRigidbody.velocity.magnitude > MaxSpeed){
                    backgroundRigidbody.velocity = Vector2.ClampMagnitude(backgroundRigidbody.velocity,MaxSpeed);
                } 
                //Se a posição do background for maior ou igual ao tamanho 
                if(Mathf.Abs(BackgroundPos.x) >= (horizontalLimit)){
                    BackgroundPos.x = Mathf.Clamp(BackgroundPos.x,-horizontalLimit,horizontalLimit);
                }
                if(Mathf.Abs(BackgroundPos.x) >= (verticalLimit)){
                    BackgroundPos.y = Mathf.Clamp(BackgroundPos.y,-verticalLimit,verticalLimit);
                }
                backgroundRigidbody.position = new Vector3 (BackgroundPos.x,BackgroundPos.y,0.0f);
            }
            //Se atingir o ponto desejado para
            else {
               backgroundRigidbody.velocity = Vector3.zero;
            }

        }
        //Se a sprite do player não estiver ativada para
        else{
            backgroundRigidbody.velocity = Vector3.zero;
        }

    }
}
