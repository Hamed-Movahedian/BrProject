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
    public Renderer HeadMaterial
    {
        get { return HeadMesh.GetComponent<Renderer>(); }
    }

    [Header("Face Attachment")]
    public MeshFilter FaceAttachmentMesh;
    public Renderer FaceAttachmentMaterial
    {
        get { return FaceAttachmentMesh.GetComponent<Renderer>(); }
    }

    [Header("Hip Attachment")]
    public MeshFilter HipAttachmentMesh;

    private Profile _profile;

    public Renderer HipAttachmentMaterial
    {
        get { return HipAttachmentMesh.GetComponent<Renderer>(); }
    }

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
}

