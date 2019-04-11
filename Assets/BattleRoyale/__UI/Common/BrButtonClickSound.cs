using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class BrButtonClickSound : MonoBehaviour
{
    public AudioClip ClickSound;

    public AudioSource audioSource;

    private void OnEnable()
    {
        if (!audioSource)
            audioSource = GameObject.FindWithTag("MenuAudioSource").GetComponent<AudioSource>();


        GetComponent<Button>().onClick.RemoveListener(Clicked);
        GetComponent<Button>().onClick.AddListener(Clicked);

    }

    public void Clicked()
    {
        audioSource.clip = ClickSound;

        audioSource.Play();
    }
}