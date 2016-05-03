using UnityEngine;
using System.Collections;

public struct CharaData
{
    public enum CharClassType
    {
        CHARACTER = 1,
        BUILDING,
    }

    public enum CharModel
    {
        NONE = -1,
        SOLDIER = 1,
        BOWMAN,
        BOWMAN1,
        BOWMAN2,
        BOWMAN3,
        BOWMAN4,
        GIANT,
        VIKING,
    }

    public enum BuildingModel
    {
        BARRACK = 1,
        CANNON,
    }

    public void Reset()
    {
        isDirty = true;
    }

    public bool isDirty;

    public long modelId;
    public int classtype;
    public int modeltype;//1.soldier, 2.bowman
    public int level;
    public Vector3 pos;
    public float speed;
    public int camp;
    public Vector3 rotation;
    public CharaStatus.Pose pose;
    public float life;
    public float maxlife;
    public int attackPower;
    public float attackRange;
    public float searchInterval;
    public float attackInterval;
    public int currentUseSkillId;
}