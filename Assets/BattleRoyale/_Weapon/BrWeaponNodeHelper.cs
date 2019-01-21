using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrWeaponNodeHelper : MonoBehaviour {

    [Header("Display & Debug Settings")]
    public Mesh WeaponReference;

    public bool Show = true;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnDrawGizmos()
    {
        if (Show)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawMesh(WeaponReference, transform.position, transform.rotation, transform.localScale);

        }        
    }
}
