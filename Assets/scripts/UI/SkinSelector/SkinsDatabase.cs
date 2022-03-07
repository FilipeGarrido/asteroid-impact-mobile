using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SkinsDatabase : ScriptableObject
{
    public Skins[] PlayerSkins;
    public int SkinCounter
    {
        get
        {
            return PlayerSkins.Length;
        }
    }
    public Skins GetSkins(int index)
    {
        return PlayerSkins[index];
    }
}
