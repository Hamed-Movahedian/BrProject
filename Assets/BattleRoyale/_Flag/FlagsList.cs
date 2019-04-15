using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Flags List", menuName = "BattleRoyal/Flags List", order = 5)]
public class FlagsList : ScriptableObject
{
    public FlagData[] Flags;

    public FlagData this[int index]
    {
        get { return Flags[index]; }
        set { Flags[index] = value; }
    }
    
#if UNITY_EDITOR
    [ContextMenu("Sync")]
    void Sync()
    {
        var flagsJson = JArray.FromObject(
            Flags.Select(flag =>
                new
                {
                    flag.ID,
                    flag.Name,
                    flag.HasByDefault
                }
            )
        );

        BrServerController.Instance.PostEditor(
            "Flags/Sync",
            flagsJson.ToString(),
            output =>
            {
                var newIds = JsonConvert.DeserializeObject<List<int>>(
                    output);
                
                for (int i = 0; i < Flags.Length; i++)
                {
                    Flags[i].ID = newIds[i];
                    UnityEditor.EditorUtility.SetDirty(Flags[i]);

                }
                UnityEditor.EditorUtility.SetDirty(this);
                UnityEditor.AssetDatabase.SaveAssets();
            });
    }
#endif
}
