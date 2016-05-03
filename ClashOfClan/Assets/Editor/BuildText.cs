using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

public class BuildText : MonoBehaviour {
    [MenuItem("BuildAssetBundle/Build Text")]
    static void Buildtxt()
    {
        string applicationPath = Application.dataPath;
        string saveDir = applicationPath + "/StreamingAssets/";
        string savePath = saveDir + "txt.assetbundle";

        Object[] selections = Selection.GetFiltered(typeof(object), SelectionMode.DeepAssets);
        List<Object> outs = new List<Object>();
        for (int i = 0, max = selections.Length; i < max; i++)
        {
            Object obj = selections[i];
            //asset path:short path to the asset folder
            string fileAssetPath = AssetDatabase.GetAssetPath(obj);
            //check the prefix
            if (fileAssetPath.Substring(fileAssetPath.LastIndexOf('.') + 1) != "txt")
                continue;

            string fileWholePath = applicationPath + "/" + fileAssetPath.Substring(fileAssetPath.IndexOf("/"));
            outs.Add(obj);
        }
        if (BuildPipeline.BuildAssetBundle(null, outs.ToArray(), savePath, 
            BuildAssetBundleOptions.CollectDependencies|BuildAssetBundleOptions.CompleteAssets, BuildTarget.Android))
            EditorUtility.DisplayDialog("ok", "build" + savePath + "success, length = " + outs.Count, "ok");
        else
            Debug.LogWarning("build" + savePath + "failed");
    }
}
