using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrBushModule : MonoBehaviour
{
    public static readonly Dictionary<Collider,BrBushModule> Modules=new Dictionary<Collider, BrBushModule>();

    public GameObject ParticleSystem;
    public Material InBushMaterial;
    public float OnFireShowDuration = 2;
    public float OnHiShowDuration = 3;
    
    
    private bool isInitialized;
    private BrCharacterModel characterModel;
    private BrCharacterController characterController;
    private int counter;

    private float timeToShow = 0;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }
    void Awake()
    {
        Initialize();
    }

    void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (isInitialized)
            return;
        
        isInitialized = true;

        ParticleSystem.SetActive(false);

        var collider = GetComponentInParent<CapsuleCollider>();
        
        if(collider==null)
            Debug.LogError("Collider not found!!!");
        
        if(!Modules.ContainsKey(collider))
            Modules.Add(collider,this);

        characterModel = GetComponentInParent<BrCharacterModel>();
        characterController = GetComponentInParent<BrCharacterController>();
        characterController.GetComponent<BrCharacterHitEffect>().OnHit.AddListener(() => { ShowForSeconds(OnFireShowDuration); });
        characterController.GetComponent<BrWeaponController>().OnFire.AddListener(() => { ShowForSeconds(OnHiShowDuration); });
    }

    private void ShowForSeconds(float duration)
    {
        if (counter <= 0) 
            return;
        
        Show();
        timeToShow += duration;
    }

    private void Update()
    {
        if (timeToShow <= 0)
            return;

        timeToShow -= Time.deltaTime;
        if (timeToShow <= 0)
        {
            timeToShow = 0;
            if (counter > 0)
                Hide();
        }
    }

    public void Enter()
    {
        if (counter == 0)
        {
            Hide();
        }

        counter++;
    }

    public void Exit()
    {
        if (counter == 1)
        {
            Show();
        }

        counter--;
    }

    private void Hide()
    {
        ParticleSystem.SetActive(true);
        if (characterController.isMine)
            characterModel.SetTransparent(InBushMaterial);
        else
        {
            characterModel.Hide();
        }
    }

    private void Show()
    {
        ParticleSystem.SetActive(false);

        characterModel.Show();
    }
}
