using System.Collections;
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
    {
        get { return Characters[index]; }
        set { Characters[index] = value; }
    }

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
            output => { Debug.Log(output); });
    }
}