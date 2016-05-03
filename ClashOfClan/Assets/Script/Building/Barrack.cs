using UnityEngine;
using System.Collections;

public class Barrack : Building {

    public Barrack()
    {
        this.START_METHOD("Barrack");
        model = (GameObject)Object.Instantiate(Resources.Load("barrack"));
        model.name = "" + ID;

        Transform house = model.transform.GetChild(0);
        Renderer render = house.GetComponent<Renderer>();
        render.sortingOrder = layerOrder = LAYER_BASE + 1;
        Transform grass = model.transform.GetChild(1);
        Renderer grassRender = grass.GetComponent<Renderer>();
        grassRender.sortingOrder = 0;

        status = model.GetComponent<CharaStatus>();
        status.Parent = this;
        buildingType = CharaData.BuildingModel.BARRACK;
        this.END_METHOD("Barrack");
    }

    public override void Start()
    {
        int classType = data.classtype;
        base.Start();
        BuildingConf conf = BuildingConfManager.Instance.GetBuildingConfById(10102);
        if ( conf != null )
        {
            data.life = conf.life;
            data.maxlife = conf.life;
        }
    }

    public override void Update()
    {
        base.Update();
        if (data.life <= 0)
        {
            Transform house = model.transform.GetChild(0);
            house.GetComponent<Renderer>().sortingOrder = 1;
        }
    }
}
