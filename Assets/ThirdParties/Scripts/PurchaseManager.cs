#if UNITY_ANDROID
using System.Collections;
using System.Collections.Generic;
using BazaarPlugin;
#endif
using UnityEngine;
using UnityEngine.UI;

public class PurchaseManager : MonoBehaviour
{
    public Text Debuger;
    void Start()
    {

#if UNITY_ANDROID
        BazaarIAB.init("MIHNMA0GCSqGSIb3DQEBAQUAA4G7ADCBtwKBrwDd+IlpeNJRf25jGcQxyNnaM2F7ieP5q6yt3PGnizCrzWIbHoLmquM3ZQbPWXSYjpYEJ2dbIHROEvZHD4+oQOEm2skVGCRpbq2nFZm9p1QKstTwbYnWI1bRmeRil/G56VrMt1CvmN3pmgQ/CW6N4aZJawbn/58M+2c0Fwz9AUIECYhB6GHrmqtmBVq2u8zVE62hjK/Ri1QH6bSspvl73feIfDA+yQ4NWV1gwNT9KXECAwEAAQ==");

        string[] items = new string[] {"dadad","adsf" };
        BazaarIAB.querySkuDetails(items);
        Debuger.text = "";
        foreach (string item in items)
            Debuger.text += item + "\n";
#endif
    }

    // Update is called once per frame
    void Update()
    {

    }
}
