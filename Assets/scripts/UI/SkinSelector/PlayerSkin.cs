using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkin : MonoBehaviour
{
    public SkinsDatabase SkinsDB;
    public SpriteRenderer CurrentPlayerSkin;

    private int _selectedOption = 0;
    private static readonly string _skinSelected = "SkinSelected";
    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey(_skinSelected)){
            _selectedOption = 0;
            UpdateSkin(_selectedOption);
        }
        else{
            LoadSkin();
        }
        
    }

    private void UpdateSkin(int _selectedOption)
    {
        Skins skin = SkinsDB.GetSkins(_selectedOption);
        CurrentPlayerSkin.sprite = skin.PlayerSkin;
    }
    public void LoadSkin(){
        _selectedOption = PlayerPrefs.GetInt(_skinSelected);
        UpdateSkin(_selectedOption);
    }

}
