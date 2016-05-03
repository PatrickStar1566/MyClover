using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class BuildTable : Editor
{
    [MenuItem("BuildAssetBundle/Build Csv")]
    // this static is neccessarily;
    static void BuildCsv()
    {
        // dataPath is Assets;
        string applicationPath = Application.dataPath;
        // StreamingAssets for Unity;
        string saveDir = applicationPath + "/StreamingAssets/";
        string savePath = saveDir + "csv.assetbundle";

        // 过滤器,拿到当前选择的目录;
        Object[] selections = Selection.GetFiltered(typeof(object), SelectionMode.DeepAssets);
        List<Object> outs = new List<Object>();
        for (int i = 0, max = selections.Length; i < max; i++)
        {
            Object obj = selections[i];
            //asset path:short path to the asset folder
            string fileAssetPath = AssetDatabase.GetAssetPath(obj);
            //check the prefix
            if (fileAssetPath.Substring(fileAssetPath.LastIndexOf('.') + 1) != "csv")
                continue;

            string fileWholePath = applicationPath + "/" + fileAssetPath.Substring(fileAssetPath.IndexOf("/"));

            soCsv csv = ScriptableObject.CreateInstance<soCsv>();
            csv.fileName = obj.name;
            csv.content = File.ReadAllBytes(fileWholePath);
            //does this read and write is neccessarily;
            string assetPathTemp = "Assets/Resources_Local/Temp/" + obj.name + ".asset";
            AssetDatabase.CreateAsset(csv, assetPathTemp);

            Object outObj = AssetDatabase.LoadAssetAtPath(assetPathTemp, typeof(soCsv));

            Debug.Log("package : " + outObj.name);
            outs.Add(outObj);
        }
        Object[] outObjs = outs.ToArray();
        if (BuildPipeline.BuildAssetBundle(null, outs.ToArray(), savePath, BuildAssetBundleOptions.CollectDependencies, BuildTarget.Android))
            EditorUtility.DisplayDialog("ok", "build" + savePath + "success, length = " + outObjs.Length, "ok");
        else
            Debug.LogWarning("build" + savePath + "failed");

        AssetDatabase.Refresh();
    }
}

