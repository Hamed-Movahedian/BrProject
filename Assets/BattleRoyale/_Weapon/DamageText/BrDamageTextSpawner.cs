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
    // Start is called before the first frame update
    void Start()
    {
        characterController.OnTakeDamage += takeDamage;
    }

    private void takeDamage(int amount, int type)
    {
        var pos = Random.insideUnitSphere * radious;
        pos.y = 0;
        var dt = BrPoolManager.Instance.Instantiate(DamageTextPrefab.name, transform.position + pos, Quaternion.identity)
            .GetComponent<BrDamageText>();
        dt.Initialize(amount, colors[type]);

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radious);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
