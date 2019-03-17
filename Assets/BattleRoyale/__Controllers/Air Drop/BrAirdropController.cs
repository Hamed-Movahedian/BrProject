using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class BrAirdropController : MonoBehaviourPun
{
    #region Instance

    private static BrAirdropController _instance;

    public static BrAirdropController Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<BrAirdropController>();
            return _instance;
        }
    }

    #endregion


    public List<BrAirDropPlaceHolder> locations;
    public float Delay = 3;
    public PlayableDirector airDropDirector;
    public Transform airDropRoot;

    public List<BrPickupBase> pickup1Condidates;
    public List<BrPickupBase> pickup2Condidates;
    public List<BrPickupBase> pickup3Condidates;

    public UnityEvent OnNewAirDrop;
    public UnityEvent OnUnpackAirDrop;

    #region OnUnpack

    public delegate void OnUnpackDel(BrCharacterController player);

    public OnUnpackDel OnUnpack;

    #endregion

    private int activePlaceHolderIndex = -1;

    private List<int> innerIndexies = new List<int>();
    private List<int> outterIndexies = new List<int>();
    private System.Random random;

    private BrAirDropPlaceHolder activePlaceHolder => locations[activePlaceHolderIndex];
    public Vector3 DropPosition => airDropRoot.position;

    private void Start()
    {
        random = BrRandom.CreateNew();
        
        innerIndexies = Enumerable.Range(0, locations.Count).ToList();

        //New area
        BrKillZone.Instance.OnNewCircleExtented += (curr, target, delay, shrinkTime) =>
        {
            StartCoroutine(SelectPositionCoroutine(curr, target));
        };
    }

    private IEnumerator SelectPositionCoroutine(BrRing curr, BrRing target)
    {
        var startTime = Time.time;
        var endTime = startTime + Delay;
        
        outterIndexies.Clear();

        foreach (var i in innerIndexies)
        {
            if (!target.IsInside(locations[i].transform.position))
                outterIndexies.Add(i);

            if (Time.time - startTime > 0.005)
            {
                yield return null;
                startTime = Time.time;
            }
        }

        yield return null;
        
        outterIndexies.ForEach(i => innerIndexies.Remove(i));

        while (Time.time<endTime)
            yield return null;

        int selectedIndex = -1;

        if (outterIndexies.Count > 0)
            selectedIndex = outterIndexies.RandomSelection(random);
        else if (innerIndexies.Count > 0)
            selectedIndex = innerIndexies.RandomSelection(random);
        else
            yield break;
        
        if(PhotonNetwork.IsMasterClient)
            photonView.RPC(nameof(DropAirSupplyRpc), RpcTarget.All, selectedIndex);

    }

    [PunRPC]
    private void DropAirSupplyRpc(int posIndex)
    {
        activePlaceHolderIndex = posIndex;
        airDropRoot.position = locations[posIndex].transform.position;
        airDropDirector.Stop();
        airDropDirector.Play();
        OnNewAirDrop.Invoke();
    }

    public void Unpack(BrLocalPlayerTrigger playerTrigger)
    {
        photonView.RPC(nameof(UnpackRpc), RpcTarget.Others, playerTrigger.LastUserID);
        
        UnpackRpc(playerTrigger.LastUserID);
    }

    [PunRPC]
    private void UnpackRpc(string userID)
    {
        BrPickupManager.Instance.InstantiatePickup(
                pickup1Condidates.RandomSelection(random),
                activePlaceHolder.GetPickupPos(0));

        BrPickupManager.Instance.InstantiatePickup(
                pickup2Condidates.RandomSelection(random),
                activePlaceHolder.GetPickupPos(1));

        BrPickupManager.Instance.InstantiatePickup(
                pickup3Condidates.RandomSelection(random),
                activePlaceHolder.GetPickupPos(2));

        OnUnpackAirDrop.Invoke();
        
        OnUnpack(BrCharacterDictionary.Instance.GetCharacter(userID));
    }

}