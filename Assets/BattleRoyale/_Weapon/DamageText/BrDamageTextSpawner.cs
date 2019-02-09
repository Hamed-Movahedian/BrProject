using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BrDamageTextSpawner : MonoBehaviour
{
    public BrCharacterController characterController;
    public GameObject DamageTextPrefab;
    public float radious = 2;
    public List<Color> colors;

    private int damageAmount = 0;
    private int damageType = 0;
    // Start is called before the first frame update
    void Awake()
    {
        characterController.OnTakeDamage += (amount, type) =>
        {
            if (damageAmount == 0)
            {
                damageAmount = amount;
                damageType = type;
                Invoke(nameof(takeDamage), .5f);
            }
            else
            {
                damageAmount += amount;
            }
        };
    }

    private void takeDamage()
    {
        var pos = Random.insideUnitSphere * radious;
        pos.y = 0;
        var dt = BrPoolManager.Instance.Instantiate(DamageTextPrefab.name, transform.position + pos, Quaternion.identity)
            .GetComponent<BrDamageText>();
        dt.Initialize(damageAmount, colors[damageType]);
        damageAmount = 0;

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radious);
    }

   
}
