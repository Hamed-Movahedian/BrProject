using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveMeshes : MonoBehaviour
{
    public Transform Parent;

    [ContextMenu("Remove")]
    public void Remove()
    {
        foreach (MeshFilter filter in GetComponentsInChildren<MeshFilter>())
        {
            DestroyImmediate(filter.gameObject.GetComponent<MeshRenderer>());
            DestroyImmediate(filter);
        }
        foreach (BoxCollider boxCollider in GetComponentsInChildren<BoxCollider>())
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.parent = boxCollider.transform;
            cube.transform.localEulerAngles = Vector3.zero;
            cube.transform.localScale = boxCollider.size;
            cube.transform.localPosition = boxCollider.center;
            cube.transform.parent = Parent;


        }

        foreach (SphereCollider boxCollider in GetComponentsInChildren<SphereCollider>())
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            cube.transform.parent = boxCollider.transform;
            cube.transform.localEulerAngles = Vector3.zero;
            cube.transform.localScale = boxCollider.radius * Vector3.one;
            cube.transform.localPosition = boxCollider.center;
            cube.transform.parent = Parent;

        }
        foreach (CapsuleCollider boxCollider in GetComponentsInChildren<CapsuleCollider>())
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            cube.transform.parent = boxCollider.transform;
            cube.transform.localScale = new Vector3(boxCollider.radius,boxCollider.height ,boxCollider.radius);
            cube.transform.localEulerAngles = Vector3.zero;
            cube.transform.localPosition = boxCollider.center;
            cube.transform.parent = Parent;

        }


    }
}
