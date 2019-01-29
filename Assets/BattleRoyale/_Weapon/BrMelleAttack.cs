using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrMelleAttack : MonoBehaviour
{
    private BrWeaponController _weaponController;
    private bool isReady = true;
    private BrCharacterController _player;
    [SerializeField] private int Damage = 10;
    [SerializeField] private float Delay=0.2f;

    private void OnEnable()
    {
        _weaponController = GetComponentInParent<BrWeaponController>();
        _player = GetComponentInParent<BrCharacterController>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (_weaponController.Armed)
            return;

        if (!other.CompareTag("Player"))
            return;

        if (_player.AimVector.magnitude <= 0.1f)
            return;

        if (!isReady)
            return;

        var otherPlayer = other.GetComponent<BrCharacterController>();

        if (!otherPlayer)
            return;

        if (otherPlayer == _player)
            return;
        
        otherPlayer.TakeDamage(Damage, _player.transform.forward, _player, "Melee");
        isReady = false;
        Invoke(nameof(Ready),Delay);
    }

    private void Ready()
    {
        isReady=true;
    }
}