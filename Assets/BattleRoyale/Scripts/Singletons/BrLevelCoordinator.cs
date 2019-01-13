using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrLevelCoordinator : MonoBehaviour
{
    public Bounds levelBound=new Bounds();

    public static BrLevelCoordinator instance;
    public Vector3 Center => levelBound.center;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
		
	}

    private void OnDrawGizmos()
    {
        Gizmos.color=Color.green;
        Gizmos.DrawWireCube(levelBound.center,levelBound.extents);
    }

    // Update is called once per frame
    void Update () {
		
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
