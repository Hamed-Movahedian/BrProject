using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrAirDropArrow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var masterCharacter = BrCharacterController.MasterCharacter;
        if (masterCharacter == null)
            return;

        var dir = BrAirdropController.Instance.DropPosition - masterCharacter.transform.position;
        
        transform.eulerAngles = new Vector3(0, 0,
            BrCamera.Instance.MainCamera.transform.eulerAngles.y - Quaternion.LookRotation(dir).eulerAngles.y
        );
    }
}