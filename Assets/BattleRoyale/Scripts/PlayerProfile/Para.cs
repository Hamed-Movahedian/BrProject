using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Para : MonoBehaviour
{
    private int _currentPara;

    public bool IsOwner = true;
    public ParaData Data;

    public Texture2D Texture;
    public MeshFilter ParaModel;
    public MeshFilter RiserModel;

    [ContextMenu("Set Para")]
    public void UpdatePara()
    {
        Data.SetToPara(this);
    }

}
