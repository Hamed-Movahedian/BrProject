using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class FlagDataWizard : ScriptableWizard
{
    private static FlagData _data;
    private static Flag _flag;

    private static Flag Flag;

    public static FlagData FlagData;


    public string Name;
    public Texture2D Icon;
    public Texture2D FlagImage;


    [MenuItem("Battle Royal/Create Flag Data")]
    static void CreateWizard()
    {
        _flag = FindObjectOfType<Flag>();
        DisplayWizard<FlagDataWizard>("Create Flag Data", "Save", "Apply");
    }


    void OnWizardCreate()
    {
        CreatIcons();
        FlagData flag= CreateFlag();


        string path = "Assets/Player/FlagsData/" + Name + ".asset";
        if (File.Exists(path))
        {
            if (!EditorUtility.DisplayDialog("FlagData Over Write",
               "Are you sure you want to Over write " + Name
               + " ?", "Replace", "Do Not Replace")) return;

        }
        AssetDatabase.CreateAsset(flag, path);
        AssetDatabase.SaveAssets();
        Repaint();

    }
    private FlagData CreateFlag()
    {
        FlagData flagData = new FlagData()
        {
            Name = Name,
            Icon = Icon,
            Image = FlagImage

        };
        return flagData;
    }


    void OnWizardOtherButton()
    {
        if (FlagData)
        {
            Name = FlagData.Name;
            FlagData.Icon = Icon;
            FlagData.Image = FlagImage;

            FlagData = null;
            _data = null;
        }
    }

    void OnWizardUpdate()
    {
        if (!Flag)
            Flag = _flag;

        if (!Equals(Flag, _flag))
            _flag = Flag;

        if (FlagData && FlagData != _data)
        {
            _data = FlagData;
            OnWizardOtherButton();
        }

        Flag.FlagMesh.GetComponent<Renderer>().sharedMaterial.mainTexture= FlagImage;
    }


    public void CreatIcons()
    {
        Camera iconCam = Flag.transform.Find("IconCam").GetComponent<Camera>();
        RenderTexture icon = iconCam.targetTexture;

        Texture2D iconPhoto = new Texture2D(icon.width, icon.height, TextureFormat.ARGB32, false);

        iconCam.Render();
        RenderTexture.active = icon;
        iconPhoto.ReadPixels(new Rect(0, 0, icon.width, icon.height), 0, 0);

        byte[] iconByte;
        iconByte = iconPhoto.EncodeToPNG();
        string iconPath = "Assets/Player/FlagsData/Icons/" + Name + "_Icon.png";
        System.IO.File.WriteAllBytes(iconPath, iconByte);

    }


}
