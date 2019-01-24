using Photon.Pun;
using UnityEngine;

public class BrBullet : MonoBehaviour
{
    public float Speed;
    public LayerMask CollitionMask;
    public GameObject BulletModel;
    public GameObject HitFx;
    public float HitFixDuration = 1;

    public BrCharacterController OwnerCharacterController { get; set; }
    public bool IsMine => OwnerCharacterController.isMine;

    private float _range;
    private BrWeapon _weapon;

    private void Update()
    {
        if (Time.deltaTime <= 0)
            return;

        float moveDist = Speed * Time.deltaTime;

        var moveVector = transform.forward * moveDist;

        
        RaycastHit hitInfo;

        if (IsMine && Physics.Raycast(transform.position, moveVector, out hitInfo, moveDist, CollitionMask))
        {
            transform.position = hitInfo.point;

            if (hitInfo.collider.tag == "Player")
                HitPlayer(hitInfo);
            else
                HitEnviroment();
        }
        else
        {
            transform.position += moveVector;

            _range -= moveDist;

            if (_range <= 0)
            {
                enabled = true;

                BrPoolManager.Instance.Destroy(gameObject);
            }
        }
    }

    private void DestroyBullet()
    {
        enabled = true;

        BrPoolManager.Instance.Destroy(gameObject);
    }

    private void HitEnviroment()
    {
        if (!enabled)
            return;

        enabled = false;
        
        BulletModel.gameObject.SetActive(false);
        HitFx.gameObject.SetActive(true);
        Invoke("DestroyBullet", HitFixDuration);
    }   

    private void HitPlayer(RaycastHit hitInfo)
    {
        if (!enabled)
            return;

        enabled = false;

        if (IsMine)
        {
            var controller = hitInfo.collider.GetComponent<BrCharacterController>();

            if (controller && controller != OwnerCharacterController)
                controller.TakeDamage(_weapon.BulletDamage, transform.forward, OwnerCharacterController, _weapon.name); 
        }

        DestroyBullet();
    }

    public void Initialize(BrWeapon weapon)
    {
        enabled = true;
        BulletModel.gameObject.SetActive(true);
        HitFx.gameObject.SetActive(false);
        OwnerCharacterController = weapon.WeaponController.CharacterController;
        _weapon = weapon;
        _range = weapon.BulletRange;
    }

}