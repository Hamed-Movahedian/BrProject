using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BR.Lobby;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BrZonePointer : MonoBehaviour
{
    [Multiline]
    public string ZoneName;
    public Text ZoneText;

    public UnityEvent Show, Hide;
    private List<BrZonePointer> _zones;
    private bool _active = false;
    void Start()
    {
        _zones= transform.parent.GetComponentsInChildren<BrZonePointer>().ToList();
        _zones.Remove(this);
        GetComponent<Button>().onClick.AddListener(() => Clicked());
        ZoneText.text = ZoneName;
    }

    private void Clicked()
    {
        _zones.ForEach(z=>z.UnClicked());
        if (_active)
            return;
        
        Show.Invoke();
        _active = true;
        LobbyManager.Instance.MarkerParent.GetComponent<Button>().onClick.Invoke();
    }

    private void UnClicked()
    {
        if(!_active) 
            return;
        
        Hide.Invoke();
        _active = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
