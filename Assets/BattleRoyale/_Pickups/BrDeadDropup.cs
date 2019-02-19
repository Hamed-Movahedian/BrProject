using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

public class BrDeadDropup : MonoBehaviour
{
    public List<BrWeaponPickup> WeaponsPrefabs;
    public List<BrPickupBase> Prefabs;
    private BrCharacterController localPlayer;

    private void Awake()
    {
        BrPlayerTracker.Instance.OnMasterPlayerRegister += player =>
        {
            localPlayer = player;
            player.OnDead.AddListener(() =>
            {
                var pos = localPlayer.transform.position + Quaternion.Euler(0,90,0)*localPlayer.transform.forward;
                var pos2 = localPlayer.transform.position + Quaternion.Euler(0,-90,0)*localPlayer.transform.forward;
                
                var weapon = localPlayer.WeaponController.CurrWeapon;

                BrPickupBase pickup=null;
                
                if (weapon) pickup = WeaponsPrefabs.FirstOrDefault(w => w.WeaponName == weapon.gameObject.name);

                if (pickup == null)
                    pickup = Prefabs[Random.Range(0, Prefabs.Count)];
                
                BrPickupManager.Instance.AddPickup(
                    PhotonNetwork.Instantiate(pickup.name, pos, Quaternion.identity).GetComponent<BrPickupBase>());
                
                if (pickup == null)
                    pickup = Prefabs[Random.Range(0, Prefabs.Count)];
                
                BrPickupManager.Instance.AddPickup(
                    PhotonNetwork.Instantiate(pickup.name, pos2, Quaternion.identity).GetComponent<BrPickupBase>());
            });
        };
    }
}