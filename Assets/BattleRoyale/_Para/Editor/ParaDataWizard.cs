using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ParaDataWizard : ScriptableWizard
{
    private static ParaData _data;
    private static Para _para;

    [Header("Para Info")]
    public string Name;
    public Texture2D Icon;

    [Header("Para Data")]
    public ParaData ParaData;
    public Para Para;

    [Header("Para Model")]
    public Mesh ParaMesh;
    public Mesh RiserMesh;
    public Texture2D ParaTexture;
    public Vector3 ParaLocalPos;



    [MenuItem("Battle Royal/Create Para Data")]
    static void CreateWizard()
    {
        _para = FindObjectOfType<Para>();
        DisplayWizard<ParaDataWizard>("Create Para Data", "Save", "Apply");
    }


    void OnWizardCreate()
    {
        CreatIcons();
        ParaData para = CreatePara();


        string path = "Assets/BattleRoyale/_Para/Data/" + Name + ".asset";
        if (File.Exists(path))
        {
            if (!EditorUtility.DisplayDialog("ParaData Over Write",
               "Are you sure you want to Over write " + Name
               + " ?", "Replace", "Do Not Replace")) return;

        }
        AssetDatabase.CreateAsset(para, path);
        Repaint();

    }
    private ParaData CreatePara()
    {
        ParaData paraData = new ParaData
        {
            Name = Name,
            ParaModel = ParaMesh,
            Texture = ParaTexture,
            ParaOffsetPos = Para.ParaModel.transform.localPosition,
            RiserModel = RiserMesh

        };
        return paraData;
    }
    

    void OnWizardOtherButton()
    {
        if (ParaData)
        {
            Name = ParaData.Name;
            ParaTexture = ParaData.Texture;
            ParaMesh = ParaData.ParaModel;
            RiserMesh = ParaData.RiserModel;
            Icon = ParaData.Icon;
            ParaLocalPos = ParaData.ParaOffsetPos;

            ParaData = null;
            _data = null;
        }
    }

    void OnWizardUpdate()
    {
        if (!Para)
            Para = _para;

        if (!Equals(Para, _para))
            _para = Para;

        if (ParaData && ParaData!=_data)
        {
            _data = ParaData;
            OnWizardOtherButton();
        }

        Para.ParaModel.mesh = ParaMesh;
        Para.ParaModel.transform.localPosition =ParaLocalPos;
        Para.ParaModel.GetComponent<Renderer>().material.mainTexture = ParaTexture;
        Para.RiserModel.mesh = RiserMesh;

    }


    public void CreatIcons()
    {
        Camera iconCam = Para.transform.Find("IconCam").GetComponent<Camera>();
        RenderTexture icon = iconCam.targetTexture;
        RenderTexture.active = icon;

        Texture2D iconPhoto = new Texture2D(icon.width, icon.height, TextureFormat.ARGB32, false);
        iconPhoto.ReadPixels(new Rect(0, 0, icon.width, icon.height), 0, 0);
        RenderTexture.active = null;

        //iconCam.Render();

        byte[] iconByte;
        iconByte = iconPhoto.EncodeToPNG();
        string iconPath = "Assets/BattleRoyale/_Para/Icons/" + Name + "_Icon.png";
        System.IO.File.WriteAllBytes(iconPath, iconByte);

    }

}
