using UnityEngine;
using System.Collections;

public class GlobalManager : UnityAllSceneSingleton<GlobalManager>
{
    // 刷新频率;
    public float f_UpdateInterval = 0.5f;
    // 上一帧刷新帧;
    private float f_LastInterval;
    // 总帧数;
    private int i_Frames = 0;
    // 平均帧数,FPS:Frame per second;
    private float f_FPS;
    // 当前顶点数;
    public static int verts;
    // 当前面数;
    public static int tris;

    public override void Awake()
    {
        Application.targetFrameRate = 45;
    }

    void Start()
    {
        // Load All Mangers;
        GameManager.Instance.curStatus = GameManager.Status.LOAD_RESOURCE;
        InputListener.Init();
        DataManager.Init();
        f_LastInterval = Time.realtimeSinceStartup;
        i_Frames = 0;
    }

    void GetObjectStats()
    {
        verts = 0;
        tris = 0;
        GameObject[] ob = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (GameObject obj in ob)
        {
            GetObjectStats(obj);
        }
    }

    void GetObjectStats(GameObject obj)
    {
        Component[] filters;
        // MeshFilter多面体,面片;
        filters = obj.GetComponentsInChildren<MeshFilter>();

        foreach (MeshFilter f in filters)
        {
            // 三角形三个顶点, / 3表示一个面;
            tris += f.sharedMesh.triangles.Length / 3;
            verts += f.sharedMesh.vertexCount;
        }
    }

    void OnGUI()
    {
        GUI.skin.label.normal.textColor = new Color(255, 0, 0, 1.0f);
        //GUI.Label(new Rect(0, 10, 200, 200), "FPS:" + (f_FPS).ToString("f2"));
        GUILayout.Label("FPS:" + (f_FPS).ToString("f2"));
        GUI.skin.label.normal.textColor = new Color(255, 255, 0, 1.0f);
        string vertsdisplay = verts.ToString("#, ##0 verts");
        GUILayout.Label(vertsdisplay);
        string trisdisplay = tris.ToString("#, ##0 tris");
        GUILayout.Label(trisdisplay);
    }

    void Update()
    {
        ++i_Frames;
        if ( Time.realtimeSinceStartup > f_UpdateInterval + f_LastInterval )
        {
            f_FPS = i_Frames / (Time.realtimeSinceStartup - f_LastInterval);
            i_Frames = 0;

            f_LastInterval = Time.realtimeSinceStartup;
            GetObjectStats();
        }
    }
}
