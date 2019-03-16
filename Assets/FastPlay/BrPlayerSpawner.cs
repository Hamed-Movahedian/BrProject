using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

public class BrPlayerSpawner : MonoBehaviour
{
    #region Instance

    private static BrPlayerSpawner instance;

    public static BrPlayerSpawner Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<BrPlayerSpawner>();
            return instance;
        }
    }

    #endregion

    public bool SpawnUserPlayerInAdmin = false;
    
    public GameObject playerPrefab;

    public List<Profile> AiProfiles;

    public List<BrAiBehavioursAsset> AiBehaviours;


    private void Awake()
    {
        BrGameManager.Instance.OnStart += () =>
        {
            if (PhotonNetwork.LocalPlayer.CustomProperties["Admin"].ToString() == "1")
            {
                AdminSpawner();
            }
            else
            {
                SpawnUserPlayer();
            }
        };
    }

    private void AdminSpawner()
    {
        if(SpawnUserPlayerInAdmin)
            SpawnUserPlayer();
        
        var locations = GameObject.FindGameObjectsWithTag("AiSpawnLocation").ToList();

        if (locations.Count == 0)
            Debug.LogAssertion("no ai location found!!!");

        SetupAiProfiles();

        for (var i = 0; i < AiProfiles.Count; i++)
        {
            var profile = AiProfiles[i];

            var location = locations.RandomSelection();

            locations.Remove(location);

            var playerGo = PhotonNetwork.Instantiate(this.playerPrefab.name, location.transform.position,
                Quaternion.identity, 0,new []{profile.Serialize()});

            var aiCharacterController = playerGo.GetComponentInChildren<BrAiCharacterController>();

            // setup character
            var characterController = aiCharacterController.character;
            characterController.AiIndex = i;
            characterController.profile = profile;

            // setup ai
            aiCharacterController.aiBehaviour = AiBehaviours[profile.AiBehaviorIndex];
        }
    }

    private void SetupAiProfiles()
    {
        // set random profiles for TEST
        for (int i = 0; i < AiProfiles.Count; i++)
        {
            AiProfiles[i].UserID = $"AI-{i}";
            AiProfiles[i].CurrentCharacter =
                Random.Range(0, ProfileManager.Instance().CharactersList.Characters.Length);
            AiProfiles[i].CurrentFlag = Random.Range(0, ProfileManager.Instance().FlagsList.Flags.Length);
            AiProfiles[i].CurrentPara = Random.Range(0, ProfileManager.Instance().ParasList.Paras.Length);
        }
    }

    private void SpawnUserPlayer()
    {
        if (playerPrefab == null)
        {
            Debug.LogError(
                "<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'",
                this);
        }
        else
        {
            var position = JsonUtility.FromJson<Vector3>((string) PhotonNetwork.LocalPlayer.CustomProperties["Pos"]);

            position = BrLevelBound.Instance.GetLevelPos(position);

            PhotonNetwork.Instantiate(this.playerPrefab.name, position, Quaternion.identity, 0,
                new []{PhotonNetwork.LocalPlayer.CustomProperties["Profile"]});
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, .5f);
    }
}