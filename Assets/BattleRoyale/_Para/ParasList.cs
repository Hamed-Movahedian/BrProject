using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Paras List", menuName = "BattleRoyal/Paras List", order = 3)]
public class ParasList : ScriptableObject
{
    public ParaData[] Paras;
    
    public ParaData this[int index]
    {
        get { return Paras[index]; }
        set { Paras[index] = value; }
    }
#if UNITY_EDITOR
    [ContextMenu("Sync")]
    void Sync()
    {
        var paraJson = JArray.FromObject(
            Paras.Select(flag =>
                new
                {
                    flag.ID,
                    flag.Name,
                    flag.HasByDefault
                }
            )
        );

        BrServerController.Instance.PostEditor(
            "Paras/Sync",
            paraJson.ToString(),
            output =>
            {
                var newIds = JsonConvert.DeserializeObject<List<int>>(
                    output);
                
                for (int i = 0; i < Paras.Length; i++)
                {
                    Paras[i].ID = newIds[i];
                    UnityEditor.EditorUtility.SetDirty(Paras[i]);

                }
                UnityEditor.EditorUtility.SetDirty(this);
                UnityEditor.AssetDatabase.SaveAssets();
            });
    }
#endif
}
