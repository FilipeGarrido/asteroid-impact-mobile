using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

public class SkinManager : MonoBehaviour
{
    public SkinsDatabase SkinsDB;
    public SpriteRenderer ShowSkin;
    public TMP_Text ShowSkinName;
    public PlayerSkin SkinSelectedToPlayer;
    public ScoreBoard ScoreBoardData;
    public Rigidbody2D RB2D;
    public GetMenu SkinConfirmedText;
    public GetMenu SkinLocked;
    public TMP_Text SkinLockedText;
    public int Torque;
    private int _selectedOption = 0;
    private int _timer = 300;
    private int score = 0;
    private bool _skinUnlocked = true;
        
    private static readonly string _skinSelected = "SkinSelected";
    // Start is called before the first frame update
    void Start()
    {   
        score = ScoreBoardData._scoreToSave[0];
        if(!PlayerPrefs.HasKey(_skinSelected)){
            _selectedOption = 0;
            
        }
        else{
            LoadSkin();
        }
        UpdateSkin(_selectedOption);
    }

    void Update(){
    
        RB2D.AddTorque(-Torque);
      
        if(_timer<20){
            SkinConfirmedText.gameObject.SetActive(true);
            _timer++;
        }
        else{
            SkinConfirmedText.gameObject.SetActive(false); 
        }
        
    }

    public void NextOption(){
        _selectedOption++;
        if(_selectedOption >= SkinsDB.SkinCounter){
            _selectedOption = 0;
        }
        UpdateSkin(_selectedOption);
    }
    public void BackOption(){
        _selectedOption--;
        if(_selectedOption < 0){
            _selectedOption = SkinsDB.SkinCounter - 1;
        }
        UpdateSkin(_selectedOption);
    }

    private void UpdateSkin(int _selectedOption)
    {
        score = ScoreBoardData._scoreToSave[0];
        Skins skin = SkinsDB.GetSkins(_selectedOption);
        ShowSkin.sprite = skin.PlayerSkin;
        ShowSkinName.text = skin.SkinName;
        if(_selectedOption == 1){
            if(score<1000){
                SkinLocked.gameObject.SetActive(true);
                SkinLockedText.text = "1000 points to Unlock";
            }
            else{
                SkinLocked.gameObject.SetActive(false);
            }
        }else if(_selectedOption == 2){
            if(score<2000){
                SkinLocked.gameObject.SetActive(true);
                SkinLockedText.text = "2000 points to Unlock";
            }
            else{
                SkinLocked.gameObject.SetActive(false);
            }
        }else if(_selectedOption == 3){
            if(score<3000){
                SkinLocked.gameObject.SetActive(true);
                SkinLockedText.text = "3000 points to Unlock";
            }
            else{
                SkinLocked.gameObject.SetActive(false);
            }
        }else if(_selectedOption == 4){
            if(score<4000){
                SkinLocked.gameObject.SetActive(true);
                SkinLockedText.text = "4000 points to Unlock";
            }
            else{
                SkinLocked.gameObject.SetActive(false);
            }
        }
        else if(_selectedOption == 5){
            if(score<8000){
                SkinLocked.gameObject.SetActive(true);
                SkinLockedText.text = "8000 points to Unlock";
            }
            else{
                SkinLocked.gameObject.SetActive(false);
            }
        }
        else if(_selectedOption == 6){
            if(score<10000){
                SkinLocked.gameObject.SetActive(true);
                SkinLockedText.text = "10000 points to Unlock";
            }
            else{
                SkinLocked.gameObject.SetActive(false);
            }
        }
        else if(_selectedOption == 7){
            if(score<15000){
                SkinLocked.gameObject.SetActive(true);
                SkinLockedText.text = "15000 points to Unlock";
            }
            else{
                SkinLocked.gameObject.SetActive(false);
            }
        }
        else if(_selectedOption == 8){
            if(score<15000){
                SkinLocked.gameObject.SetActive(true);
                SkinLockedText.text = "15000 points to Unlock";
            }
            else{
                SkinLocked.gameObject.SetActive(false);
            }
        }
        else if(_selectedOption == 9){
            if(score<15000){
                SkinLocked.gameObject.SetActive(true);
                SkinLockedText.text = "15000 points to Unlock";
            }
            else{
                SkinLocked.gameObject.SetActive(false);
            }
        }
        else{
            SkinLocked.gameObject.SetActive(false);
        }
    }

    public void SkinConfirm(){
        if(_selectedOption == 1){
            if(score<1000){
                _skinUnlocked = false;
            }
            else{
                _skinUnlocked = true;
            }
        }else if(_selectedOption == 2){
            if(score<2000){
                _skinUnlocked = false;
            }
            else{
                _skinUnlocked = true;
            }
        }else if(_selectedOption == 3){
            if(score<3000){
                _skinUnlocked = false;
            }
            else{
                _skinUnlocked = true;
            }
        }
        else if(_selectedOption == 4){
            if(score<4000){
                _skinUnlocked = false;
            }
            else{
                _skinUnlocked = true;
            }
        }
        else if(_selectedOption == 5){
            if(score<8000){
                _skinUnlocked = false;
            }
            else{
                _skinUnlocked = true;
            }
        }
        else if(_selectedOption == 6){
            if(score<10000){
                _skinUnlocked = false;
            }
            else{
                _skinUnlocked = true;
            }
        }
        else if(_selectedOption == 7){
            if(score<15000){
                _skinUnlocked = false;
            }
            else{
                _skinUnlocked = true;
            }
        }
        else if(_selectedOption == 8){
            if(score<15000){
                _skinUnlocked = false;
            }
            else{
                _skinUnlocked = true;
            }
        }
        else if(_selectedOption == 8){
            if(score<15000){
                _skinUnlocked = false;
            }
            else{
                _skinUnlocked = true;
            }
        }
        else{
            _skinUnlocked = true;
        }
        SaveSkin(_skinUnlocked);
    }

    private void LoadSkin(){
        _selectedOption = PlayerPrefs.GetInt(_skinSelected);
    }

    public void SaveSkin(bool _skinUnlocked){
        if(_skinUnlocked){
            PlayerPrefs.SetInt(_skinSelected,_selectedOption);
            SkinSelectedToPlayer.LoadSkin();
            _timer = 0;
        }     
    }
}
