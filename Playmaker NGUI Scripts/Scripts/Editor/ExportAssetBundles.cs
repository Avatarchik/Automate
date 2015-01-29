using UnityEngine;
using UnityEditor;

/*
 * *************************************************************************************
 * Created by: Rocket Games Mobile  (http://www.rocketgamesmobile.com), 2013
 * For use in Unity 3.5, Unity 4.0+
 * *************************************************************************************
*/
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

public class ExportAssetBundles {

#region PC
   #if UNITY_STANDALONE_WIN
    
    [MenuItem("Assets/Asset Bundles/PC - Build AssetBundle From Selection - With dependencies")]
    static void ExportResource_PC() {
        ExportWithDependencies(BuildTarget.StandaloneWindows);
    }

    [MenuItem("Assets/Asset Bundles/PC - Build AssetBundle From Selection - No dependency tracking")]
    static void ExportResourceNoTrack_PC() {
        ExportNoDependencies(BuildTarget.StandaloneWindows);
    }

    #endif
    // PC
#endregion


#region WePlayer
    #if UNITY_WEBPLAYER
    
    [MenuItem("Assets/Asset Bundles/WebPlayer - Build AssetBundle From Selection - With dependencies")]
    static void ExportResourceWeb() {
        ExportWithDependencies(BuildTarget.WebPlayer);
    }

    [MenuItem("Assets/Asset Bundles/WebPlayer - Build AssetBundle From Selection - No dependency tracking")]
    static void ExportResourceNoTrackWeb() {
        ExportNoDependencies(BuildTarget.WebPlayer);
    }

    #endif
// WePlayer
#endregion

#region iOS
    #if UNITY_IPHONE

        [MenuItem ("Assets/Asset Bundles/iOS - Build AssetBundle From Selection - With dependencies")]
        static void ExportResource_iOS () {
                ExportWithDependencies (BuildTarget.iPhone);
        }

        [MenuItem ("Assets/Asset Bundles/iOS - Build AssetBundle From Selection - No dependency tracking")]
        static void ExportResourceNoTrack_iOS () {
                ExportNoDependencies (BuildTarget.iPhone);
        }

        // C# Example
        // Builds an asset bundle from the selected objects in the project view.
        // Once compiled go to "Menu" -> "Assets" and select one of the choices
        // to build the Asset Bundle

        [MenuItem ("Assets/Asset Bundles/Build all slots")]
        static void ExportSlots () { 
//        List<string> Levels = new List<string> ();
                EditorBuildSettingsScene[] Scenes = EditorBuildSettings.scenes;

                foreach (EditorBuildSettingsScene Scene in Scenes) {

                        if (!Scene.enabled) {
//                Levels.Add (Scene.path);
                                var sceneName = Path.GetFileNameWithoutExtension (Scene.path);
                                var bname = "Bundles/ios/" + sceneName + ".bundle";
                                Debug.Log ("Building slot " + bname);

                                BuildPipeline.BuildStreamedSceneAssetBundle (new [] { Scene.path }, 
                                        bname, EditorUserBuildSettings.activeBuildTarget, BuildOptions.UncompressedAssetBundle);

                                var fsi = File.OpenRead (bname);
                                var fso = File.Create (bname + ".gz");

                                using(GZipStream output = new GZipStream(fso, CompressionMode.Compress)) {
                                        byte[] bytes = new byte[4096];
                                        int n;
                                        while((n = fsi.Read(bytes, 0, bytes.Length)) != 0) {
                                                output.Write(bytes, 0, n);
                                        }
                                }

                                fsi.Close ();
                                fso.Close ();
                        }
                }
        }

    #endif
    // iOS
#endregion

#region Android
    #if UNITY_ANDROID
    
    [MenuItem("Assets/Asset Bundles/Android - Build AssetBundle From Selection - With dependencies")]
    static void ExportResource_Android() {
        ExportWithDependencies(BuildTarget.Android);
    }

    [MenuItem("Assets/Asset Bundles/Android - Build AssetBundle From Selection - No dependency tracking")]
    static void ExportResourceNoTrack_Android() {
        ExportNoDependencies(BuildTarget.Android);
    }

    // C# Example
    // Builds an asset bundle from the selected objects in the project view.
    // Once compiled go to "Menu" -> "Assets" and select one of the choices
    // to build the Asset Bundle
    
    [MenuItem ("Assets/Asset Bundles/Build all slots")]
    static void ExportSlots () { 
        //        List<string> Levels = new List<string> ();
        EditorBuildSettingsScene[] Scenes = EditorBuildSettings.scenes;
        
        foreach (EditorBuildSettingsScene Scene in Scenes) {
            
            if (!Scene.enabled) {
                //                Levels.Add (Scene.path);
                var sceneName = Path.GetFileNameWithoutExtension (Scene.path);
                var bname = "Bundles/android/" + sceneName + ".bundle";
                Debug.Log ("Building slot " + bname);
                
                BuildPipeline.BuildStreamedSceneAssetBundle (new [] { Scene.path }, 
                bname, EditorUserBuildSettings.activeBuildTarget, BuildOptions.UncompressedAssetBundle);
                
                var fsi = File.OpenRead (bname);
                var fso = File.Create (bname + ".gz");
                
                using(GZipStream output = new GZipStream(fso, CompressionMode.Compress)) {
                    byte[] bytes = new byte[4096];
                    int n;
                    while((n = fsi.Read(bytes, 0, bytes.Length)) != 0) {
                        output.Write(bytes, 0, n);
                    }
                }
                
                fsi.Close ();
                fso.Close ();
            }
        }
    }

    #endif

    // Android
#endregion

#region Blackberry
    #if UNITY_BLACKBERRY
    
    [MenuItem("Assets/Asset Bundles/Blackberry - Build AssetBundle From Selection - With dependencies")]
    static void ExportResource_Blackberry() {
        ExportWithDependencies(BuildTarget.BB10);
    }

    [MenuItem("Assets/Asset Bundles/Blackberry - Build AssetBundle From Selection - No dependency tracking")]
    static void ExportResourceNoTrack_Blackberry() {
        ExportNoDependencies(BuildTarget.BB10);
    }

    #endif
// Blackberry
#endregion

#region Win8
    // for now, Windows 8 / Metro falls under PC
    // Win8
#endregion

#region ExportWithDependencies

    private static void ExportWithDependencies (BuildTarget buildTarget) {
        // Bring up save panel
        string basename = Selection.activeObject ? Selection.activeObject.name : "New Resource";
        string path = EditorUtility.SaveFilePanel ("Save Resources", "", basename, "");

        if (path.Length != 0) {
            // Build the resource file from the active selection.
            Object[] selection = Selection.GetFiltered (typeof(Object), SelectionMode.DeepAssets);

            Debug.Log (selection.Length);
            Debug.Log (selection [0].name);

            BuildPipeline.BuildAssetBundle (
                                Selection.activeObject,
                                selection, string.Format ("{0}.{1}.unity3d", path, buildTarget.ToString ()),
                                BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets,
                                buildTarget);

            Selection.objects = selection;
        }
    }

    // ExportWithDependencies
#endregion

#region ExportNoDependencies

    private static void ExportNoDependencies (BuildTarget buildTarget) {
        // Bring up save panel
        string basename = Selection.activeObject ? Selection.activeObject.name : "New Resource";
        string path = EditorUtility.SaveFilePanel ("Save Resources", "", basename, "");

        if (path.Length != 0) {
            // Build the resource file from the active selection.
            Object[] selection = Selection.GetFiltered (typeof(Object), SelectionMode.DeepAssets);

            Debug.Log (selection.Length);
            Debug.Log (selection [0].name);

            BuildPipeline.BuildAssetBundle (
                                Selection.activeObject,
                                selection, string.Format ("{0}.{1}.unity3d", path, buildTarget.ToString ()),
                                BuildAssetBundleOptions.CompleteAssets,
                                buildTarget);

            Selection.objects = selection;
        }
    }

    // ExportNoDependencies
#endregion
}