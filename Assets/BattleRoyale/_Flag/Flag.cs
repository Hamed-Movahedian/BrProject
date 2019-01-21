using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    public FlagData Data;
    public FlagsList flagsList;
    public MeshFilter FlagMesh;

    private int _currentFlag;

    public bool IsOwner = true;

    [ContextMenu("Set Flag")]
    public void UpdateFlag()
    {
        Data.SetToFlag(this);
    }
}
