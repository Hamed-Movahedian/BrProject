using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Flag Data", menuName = "BattleRoyal/Flag Data", order = 6)]
public class FlagData : ScriptableObject
{
    public string Name;
    public Texture2D Icon;
    public Texture2D Image;

    public void SetToFlag(Flag flag)
    {
        WavySprite sprite = flag.FlagMesh.GetComponent<WavySprite>();

        if (sprite)
            sprite.texture = Image;
        else
            flag.gameObject.GetComponent<MeshRenderer>().material.mainTexture = Image;
    }
}
