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
    private void Awake()
    {
        BrPlayerTracker.Instance.OnPlayerRegisterd += player =>
        {
            GameObjectDic[player.gameObject] = player;
            ColliderDic[player.CapsuleCollider] = player;
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
}