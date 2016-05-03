using UnityEngine;
using System.Collections;

public class Cannon : Building
{
    public Soldier curEnemy;
    float mHitDelta;
    bool endAttack = true;
    int curFps = 0;
    float mFPS = 16;
    float moveDistance = 0.5f;
    float scaleFactor = 0.6f;
    int turnSpeed = 12;

    public Cannon()
    {
        this.START_METHOD("Cannon");

        model = (GameObject)Object.Instantiate(Resources.Load("cannon"));
        model.name = "" + ID;

        Transform house = model.transform.GetChild(0);
        Renderer render = house.GetComponent<Renderer>();
        render.sortingOrder = layerOrder = LAYER_BASE + 1;
        Transform grass = model.transform.GetChild(1);
        Renderer grassRender = grass.GetComponent<Renderer>();
        grassRender.sortingOrder = 0;
        Transform support = model.transform.GetChild(2);
        support.GetComponent<Renderer>().sortingOrder = 2;

        status = model.GetComponent<CharaStatus>();
        status.rotateWeapon = true;
        status.CurPose = CharaStatus.Pose.None;
        status.Parent = this;
        buildingType = CharaData.BuildingModel.CANNON;

        this.END_METHOD("Cannon");
    }

    public override void Start()
    {
        base.Start();
        BuildingConf conf = BuildingConfManager.Instance.GetBuildingConfById(10111);
        if (conf != null)
        {
            data.life = 20;//conf.life;
            data.maxlife = 20;//conf.life;
            data.attackRange = conf.attackRange[1];
            data.searchInterval = 0.1f;
            data.attackInterval = 1.0f;
            data.attackPower = conf.attack[1];
        }
    }

    public override void Update()
    {
        base.Update();

        if ( data.life <= 0 )
        {
            Transform house = model.transform.GetChild(0);
            house.GetComponent<Renderer>().sortingOrder = 1;
        }

        CheckEnemy();

        HitEnemy();
    }

    public override void DoAI()
    {
        base.DoAI();
        curEnemy = null;
    }

    float lastTime = Time.realtimeSinceStartup;
    float lastAttackTime = Time.realtimeSinceStartup;
    bool canAttack = false;
    Vector3 lastDir = Vector3.zero;
    void RotateTowards(Vector3 dir)
    {
        if (dir == Vector3.zero) return;
        if (canAttack == false)
            lastDir = dir;
        Quaternion rot = GetTransform().rotation;
        Quaternion toTarget = Quaternion.LookRotation(lastDir);

        rot = Quaternion.Slerp(rot, toTarget, turnSpeed * Time.deltaTime);
        Vector3 euler = rot.eulerAngles;
        euler.z = 0;
        euler.x = 0;
        rot = Quaternion.Euler(euler);
        GetTransform().rotation = rot;
        if ( Quaternion.Angle(GetTransform().rotation, toTarget) < 10.0f )
            canAttack = true;
    }

    public void CheckEnemy()
    {
        if (GameManager.Instance.curStatus != GameManager.Status.START_GAME)
            return;
        if ( Time.realtimeSinceStartup > lastTime + data.searchInterval )
        {
            curEnemy = GameManager.Instance.FindEnemyByDistance(this);
            if ( curEnemy != null )
            {
                status.CurPose = CharaStatus.Pose.Attack;
                Vector3 dir = curEnemy.GetFeetPosition() - GetTransform().position;
                dir.y = 0;
                RotateTowards(dir);
            }
            lastTime = Time.realtimeSinceStartup;
        }
    }

    public void HitEnemy()
    {
        if (GameManager.Instance.curStatus != GameManager.Status.START_GAME)
            return;

        curEnemy = GameManager.Instance.FindEnemyByDistance(this);
        if ( curEnemy == null )
        {
            GetTransform().GetChild(0).localPosition = new Vector3(0.0f, 0.3f, 0.0f);
            status.CurPose = CharaStatus.Pose.Idle;
            return;
        }
        HitAni();
        if( Time.realtimeSinceStartup > lastAttackTime + data.attackInterval )
        {
            endAttack = false;
            lastAttackTime = Time.realtimeSinceStartup;
        }
    }

    void HitAni()
    {
        if (endAttack || canAttack == false)
            return;
        mHitDelta += RealTime.deltaTime;
        float rate = 1f / mFPS;
        if ( rate < mHitDelta )
        {
            mHitDelta = (rate > 0f) ? mHitDelta - rate : 0f;
            if ( curFps > 0 )
            {
                curFps = 0;
                GetTransform().GetChild(0).localPosition = new Vector3(0.0f, 0.3f, 0.0f);
                QuadTexture4Ngui gui = GetTransform().GetChild(0).GetComponent<QuadTexture4Ngui>();
                gui.ScaleFactor = 0.5f;

                endAttack = true;
                canAttack = false;
            }
            else
            {
                GameObject bulletgo = (GameObject)GameObject.Instantiate(Resources.Load("cannonbullet"));

                CannonBullet bullet = bulletgo.GetComponent<CannonBullet>();
                Transform gun = GetTransform().GetChild(0);
                bulletgo.transform.position = GetTransform().position + GetTransform().forward * 0.6f;

                bullet.parent = this;

                bullet.Fire(curEnemy.GetTransform().gameObject);
                Vector3 dir = Vector3.back * moveDistance;
                GetTransform().GetChild(0).localPosition = new Vector3(dir.x, dir.y + 0.3f, dir.z);
                QuadTexture4Ngui gui = GetTransform().GetChild(0).GetComponent<QuadTexture4Ngui>();
                gui.ScaleFactor = scaleFactor;
                ++curFps;
            }
        }
    }
}
