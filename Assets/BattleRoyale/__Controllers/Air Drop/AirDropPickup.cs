using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirDropPickup : MonoBehaviour
{
    [SerializeField]
    private float radious = 3;

    [SerializeField] private Color color=Color.red;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position,radious);
    }

}
