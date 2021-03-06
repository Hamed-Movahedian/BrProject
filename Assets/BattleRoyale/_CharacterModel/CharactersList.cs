﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[CreateAssetMenu(fileName = "CharactersList", menuName = "BattleRoyal/CharactersList", order = 1)]
public class CharactersList : ScriptableObject
{
    public CharacterData[] Characters;

    public CharacterData this[int index] 
        => Characters.FirstOrDefault(c=>c.ID==index);

#if UNITY_EDITOR
    [ContextMenu("Sync")]
    void Sync()
    {
        var charactersJson = JArray.FromObject(
            Characters.Select(character =>
                new
                {
                    character.ID,
                    character.Name,
                    character.HasByDefault
                }
            )
        );

        BrServerController.Instance.PostEditor(
            "Characters/Sync",
            charactersJson.ToString(),
            output =>
            {
                var newIds = JsonConvert.DeserializeObject<List<int>>(
                    output);
                
                for (int i = 0; i < Characters.Length; i++)
                {
                    Characters[i].ID = newIds[i];
                    UnityEditor.EditorUtility.SetDirty(Characters[i]);

                }
                UnityEditor.EditorUtility.SetDirty(this);
                UnityEditor.AssetDatabase.SaveAssets();
            });
    }
 
#endif
    
}