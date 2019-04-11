using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ServerController", menuName = "BattleRoyal/Server Controller")]
public class BrServerController : ScriptableObject
{
    #region Instance

    private static BrServerController instance;

    public static BrServerController Instance
    {
        get
        {
            if (instance == null)
                instance = Resources.Load<BrServerController>("ServerController");
            return instance;
        }
    }

    #endregion

    public string URL=@"http://localhost:3794";

    [ContextMenu("Test")]
    void Test()
    {
        Debug.Log(Instance.URL);
    }
}