using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharaManager : IMessageObject {
    List<Chara> chars = new List<Chara>();
    List<Chara> building = new List<Chara>();
    List<Chara> allChara = new List<Chara>();
    List<Vector3> positions = new List<Vector3>();

    public Chara SpawnChar(CharaData.CharClassType classType, int charModelType, int camp, int level, Vector3 pos,
        Vector3 dir, CharaStatus.Pose pose)
    {
        this.START_METHOD("SpawnChar");
        Chara tempChar = null;
        if ( classType == CharaData.CharClassType.CHARACTER )
        {
            if ( (CharaData.CharModel)charModelType == CharaData.CharModel.BOWMAN )
            {
                BowMan chara = new BowMan();
                chara.SetPos(pos);
                chara.SetDir(dir);
                chara.SetPose(pose);
                chara.SetCamp(camp);
                tempChar = chara;
            }

            if (tempChar != null)
                chars.Add(tempChar);
            else
                throw new UnityException("no current chara type to spawn");
        }
        else if ( classType == CharaData.CharClassType.BUILDING )
        {
            if ( (CharaData.BuildingModel)charModelType == CharaData.BuildingModel.BARRACK )
            {
                Barrack chara = new Barrack(); // TODO: need change class to resource pool
                chara.SetPos(pos);
                chara.SetDir(dir);
                chara.SetCamp(camp);
                tempChar = chara;
            }
            else if ( (CharaData.BuildingModel)charModelType == CharaData.BuildingModel.CANNON )
            {
                Cannon chara = new Cannon();
                chara.SetPos(pos);
                chara.SetDir(dir);
                chara.SetCamp(camp);
                tempChar = chara;
            }

            if (tempChar != null)
                building.Add(tempChar);
            else
                throw new UnityException("no current building type to spawn");
        }

        allChara.Add(tempChar);

        this.END_METHOD("SpawnChar");
        return tempChar;
    }

    public Building GetChoseBuildingByPos(Vector3 pos)
    {
        for (int i = 0; i < building.Count; ++i)
        {
            if (building[i].Life <= 0) continue;
            Building curBuilding = (Building)building[i];
            if (pos == curBuilding.GetTransform().position)
                return curBuilding;
        }

        return null;
    }

    public Building GetLastChooseBuildingBySeeker(AIPath path)
    {
        Building curBuilding = GetChooseBuildingByPos(path.lastTargetPos);
        return curBuilding;
    }

    public Building GetChooseBuildingByPos(Vector3 pos)
    {
        for (int i = 0; i < building.Count; ++i)
        {
            if (building[i].Life <= 0) continue;
            Building curBuild = (Building)building[i];
            if (pos == curBuild.GetTransform().position)
                return curBuild;
        }

        return null;
    }

    public int GetChooseTheBuildingIdxByPos(Vector3 pos)
    {
        List<Chara> liveBuilding = new List<Chara>();
        int j = -1;
        for (int i = 0; i < building.Count; ++i)
        {
            if (building[i].Life <= 0) continue;
            ++j;
            Building curBuild = (Building)building[i];
            if (pos == curBuild.GetTransform().position)
                return j;
        }

        return j;
    }

    public void DestroyAll()
    {
        this.START_METHOD("DestroyAll");
        allChara.Clear();
        for (int i = chars.Count - 1; i >= 0; --i)
        {
            chars[i].Destroy();
            chars.RemoveAt(i);
        }
        for (int i = building.Count - 1; i >= 0; --i)
        {
            building[i].Destroy();
            building.RemoveAt(i);
        }
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
        this.END_METHOD("DestroyAll");
    }

    public void DestroyChar(long id)
    {
        this.START_METHOD("DestroyChar");
        for(int i = chars.Count - 1; i >= 0; --i)
        {
            if ( chars[i].ID == id )
            {
                chars[i].Destroy();
                chars.RemoveAt(i);
                break;
            }
        }

        for (int i = allChara.Count - 1; i >= 0; --i)
        {
            if ( allChara[i].ID == id )
            {
                if ( allChara[i].ID == id )
                {
                    allChara[i].Destroy();
                    allChara.RemoveAt(i);
                    break;
                }
            }
        }
        this.END_METHOD("DestroyChar");
    }

    public void DestroyBuilding(long id)
    {
        this.START_METHOD("DestroyBuilding");
        for (int i = building.Count - 1; i >= 0; --i)
        {
            if (building[i].ID == id)
            {
                building[i].Destroy();
                building.RemoveAt(i);
                break;
            }
        }

        for (int i = allChara.Count - 1; i >= 0; --i)
        {
            if (allChara[i].ID == id)
            {
                if (allChara[i].ID == id)
                {
                    allChara[i].Destroy();
                    allChara.RemoveAt(i);
                    break;
                }
            }
        }
        this.END_METHOD("DestroyBuilding");
    }

    public Soldier FindEnemySoldierByDistance(Building building)
    {
        Dictionary<float, Soldier> soldiers = new Dictionary<float, Soldier>();
        float shortestDist = 10;
        for (int i = 0; i < chars.Count; ++i)
        {
            if (chars[i].GetCamp() == building.GetCamp()) continue;
            Soldier soldier = (Soldier)chars[i];
            Vector3 dir = building.GetTransform().position - soldier.GetFeetPosition();
            dir.y = 0;
            float targetDist = dir.magnitude;
            if ( targetDist <= building.GetAttackRange() )
            {
                if (targetDist < shortestDist)
                {
                    shortestDist = targetDist;
                    soldiers[targetDist] = soldier;
                }
            }
        }

        if (soldiers.Count == 0)
            return null;
        return soldiers[shortestDist];
    }

    public bool ClosestBuildingBySeeker(AIPath ai)
    {
//         float shortestPath = 0;
//         int shortestBuilding = 0;

        positions.Clear();
        for (int i = 0; i < building.Count; ++i)
        {
            if (building[i].Life <= 0) continue;
            Building build = (Building)building[i];
            positions.Add(build.GetTransform().position);
        }

        if ( positions.Count == 0 )
        {
            return false;
        }
        ai.MultiplySearch(positions.ToArray());
        return true;
    }

    public Building GetBuildingById(long id)
    {
        for (int i = building.Count - 1; i >= 0; --i)
        {
            if (building[i].ID == id)
                return building[i] as Building;
        }
        return null;
    }
}
