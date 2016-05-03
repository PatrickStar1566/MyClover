using UnityEngine;
using System.Collections;
using System;

public class Soldier : Chara {
    public CharaData.CharModel charaType;

    public Transform GetTransform()
    {
        return model.transform;
    }

    bool needDelayCall = false;
    float delayCallTime = 1.0f;
    bool needResearch = false;
    protected AIPath aiPath;
    protected Chara curAttackTarget = null;

    public Soldier() { }

    public Soldier(string prefabs)
    {
        model = (GameObject)GameObject.Instantiate(Resources.Load(prefabs));
        Transform soldier = model.transform.GetChild(0);
        // Renderer's order within a sorting layer.;
        soldier.gameObject.GetComponent<Renderer>().sortingOrder = layerOrder = LAYER_BASE;

        status = model.GetComponent<CharaStatus>();
        status.CurPose = CharaStatus.Pose.Run;
        status.Parent = this;

        // aiPath;
        aiPath = model.GetComponent<AIPath>();
        aiPath.canMove = false;
        aiPath.parent = this;
    }

    public override void Start()
    {
        data.classtype = (int)CharaData.CharClassType.CHARACTER;
        base.Start();
    }

    public Vector3 GetFeetPosition()
    {
        return aiPath.GetFeetPosition();
    }

    bool FindClosestTarget()
    {
        return GameManager.Instance.GetClosestBuilding(aiPath);
    }

    public override void Destroy()
    {
        base.Destroy();
    }

    public override void CancelAI()
    {
        base.CancelAI();
        if ( !aiPath.TargetReached )
        {
            aiPath.CancelCurrentPath();
            aiPath.canMove = false;
        }
    }

    public override void DoAI()
    {
        base.DoAI();

        if ( FindClosestTarget() == false )
        {
            status.CurPose = CharaStatus.Pose.Idle;
            aiPath.canMove = false;
        }
        else
        {
            status.CurPose = CharaStatus.Pose.Run;
            aiPath.canMove = true;
        }
    }

    public override void OnReachedTarget()
    {
        base.OnReachedTarget();
        if (needDelayCall)
            return;

        aiPath.endReachedDistance = data.attackRange;
        if ( needResearch )
        {
            needDelayCall = true;
            needResearch = false;
        }
        else
        {
            Building build = GameManager.Instance.GetLastChooseBuilding(aiPath);
            if (build == null || build.Life <= 0)
            {
                if (FindClosestTarget() == false)
                {
                    status.CurPose = CharaStatus.Pose.Idle;
                    aiPath.canMove = false;
                }
                else
                {
                    status.CurPose = CharaStatus.Pose.Run;
                    aiPath.canMove = true;
                }
                return;
            }
            curAttackTarget = build;
            data.currentUseSkillId = 0;
            status.CurPose = CharaStatus.Pose.Attack;
            aiPath.canMove = false;
        }
    }

    public override void OnAttackEnd()
    {
        base.OnAttackEnd();
        if (curAttackTarget != null && curAttackTarget.Life > 0)
        {
            SkillManager.Instance.CalcSkillDamage(this, curAttackTarget);
        }
        else
        {
            curAttackTarget.OnDieEvent -= OnTargetDie;
            if (needResearch)
            {
                needDelayCall = true;
                needResearch = false;
            }
            else
            {
                Building build = GameManager.Instance.GetLastChooseBuilding(aiPath);
                if (build == null || build.Life <= 0)
                {
                    if (FindClosestTarget() == false)
                    {
                        status.CurPose = CharaStatus.Pose.Idle;
                        aiPath.canMove = false;
                    }
                    else
                    {
                        status.CurPose = CharaStatus.Pose.Run;
                        aiPath.canMove = true;
                    }
                }
            }
        }
    }

    public override void OnPathComplete()
    {
        base.OnPathComplete();

        Building build = GameManager.Instance.GetLastChooseBuilding(aiPath);
        if (build != null)
        {
            curAttackTarget = build;
            curAttackTarget.OnDieEvent += OnTargetDie;
        }
        else
        {
            aiPath.lastChoosenTarget = -1;
        }
    }

    public virtual void OnTargetDie()
    {
        aiPath.lastChoosenTarget = -1;
        if (aiPath == null)
            return;
        if (FindClosestTarget() == false)
        {
            status.CurPose = CharaStatus.Pose.Idle;
            aiPath.canMove = false;
        }
        else
        {
            status.CurPose = CharaStatus.Pose.Run;
            aiPath.canMove = true;
        }
        if (curAttackTarget != null)
        {
            curAttackTarget.OnDieEvent -= OnTargetDie;
            curAttackTarget = null;
        }
    }

    void DoDelay()
    {
        if (FindClosestTarget() == false)
        {
            status.CurPose = CharaStatus.Pose.Idle;
            aiPath.canMove = false;
        }
        else
        {
            status.CurPose = CharaStatus.Pose.Run;
            aiPath.canMove = true;
        }
    }

    public virtual void OnDie()
    {

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

            GameManager.Instance.DeleteByID(ID);
            if (OnDieEvent != null)
                OnDieEvent();
        }
    }

    static float waittime = Time.realtimeSinceStartup;

    public override void Update()
    {
        base.Update();
        if ( needDelayCall )
        {
            if ( waittime + delayCallTime > Time.realtimeSinceStartup )
            {
                DoDelay();
                needDelayCall = false;
                waittime = Time.realtimeSinceStartup;
            }
        }
        else
            waittime = Time.realtimeSinceStartup;
    }
}
