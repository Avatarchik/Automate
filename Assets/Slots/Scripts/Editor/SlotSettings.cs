using UnityEngine;
using UnityEditor;
using UnitySlot;

public class SlotSettings : EditorWindow {
    private string compName = "";
    private string prodName = "";
    private string androidPackage = "";
    private string iOSBundle = "";
    private int screenWidth = 640;
    private int screenHeight = 480;
    private bool fullScreen;
    private SlotVariables slotVariables;
    private string defaultReferrer;
    private bool settingsLoaded = false;

    [MenuItem("Slot/Settings")]
    static void Init () {
        SlotSettings window = (SlotSettings)EditorWindow.GetWindow (typeof(SlotSettings));
        window.Show ();
    }

    void OnGUI () {
        if (!settingsLoaded) {
            LoadSettings ();
        }

        compName = EditorGUILayout.TextField ("Company Name:", compName);
        prodName = EditorGUILayout.TextField ("Product Name:", prodName);
        androidPackage = EditorGUILayout.TextField ("Android Package:", androidPackage);
        iOSBundle = EditorGUILayout.TextField ("iOS Bundle:", iOSBundle);
        EditorGUILayout.Space ();
        screenWidth = EditorGUILayout.IntField ("Width:", screenWidth);
        screenHeight = EditorGUILayout.IntField ("Height:", screenHeight);
        fullScreen = EditorGUILayout.Toggle ("Full Screen:", fullScreen);
        EditorGUILayout.Space ();
        slotVariables.DefaultReferrer = EditorGUILayout.TextField ("Default referrer:", slotVariables.DefaultReferrer);
        EditorGUILayout.Space ();
        if(GUILayout.Button ("Save settings")) {
            SaveSettings();
        }
        EditorGUILayout.Space ();
        EditorGUILayout.Space ();
    }

    void SaveSettings () {
        PlayerSettings.companyName = compName;
        PlayerSettings.productName = prodName;
        PlayerSettings.bundleIdentifier = androidPackage;
        PlayerSettings.iPhoneBundleIdentifier = iOSBundle;
        PlayerSettings.defaultScreenWidth = screenWidth;
        PlayerSettings.defaultScreenHeight = screenHeight;
        PlayerSettings.defaultIsFullScreen = fullScreen;

        EditorPrefs.SetString ("CompName", compName);
        EditorPrefs.SetString ("ProdName", prodName);
        EditorPrefs.SetString ("AndroidPackage", androidPackage);
        EditorPrefs.SetString ("iOSBundle", iOSBundle);
        EditorPrefs.SetInt ("ScreenWidth", screenWidth);
        EditorPrefs.SetInt ("ScreenHeight", screenHeight);
        EditorPrefs.SetBool ("FullScreen", fullScreen);
        SaveSlotVariables ();
    }

    void LoadSettings () {
        compName = EditorPrefs.GetString ("CompName", "");
        prodName = EditorPrefs.GetString ("ProdName", "");
        androidPackage = EditorPrefs.GetString ("AndroidPackage", "");
        iOSBundle = EditorPrefs.GetString ("iOSBundle", "");
        screenWidth = EditorPrefs.GetInt ("ScreenWidth", 640);
        screenHeight = EditorPrefs.GetInt ("ScreenHeight", 480);
        fullScreen = EditorPrefs.GetBool ("FullScreen", fullScreen);
        ReadSlotVariables ();
        settingsLoaded = true;
    }

    void ReadSlotVariables () {
        slotVariables = XmlUtil.Deserialize<SlotVariables> ("variables");
        if (slotVariables == null) {
            slotVariables = new SlotVariables ();
        }
        Debug.Log (slotVariables.ToString ());
    }

    void SaveSlotVariables () {
        Debug.Log(string.Format("Save new default referrer [{0}]", slotVariables.DefaultReferrer));
        XmlUtil.Serialize<SlotVariables> ("Assets/Resources/variables.xml", slotVariables);
    }

}