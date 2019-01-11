#if Unitu_Android
using TapsellSDK;
#endif
using UnityEngine;

public class TapsellInitializer : MonoBehaviour
{
    public AdVideoManager AdVideoManager;
    void Start()
    {
#if Unitu_Android
        Tapsell.initialize("rjknmfmqhdfrbhedsroorjaoacnesmflnjrjreijkabfgppehssoacqqgrbdpdtsifhqet");
        AdVideoManager.gameObject.SetActive(true);
#endif
    }
}
