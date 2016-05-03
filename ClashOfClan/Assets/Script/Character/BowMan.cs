using UnityEngine;
using System.Collections;

public class BowMan : Hero {
    HeroConf conf;
    public BowMan() : base("bowman")
    {
        this.START_METHOD("BowMan");
        charaType = CharaData.CharModel.BOWMAN;
        // bowman id 1;
        conf = HeroConfManager.Instance.GetHeroConfById(1);
        if (conf != null)
        {
            data.life = conf.hitPoint;
            data.maxlife = conf.hitPoint;
            data.attackPower = conf.attackPower;
            data.attackRange = conf.attackRange;
        }
        this.END_METHOD("BowMan");
    }

    public override void Destroy()
    {
        base.Destroy();
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }
}
