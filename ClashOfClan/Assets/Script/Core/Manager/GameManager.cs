using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : UnityAllSceneSingleton<GameManager>, IMessageObject {

    public enum Status
    {
        NONE = 1,
        LOAD_RESOURCE,
        LOAD_SCENE,
        PREPARE_SCAN,
        START_GAME,
        END_GAME,
    }

    public CharaManager charaManager;
    public Status curStatus = Status.NONE;
    bool startScan = false;
    public override void Awake()
    {
        base.Awake();
        charaManager = new CharaManager();
    }

    void Start()
    {

    }

    public void ReloadScene(int scene)
    {
        this.START_METHOD("ReloadScene");

        Vector3 obstacle1Pos = new Vector3(10f, 0f, 15f);
        Cannon cannon = (Cannon)charaManager.SpawnChar(CharaData.CharClassType.BUILDING, (int)CharaData.BuildingModel.CANNON,
            1, 1, obstacle1Pos, new Vector3(0, 0, 0), CharaStatus.Pose.Idle);

        Vector3 obstacle2Pos = new Vector3(19f, 0f, 13f);
        cannon = (Cannon)charaManager.SpawnChar(CharaData.CharClassType.BUILDING, (int)CharaData.BuildingModel.CANNON,
            1, 1, obstacle2Pos, new Vector3(0, 0, 0), CharaStatus.Pose.Idle);

        Vector3 obstacle3Pos = new Vector3(9.77f, 0f, 9.30f);
        Barrack barrack = (Barrack)charaManager.SpawnChar(CharaData.CharClassType.BUILDING, (int)CharaData.BuildingModel.BARRACK,
            1, 1, obstacle3Pos, new Vector3(0, 0, 0), CharaStatus.Pose.Idle);

        Vector3 obstacle4Pos = new Vector3(13.17f, 0f, 9.73f);
        barrack = (Barrack)charaManager.SpawnChar(CharaData.CharClassType.BUILDING, (int)CharaData.BuildingModel.BARRACK,
            1, 1, obstacle4Pos, new Vector3(0, 0, 0), CharaStatus.Pose.Idle);

        Vector3 obstacle5Pos = new Vector3(10.63f, 0f, 21.06f);
        barrack = (Barrack)charaManager.SpawnChar(CharaData.CharClassType.BUILDING, (int)CharaData.BuildingModel.BARRACK,
            1, 1, obstacle5Pos, new Vector3(0, 0, 0), CharaStatus.Pose.Idle);

        Vector3 obstacle6Pos = new Vector3(30.75f, 0f, 18.58f);
        barrack = (Barrack)charaManager.SpawnChar(CharaData.CharClassType.BUILDING, (int)CharaData.BuildingModel.BARRACK,
            1, 1, obstacle6Pos, new Vector3(0, 0, 0), CharaStatus.Pose.Idle);

        StartCoroutine(DoDelay());
        this.END_METHOD("ReloadScene");
    }

    IEnumerator DoDelay()
    {
        yield return null;//new WaitForSeconds(1.0f);
        AstarPath.active.Scan();
        startScan = true;
    }

    public Building GetLastChooseBuilding(AIPath path)
    {
        return charaManager.GetLastChooseBuildingBySeeker(path);
    }

    public bool GetClosestBuilding(AIPath path)
    {
        return charaManager.ClosestBuildingBySeeker(path);
    }

    public Building GetChooseBuildingByPos(Vector3 pos)
    {
        return charaManager.GetChooseBuildingByPos(pos);
    }

    public int GetChooseTheBuildingIdxByPos(Vector3 pos)
    {
        return GetChooseTheBuildingIdxByPos(pos);
    }

    public void DestroyAll()
    {
        this.START_METHOD("DestroyAll");
        charaManager.DestroyAll();
        curStatus = Status.LOAD_SCENE;
        this.END_METHOD("DestroyAll");
    }

    public void DeleteByID(long id)
    {
        charaManager.DestroyChar(id);
    }

    public Soldier FindEnemyByDistance(Building building)
    {
        return charaManager.FindEnemySoldierByDistance(building);
    }

    public Chara SpwanCharacter(CharaData.CharClassType type, CharaData.CharModel model, int camp, int level, Vector3 startPos, Vector3 startDir, CharaStatus.Pose pose)
    {
        return charaManager.SpawnChar(type, (int)model, camp, level, startPos, startDir, pose);
    }
    // Update is called once per frame
    void Update () {
        switch ( curStatus )
        {
            case Status.LOAD_RESOURCE:
                break;
            case Status.LOAD_SCENE:
                ReloadScene(1);
                curStatus = Status.PREPARE_SCAN;
                break;
            case Status.PREPARE_SCAN:
                if (startScan && !AstarPath.active.isScanning)
                curStatus = Status.START_GAME;
                break;
            case Status.START_GAME:
                break;
            case Status.END_GAME:
                break;

        }
	}
}
