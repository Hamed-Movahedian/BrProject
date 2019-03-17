using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

public class BrCharacterDictionary : MonoBehaviour
{
    #region Instance

    private static BrCharacterDictionary _instance;

    public static BrCharacterDictionary Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<BrCharacterDictionary>();
            return _instance;
        }
    }

    #endregion

    private Dictionary<GameObject,BrCharacterController> GameObjectDic=
        new Dictionary<GameObject, BrCharacterController>();

    private Dictionary<Collider,BrCharacterController> ColliderDic=
        new Dictionary<Collider, BrCharacterController>();
    
    private Dictionary<string,BrCharacterController> UserIdDic=
        new Dictionary<string, BrCharacterController>();

    private Dictionary<int,BrCharacterController> ViewIdDic=
        new Dictionary<int, BrCharacterController>();

    private void Awake()
    {
        BrPlayerTracker.Instance.OnPlayerRegisterd += player =>
        {
            GameObjectDic[player.gameObject] = player;
            ColliderDic[player.CapsuleCollider] = player;
            UserIdDic[player.UserID] = player;
            ViewIdDic[player.ViewID] = player;
        };
    }

    public BrCharacterController GetCharacter(Collider collider)
    {
        if (ColliderDic.TryGetValue(collider, out var characterController))
            return characterController;
        
        return null;
    }

    public BrCharacterController GetCharacter(GameObject go)
    {
        if (GameObjectDic.TryGetValue(go, out var characterController))
            return characterController;
        
        return null;

    }

    public BrCharacterController GetCharacter(string userID)
    {
        if (UserIdDic.TryGetValue(userID, out var characterController))
            return characterController;
        
        return null;
    }

    public BrCharacterController GetCharacter(int viewID)
    {
        if (ViewIdDic.TryGetValue(viewID, out var characterController))
            return characterController;
        
        return null;
    }
}