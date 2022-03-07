using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToroidalPosition : MonoBehaviour
{
    public float PADDING = 0.5f;
    public Rigidbody2D _rigidbody;
    private float leftLimit;
    private float rightLimit;
    private float upLimit;
    private float downLimit;
    
    void Update()
    {
        Camera _cam = CameraGameplay._instance.myCam;
        
        var maxX = _cam.orthographicSize * _cam.aspect;
        var maxY = _cam.orthographicSize;

        leftLimit = -maxX;
        rightLimit = maxX;
        upLimit = maxY;
        downLimit = - maxY;

        Vector2 pos = _rigidbody.position;

        if(pos.x < leftLimit - PADDING){
            pos.x = rightLimit + PADDING;
        }
        else if(pos.x > rightLimit + PADDING){
            pos.x = leftLimit - PADDING;
        }
        else if(pos.y > upLimit + PADDING){
            pos.y = downLimit - PADDING;
        }
        else if(pos.y < downLimit - PADDING){
            pos.y = upLimit + PADDING;
        }
        _rigidbody.position = pos;

    }
}
