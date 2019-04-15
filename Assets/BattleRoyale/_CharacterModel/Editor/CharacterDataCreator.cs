using System.IO;
using UnityEngine;
using UnityEditor;

public class CharacterDataCreator : ScriptableWizard
{
    [Header("Character Info")]
    public string Name;
    public Texture2D FaceIcon;
    public Texture2D BodyIcon;

    [Header("Character Data")]
    public CharacterData CharacterData;
    public BrCharacterModel Character;

    private CharacterData _data;

    private static BrCharacterModel _character;

    [Header("Character Body")]
    public GameObject Body;
    public Material OverrideBodyMaterial;

    private Mesh _bodySkinnedMesh;
    private Material _bodyMaterial;

    [Header("Head Attachment")]
    public GameObject HeadAttachment;
    public Material OverrideHeadMaterial;

    private Mesh _headMesh;
    private Material _headMaterial;

    [Header("Face Attachment")]
    public GameObject FaceAttachment;
    public Material OverrideFaceAttachmentMaterial;

    private Mesh _faceAttachmentMesh;
    private Material _faceAttachmentMaterial;

    [Header("Hip Attachment")]
    public GameObject HipAttachment;
    public Material OverrideHipAttachmentMaterial;

    private Mesh _hipAttachmentMesh;
    private Material _hipAttachmentMaterial;

    [MenuItem("Battle Royal/Create Character Data")]
    static void CreateWizard()
    {
        _character = FindObjectOfType<BrCharacterModel>();
        DisplayWizard<CharacterDataCreator>("Create Character Data", "Save", "Apply");
    }

    #region Create CharacterData Asset

    void OnWizardCreate()
    {
        CreatIcons();
        CharacterData character = CreateCharacter();


        string path = "Assets/BattleRoyale/_CharacterModel/Data/" + Name + ".asset";
        if (File.Exists(path))
        {
            if (!EditorUtility.DisplayDialog("CharacterData Over Write",
               "Are you sure you want to Over write " + Name
               + " ?", "Replace", "Do Not Replace")) return;

        }
        AssetDatabase.CreateAsset(character, path);
        Repaint();

    }
    private CharacterData CreateCharacter()
    {
        CharacterData characterData = new CharacterData
        {
            Name = Name,
            BodyIcon = BodyIcon,
            FaceIcon = FaceIcon,
            BodySkinnedMesh = Body,
            BodyMaterial = _bodyMaterial,
            HeadMesh = _headMesh,
            HeadMaterial = _headMaterial,
            FaceAttachmentMesh = _faceAttachmentMesh,
            FaceAttachmentMaterial = _faceAttachmentMaterial,
            HipAttachmentMesh = _hipAttachmentMesh,
            HipAttachmentMaterial = _hipAttachmentMaterial,
            HipPosisionOffset = HipAttachment != null ? Character.HipAttachmentMesh.transform.localPosition : Vector3.zero
        };
        return characterData;
    }

    #endregion

    void OnWizardOtherButton()
    {
        if (CharacterData)
        {
            Name = CharacterData.Name;
            FaceIcon = CharacterData.FaceIcon;
            BodyIcon = CharacterData.BodyIcon;
            CharacterData.SetToCharacter(_character);
            Body = CharacterData.BodySkinnedMesh;
            OverrideBodyMaterial = CharacterData.BodyMaterial;
            HeadAttachment = GetAsset(CharacterData.HeadMesh);
            OverrideHeadMaterial = CharacterData.HeadMaterial;
            FaceAttachment = GetAsset(CharacterData.FaceAttachmentMesh);
            OverrideFaceAttachmentMaterial = CharacterData.FaceAttachmentMaterial;
            if (CharacterData.HipAttachmentMesh != null)
            {
                HipAttachment = GetAsset(CharacterData.HipAttachmentMesh);
                OverrideHipAttachmentMaterial = CharacterData.HipAttachmentMaterial;
                HipAttachment.transform.localPosition = CharacterData.HipPosisionOffset;
            }
            else
            {
                HipAttachment = null;
                OverrideHipAttachmentMaterial = null;
            }

            CharacterData = null;
            _data = null;
        }
    }

    private static GameObject GetAsset(Object prefabFilter)
    {
        return AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GetAssetPath(prefabFilter.GetInstanceID()));
    }

    void OnWizardUpdate()
    {
        if (!Character)
            Character = _character;

        if (!Equals(Character, _character))
            _character = Character;

        if (CharacterData && CharacterData != _data)
        {
            _data = CharacterData;
            OnWizardOtherButton();
        }

        if (Body != null)
        {
            _bodyMaterial = OverrideBodyMaterial != null ? OverrideBodyMaterial : Body.GetComponentInChildren<Renderer>().sharedMaterial;
            _bodySkinnedMesh = Body.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
        }

        _character.HeadMesh.gameObject.SetActive(HeadAttachment != null);
        _character.FaceAttachmentMesh.gameObject.SetActive(FaceAttachment != null);
        _character.HipAttachmentMesh.gameObject.SetActive(HipAttachment != null);

        if (HeadAttachment != null)
        {
            _headMaterial = OverrideHeadMaterial != null ? OverrideHeadMaterial : HeadAttachment.GetComponent<Renderer>().sharedMaterial;
            OverrideHeadMaterial = _headMaterial;
            _headMesh = HeadAttachment.GetComponentInChildren<MeshFilter>().sharedMesh;
        }

        if (FaceAttachment != null)
        {
            _faceAttachmentMaterial = OverrideFaceAttachmentMaterial != null ? OverrideFaceAttachmentMaterial : FaceAttachment.GetComponent<Renderer>().sharedMaterial;
            OverrideFaceAttachmentMaterial = _faceAttachmentMaterial;
            _faceAttachmentMesh = FaceAttachment.GetComponentInChildren<MeshFilter>().sharedMesh;
        }

        if (HipAttachment != null)
        {
            _hipAttachmentMaterial = OverrideHipAttachmentMaterial != null ? OverrideHipAttachmentMaterial : HipAttachment.GetComponent<Renderer>().sharedMaterial;
            OverrideHipAttachmentMaterial = _hipAttachmentMaterial;
            _hipAttachmentMesh = HipAttachment.GetComponentInChildren<MeshFilter>().sharedMesh;
        }


        if (_character)
        {
            if (_bodySkinnedMesh != null) _character.BodySkinnedMesh.sharedMesh = _bodySkinnedMesh;
            if (_bodyMaterial != null) _character.BodySkinnedMesh.material = _bodyMaterial;

            if (_headMesh != null) _character.HeadMesh.mesh = _headMesh;
            if (_headMaterial != null) _character.HeadMaterial.material = _headMaterial;

            if (_faceAttachmentMesh != null) _character.FaceAttachmentMesh.mesh = _faceAttachmentMesh;
            if (_faceAttachmentMaterial != null) _character.FaceAttachmentMaterial.material = _faceAttachmentMaterial;

            if (_hipAttachmentMesh != null) _character.HipAttachmentMesh.mesh = _hipAttachmentMesh;
            if (_hipAttachmentMaterial != null) _character.HipAttachmentMaterial.material = _hipAttachmentMaterial;
        }
    }


    public void CreatIcons()
    {
        Camera faceCam = Character.transform.Find("FaceCam").GetComponent<Camera>();
        Camera bodyCam = Character.transform.Find("BodyCam").GetComponent<Camera>();
        RenderTexture face = faceCam.targetTexture;
        RenderTexture body = bodyCam.targetTexture;

        Texture2D facePhoto = new Texture2D(face.width, face.height, TextureFormat.ARGB32, false);
        Texture2D bodyPhoto = new Texture2D(body.width, body.height, TextureFormat.ARGB32, false);

        faceCam.Render();
        RenderTexture.active = face;
        facePhoto.ReadPixels(new Rect(0, 0, face.width, face.height), 0, 0);

        byte[] faceByte;
        faceByte = facePhoto.EncodeToPNG();
        string facePath = "Assets/BattleRoyale/_CharacterModel/Icons/" + Name + "_face.png";
        System.IO.File.WriteAllBytes(facePath, faceByte);

        bodyCam.Render();
        RenderTexture.active = body;
        bodyPhoto.ReadPixels(new Rect(0, 0, body.width, body.height), 0, 0);

        byte[] bodyByte;
        bodyByte = bodyPhoto.EncodeToPNG();
        string bodyPath = "Assets/BattleRoyale/_CharacterModel/Icons/" + Name + "_body.png";
        System.IO.File.WriteAllBytes(bodyPath, bodyByte);

    }

}