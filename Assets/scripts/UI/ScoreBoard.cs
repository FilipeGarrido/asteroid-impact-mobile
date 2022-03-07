using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] private UI _scorePoints;
    public TMP_InputField NameRecord;
    public TMP_Text ScoreBoardText;
    public TMP_Text ScoreBoardTitle;
    private string[] _nameToSave = new string[5];
    public int[] _scoreToSave = new int[5];
    private string _nameTemp;
    private static readonly string ScorePref = "ScorePref";
    private static readonly string NamePref = "NamePref";

    void Awake(){
        if(PlayerPrefs.HasKey(NamePref)){
            LoadScoreBoard();
        }
        else {
            for(int i=0;i<_nameToSave.Length;i++){
                if(_nameToSave[i] == null){
                    _nameToSave[i] = "Player Name " + (i+1).ToString();
                    _scoreToSave[i] = 00;
                }
                ScoreBoardText.text += _nameToSave[i] + " : " + _scoreToSave[i].ToString() + " \r\n";
            }
        }
        _scorePoints.Back();
    }
    public void CheckRecords()
    {
        for(int i=0 ; i<_scoreToSave.Length ; i++){
            if(_scoreToSave[i] <= _scorePoints.score){
                NameRecord.gameObject.SetActive(true);
                ScoreBoardTitle.text = "New Record! \r\n\r\nTotal Score:" + _scorePoints.score.ToString();
                i = _scoreToSave.Length;
            }
            else{
                ScoreBoardTitle.text = "Game Over \r\n\r\nTotal Score:" + _scorePoints.score.ToString();
            }
        }
        ShowScoreBoard();
    }
    public void UpdateName()
    {
        _nameTemp = NameRecord.text;
        UpdateScoreRecord(_nameTemp);
    }

    public void UpdateScoreRecord(string PlayerName)
    {
        for(int i=0 ; i<_scoreToSave.Length ; i++){
            if(_scoreToSave[i] <= _scorePoints.score){
                for(int j=_scoreToSave.Length-1; j>i ; j--){
                    _scoreToSave[j]=_scoreToSave[j-1];
                    _nameToSave[j]=_nameToSave[j-1];
                }
                _scoreToSave[i] = _scorePoints.score;
                _nameToSave[i] = PlayerName;
                NameRecord.gameObject.SetActive(false);
                i = _scoreToSave.Length;
            }
        }
        SaveScoreBoard();
    }

    public void ShowScoreBoard()
    {
        ScoreBoardText.text = "";
        for(int i=0;i<_nameToSave.Length;i++){
            ScoreBoardText.text += _nameToSave[i] + " : " + _scoreToSave[i].ToString() + " \r\n";
        }
    }

    void SaveScoreBoard()
    {
        PlayerPrefsX.SetIntArray(ScorePref,_scoreToSave);
        PlayerPrefsX.SetStringArray(NamePref,_nameToSave);
        ShowScoreBoard();
    }

    void LoadScoreBoard()
    {
        _scoreToSave = PlayerPrefsX.GetIntArray(ScorePref);
        _nameToSave = PlayerPrefsX.GetStringArray(NamePref);
        ShowScoreBoard();
    }
    
    
}
