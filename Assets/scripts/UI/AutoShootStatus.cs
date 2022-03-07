using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class AutoShootStatus : MonoBehaviour
{
    public TMP_Text StatusText;
    private bool _autoShoot;
    void Awake()
    {
        _autoShoot = PlayerPrefsX.GetBool("AutoShoot");
        UpdateSatus();
    }
    
    void UpdateSatus()
    {
        string status;

        if(_autoShoot){
            status = "ON";
            StatusText.color = new Color32(0 , 255 , 0 , 255 );
        }
        else{
            status = "OFF";
            StatusText.color = new Color32(255 , 0 , 0 , 255 );
        }

        StatusText.text = status;
    }

    public void ChangeStatus()
    {
      _autoShoot = !_autoShoot;
      UpdateSatus();  
    }
}
