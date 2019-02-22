using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class BrPickupPlaceHolder : MonoBehaviour
{
    [Range(0,100)]
    public int Chance = 50;
    [SerializeField] private float Radious=0.6f;

    public List<BrPickupBase> Prefabs;
    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position+Vector3.up,"PickUp.png");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color=Color.green;
        Gizmos.DrawWireSphere(transform.position,Radious);
    }

    public void Evaluate(Random random)
    {
        gameObject.SetActive(false);
        
        if(random.Next(100)>Chance)
            return ;

        var prefab = Prefabs[random.Next(Prefabs.Count)];

        BrPickupManager.Instance.AddPickup(Instantiate(prefab, transform.position, transform.rotation));
        

    }
}
