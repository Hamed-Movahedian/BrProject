using UnityEngine;

public class BrAirDropPlaceHolder : MonoBehaviour
{
    [HideInInspector]
    public float Priority = 1;
    [SerializeField]
    private float radious = 3;

    [SerializeField] private Color color=Color.blue;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position,radious);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawIcon(transform.position+Vector3.up,"AirDrop.png",true);
    }

    public Vector3 GetPickupPos(int i) => transform.GetChild(i).position;
}