using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinSelectorMenu : MonoBehaviour
{
    [SerializeField] private UI UiCallBack;
    
    void Start()
    {
        UiCallBack.Back();
    }

}
