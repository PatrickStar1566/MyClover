using UnityEngine;
using System.Collections;

public class Building : Chara {
    public Transform GetTransform()
    {
        return model.transform;
    }

    public CharaData.BuildingModel buildingType;
    bool beGuided = false;
    bool beHited = true;

    public override void Start()
    {
        data.classtype = (int)CharaData.CharClassType.BUILDING;
        int classType = data.classtype;
        base.Start();
        // blood
    }

    public override void OnBeHit(int damage)
    {
        base.OnBeHit(damage);
        data.life -= damage;
        if (data.life <= 0f)
        {
            data.life = 0;
            //if ( status.CurPose == CharaStatus.Pose.Attack )
            status.CurPose = CharaStatus.Pose.Die;
            if (OnDieEvent != null)
                OnDieEvent();
        }
        else
        {
            beHited = true;
        }
    }

    //    float tmpHideTime = Time.realtimeSinceStartup;
    public override void Update()
    {
        base.Update();
    }
}
