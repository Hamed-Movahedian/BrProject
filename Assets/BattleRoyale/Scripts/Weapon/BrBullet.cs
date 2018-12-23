using UnityEngine;

public class BrBullet : MonoBehaviour
{
    public float Speed;
    public LayerMask CollitionMask;
    public GameObject BulletModel;
    public GameObject HitFx;
    public float HitFixDuration = 1;

    public BrCharacterController OwnerCharacterController { get; set; }

    private float _range;

    private void Update()
    {
        if(Time.deltaTime<0)
            return;

        float moveDist= Speed * Time.deltaTime;

        var moveVector = transform.forward * moveDist;

        RaycastHit hitInfo;

        if (Physics.Raycast(transform.position, moveVector, out hitInfo, moveDist, CollitionMask))
        {
            transform.position = hitInfo.point;

            if (hitInfo.collider.tag == "Player")
                HitPlayer();
            else
                HitEnviroment();
        }
        else
        {
            transform.position += moveVector;

            _range -= moveDist;

            if(_range<=0)
                DestroyBullet();
        }
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }

    private void HitEnviroment()
    {
        enabled = false;
        BulletModel.gameObject.SetActive(false);
        HitFx.gameObject.SetActive(true);
        Invoke("DestroyBullet",HitFixDuration);
    }

    private void HitPlayer()
    {
        
    }

    public void Initialize(BrWeapon weapon)
    {
        OwnerCharacterController = weapon.WeaponController.CharacterController;
        _range = weapon.BulletRange;
    }
}