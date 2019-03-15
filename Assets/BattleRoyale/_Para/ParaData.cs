using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Para Data", menuName = "BattleRoyal/Para Data", order = 4)]
public class ParaData : ScriptableObject
{
    public string Name;
    public Texture2D Icon;
    public Texture2D Texture;
    public Mesh ParaModel;
    public Mesh RiserModel;

    [HideInInspector]
    public Vector3 ParaOffsetPos;

    public Sprite Sprite;

    public void SetToPara(Para para)
    {
        para.ParaModel.mesh = ParaModel;
        para.ParaModel.GetComponent<Renderer>().material.mainTexture = Texture;
        para.ParaModel.transform.localPosition = ParaOffsetPos;
        para.RiserModel.mesh = RiserModel;
    }
}
