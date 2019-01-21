using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrLevelBound : MonoBehaviour
{
    #region Instance

    private static BrLevelBound _instance;

    public static BrLevelBound Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<BrLevelBound>();
            return _instance;
        }
    }

    #endregion

    public Bounds levelBound = new Bounds();

    public Vector3 Center => levelBound.center;

    private void OnDrawGizmos()
    {
        Gizmos.color=Color.green;
        Gizmos.DrawWireCube(levelBound.center,levelBound.extents);
    }

    internal Vector3 GetLevelPos(Vector3 pos)
    {
        return new Vector3(
            levelBound.center.x + levelBound.extents.x * pos.x,
            0,
            levelBound.center.z + levelBound.extents.z * pos.y
            );
    }

    public Vector3 NormalizePos(Vector3 pos)
    {
        return new Vector3(
            2*(pos.x - levelBound.center.x )/ levelBound.extents.x,
            2*(pos.z - levelBound.center.z )/ levelBound.extents.z,
            0
            );
    }
}
