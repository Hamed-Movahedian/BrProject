using Photon.Pun;
using UnityEngine;

public class BrBullet : MonoBehaviourPunCallbacks
{
    public float Speed;
    public LayerMask CollitionMask;
    public GameObject BulletModel;
    public GameObject HitFx;
    public float HitFixDuration = 1;

    public BrCharacterController OwnerCharacterController { get; set; }

    private float _range;
    private BrWeapon _weapon;

    private void Update()
    {
        if (!photonView.IsMine)
            return;

        if(Time.deltaTime<0)
            return;

        float moveDist= Speed * Time.deltaTime;

        var moveVector = transform.forward * moveDist;

        RaycastHit hitInfo;

        if (Physics.Raycast(transform.position, moveVector, out hitInfo, moveDist, CollitionMask))
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

            if(_range<=0)
                PhotonNetwork.Destroy(gameObject);
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
        photonView.RPC("HitEnviromentRpc",RpcTarget.Others);
    }
    [PunRPC]
    private void HitEnviromentRpc()
    {
        enabled = false;
        BulletModel.gameObject.SetActive(false);
        HitFx.gameObject.SetActive(true);
        Invoke("DestroyBullet",HitFixDuration);
    }

    private void HitPlayer(RaycastHit hitInfo)
    {
        var controller = hitInfo.collider.GetComponent<BrCharacterController>();
        if (controller)
            controller.TakeDamage(_weapon.BulletDamage, hitInfo.point, transform.forward);
        PhotonNetwork.Destroy(gameObject);
    }

    public void Initialize(BrWeapon weapon)
    {
        OwnerCharacterController = weapon.WeaponController.CharacterController;
        _weapon = weapon;
        _range = weapon.BulletRange;
    }

}