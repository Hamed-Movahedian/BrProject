using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

public class BrDeadDropup : MonoBehaviour
{
    public List<BrWeaponPickup> WeaponsPrefabs;
    public List<BrPickupBase> Prefabs;

    private System.Random random;

    private void Awake()
    {
        random = BrRandom.CreateNew();

        BrPlayerTracker.Instance.OnPlayerDead += (player, killer, weaponName) =>
        {
            Transform playerTransform = player.transform;

            var angle = random.Next(0, 360);
            
            var pos = playerTransform.position + Quaternion.Euler(0, angle, 0) * playerTransform.forward;
            var pos2 = playerTransform.position +
                       Quaternion.Euler(0, angle+random.Next(50, 310), 0) * playerTransform.forward;

            var weapon = player.WeaponController.CurrWeapon;

            BrPickupBase pickup = null;

            if (weapon) pickup = WeaponsPrefabs.FirstOrDefault(w => w.WeaponName == weapon.gameObject.name);

            if (pickup == null)
                pickup = Prefabs[random.Next(Prefabs.Count)];

            BrPickupManager.Instance.AddPickup(
                Instantiate(pickup, pos, Quaternion.identity));

            pickup = Prefabs[random.Next(0, Prefabs.Count)];

            BrPickupManager.Instance.AddPickup(
                Instantiate(pickup, pos2, Quaternion.identity));
        };
    }
}