using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BrPoolManager : MonoBehaviour, IPunPrefabPool
{
    internal static BrPoolManager insance;
    public List<GameObject> prefabList = new List<GameObject>();

    private Dictionary<string, Queue<GameObject>> pool = new Dictionary<string, Queue<GameObject>>();
    public void Awake()
    {
        PhotonNetwork.PrefabPool = this;
        insance = this;
    }

    public void Destroy(GameObject gameObject)
    {
        gameObject.SetActive(false);

        gameObject.transform.SetParent(transform);

        var prefabId = gameObject.name;

        if (!pool.ContainsKey(prefabId))
            pool[prefabId] = new Queue<GameObject>();

        pool[prefabId].Enqueue(gameObject);
    }

    public new GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
    {
        if (!pool.ContainsKey(prefabId))
            pool[prefabId] = new Queue<GameObject>();

        if (pool[prefabId].Count > 0)
        {
            GameObject go = pool[prefabId].Dequeue();

            go.transform.SetParent(null);

            go.transform.position = position;
            go.transform.rotation = rotation;
            go.SetActive(true);

            return go;
        }

        var prefab = prefabList.FirstOrDefault(p => p.name == prefabId);

        if (prefab == null)
        {
            Debug.LogError($"prefabId {prefabId} not found!!");
            return null;
        }

        GameObject newInstance = Instantiate(prefab, position, rotation);
        newInstance.name = prefabId;
        return newInstance;
    }

 
}
