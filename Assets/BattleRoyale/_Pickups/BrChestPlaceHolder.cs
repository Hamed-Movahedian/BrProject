using System.Collections.Generic;
using UnityEngine;

public class BrChestPlaceHolder : MonoBehaviour
{
    public List<BrPickupBase> PrefabList;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, .6f);
    }

    public void Replace()
    {
        var random = BrPickupManager.Instance.random;

        var prefab= PrefabList[random.Next(PrefabList.Count)];

        var pickup = Instantiate(prefab, transform.position, transform.rotation);
        
        BrPickupManager.Instance.AddPickup(pickup);
    }
}