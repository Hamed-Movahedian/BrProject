using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class BrCharacterModel : MonoBehaviour
{
    private int _currentCharacter;

    public bool IsOwner = true;
    public CharacterData Data;
    public CharactersList CharactersList;
    
    [Header("Character Body")]
    public SkinnedMeshRenderer BodySkinnedMesh;

    [Header("Head Attachment")]
    public MeshFilter HeadMesh;
    public Renderer HeadMaterial => HeadMesh.GetComponent<Renderer>();

    [Header("Face Attachment")]
    public MeshFilter FaceAttachmentMesh;
    public Renderer FaceAttachmentMaterial => FaceAttachmentMesh.GetComponent<Renderer>();

    [Header("Hip Attachment")]
    public MeshFilter HipAttachmentMesh;

    private Profile _profile;

    public Renderer HipAttachmentMaterial => HipAttachmentMesh.GetComponent<Renderer>();

    [ContextMenu("Set Character")]
    public void UpdateCharacter()
    {
        Data.SetToCharacter(this);
    }

    public Para para;
    public void SetProfile(Profile profile)
    {
        
        CharactersList.Characters[profile.CurrentCharacter].SetToCharacter(this);
        para.parasList.Paras[profile.CurrentPara].SetToPara(para);
        _profile = profile;
    }

    public void Hide()
    {
        BodySkinnedMesh.gameObject.SetActive(false);
        HeadMesh.gameObject.SetActive(false);
        FaceAttachmentMesh.gameObject.SetActive(false);
    }
    public void Show()
    {
        BodySkinnedMesh.gameObject.SetActive(true);
        CharactersList.Characters[_profile.CurrentCharacter].SetToCharacter(this);
    }

    public void SetTransparent(Material material)
    {
        
        BodySkinnedMesh.material = material;
        HeadMaterial.material = material;
        FaceAttachmentMaterial.material = material;

    }
}

