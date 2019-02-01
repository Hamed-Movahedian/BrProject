using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrRotatorByInput : MonoBehaviour
{
    public float speed = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var axis = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up,axis*Time.deltaTime*speed );
    }
}
