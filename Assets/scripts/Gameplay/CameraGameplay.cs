using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGameplay : MonoBehaviour
{
    public Camera myCam;
    public static CameraGameplay _instance;

    void Awake()
    {
        _instance = this;
    }
}
