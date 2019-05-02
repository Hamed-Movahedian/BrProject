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

    #region OnPlayerSpawned
    
        public delegate void OnPlayerSpawnedDel(BrCharacterController player);
    
        public OnPlayerSpawnedDel OnPlayerSpawned;
    
        #endregion

    private void Awake()
    {
        BrGameManager.Instance.OnStart += () =>
        {
            BrCharacterController.MasterCharacter = null;

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
        if (SpawnUserPlayerInAdmin)
            SpawnUserPlayer();

        var locations = GameObject.FindGameObjectsWithTag("AiSpawnLocation").ToList();

        if (locations.Count == 0)
            Debug.LogAssertion("no ai location found!!!");

        SetupAiProfiles();

        for (var i = 0; i < Mathf.Min(AiProfiles.Count,locations.Count); i++)
        {
            var profile = AiProfiles[i];

            var location = locations.RandomSelection();

            locations.Remove(location);

            var playerGo = PhotonNetwork.Instantiate(this.playerPrefab.name, location.transform.position,
                Quaternion.identity, 0, new[] {profile.Serialize()});

            var aiCharacterController = playerGo.GetComponentInChildren<BrAiCharacterController>();

            // setup character
            var characterController = aiCharacterController.character;
            characterController.profile = profile;
            characterController.IsAi = true;

            if (BrCharacterController.MasterCharacter == null)
                BrCharacterController.MasterCharacter = characterController;

            // setup ai
            aiCharacterController.aiBehaviour = AiBehaviours.RandomSelection();
            
            OnPlayerSpawned?.Invoke(characterController);
        }
    }

    private void SetupAiProfiles()
    {
        // set random profiles for TEST
        for (int i = 0; i < AiProfiles.Count; i++)
        {
            AiProfiles[i].Name = $"AI";
            AiProfiles[i].ID = i;
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

            var go = PhotonNetwork.Instantiate(this.playerPrefab.name, position, Quaternion.identity, 0,
                new[] {PhotonNetwork.LocalPlayer.CustomProperties["Profile"]});

            var characterController = go.GetComponent<BrCharacterController>();
            
            BrCharacterController.MasterCharacter = characterController;
            
            OnPlayerSpawned?.Invoke(characterController);

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, .5f);
    }
}