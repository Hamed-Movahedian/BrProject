using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BrKillReport : MonoBehaviour
{
    public List<PlayableDirector> Directors; 
    // Start is called before the first frame update
    void Start()
    {
        Directors.ForEach(d=>d.gameObject.SetActive(false));

        BrPlayerTracker.Instance.OnPlayerDead += (victim, killer, weaponName) =>
        {
            if (!killer || !killer.IsMaster) return;
            
            for (int i = 0; i < Directors.Count; i++)
            {
                if (!Directors[i].gameObject.activeSelf)
                {
                    Directors[i].gameObject.SetActive(true);
                    Directors[i].Play();
                    return;
                }
            }
        };
    }

    
}
