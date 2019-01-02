using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "BattleRoyal/CharacterData", order = 2)]
public class CharacterData : ScriptableObject
{
    public string Name;
    public Texture2D FaceIcon;
    public Texture2D BodyIcon;


    [Header("Character Body")]
    public GameObject BodySkinnedMesh;
    public Material BodyMaterial;

    [Header("Head Attachment")]
    public Mesh HeadMesh;
    public Material HeadMaterial;

    [Header("Face Attachment")]
    public Mesh FaceAttachmentMesh;
    public Material FaceAttachmentMaterial;

    [Header("Hip Attachment")]
    public Mesh HipAttachmentMesh;
    public Material HipAttachmentMaterial;
    public Vector3 HipPosisionOffset;

    public void SetToCharacter(BrCharacterModel character)
    {
        character.BodySkinnedMesh.sharedMesh = BodySkinnedMesh.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
        character.BodySkinnedMesh.material = BodyMaterial;

        character.HeadMesh.gameObject.SetActive(HeadMesh);
        character.HeadMesh.mesh = HeadMesh;
        character.HeadMaterial.material = HeadMaterial;

        character.FaceAttachmentMesh.gameObject.SetActive(FaceAttachmentMesh);
        character.FaceAttachmentMesh.mesh = FaceAttachmentMesh;
        character.FaceAttachmentMaterial.material = FaceAttachmentMaterial;

        character.HipAttachmentMesh.gameObject.SetActive(HipAttachmentMesh);
        character.HipAttachmentMesh.mesh = HipAttachmentMesh;
        character.HipAttachmentMaterial.material = HipAttachmentMaterial;
        character.HipAttachmentMesh.transform.localPosition = HipPosisionOffset;
    }
}
