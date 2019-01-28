using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrPlayerMiniMap : MonoBehaviour
{
    public GameObject Root;
    // Start is called before the first frame update
    void Start()
    {
        Root.SetActive(true);
        
        GetComponentInParent<BrCharacterController>().OnDead.AddListener((() =>
        {
            Root.SetActive(false);
        }));
    }
}
