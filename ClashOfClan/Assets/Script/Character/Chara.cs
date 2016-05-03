using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class Chara : IMessageObject, IComparable<Chara> {

	protected static int LAYER_BASE = 3;
	protected CharaData data;
	protected GameObject model;
	protected GameObject blood;
	protected CharaStatus status;
	protected int layerOrder;

	public delegate void OnDieHandle();
	public OnDieHandle OnDieEvent;

	static long id;
	long mId;
	public long ID { get { return mId; } }
    public float Life { get { return data.life; } }

    public int GetCamp()
    {
        return data.camp;
    }

    public int GetAttackPower()
    {
        return data.attackPower;
    }

    public float GetAttackRange()
    {
        return data.attackRange;
    }

    public int GetCurrentSkillId()
    {
        return data.currentUseSkillId;
    }

	public Vector3 GetLocalPos() { return model.transform.localPosition; }
	public Vector3 GetRealPos() { return model.transform.position; }

	private bool bNeedChange = false;

	private bool bInited = false;

	public Chara()
	{
		this.START_METHOD("Chara");
		data = (CharaData)MemoryManager.Instance.CreateNativeStruct("CharaData");
		id++;
		mId = id;
		bInited = false;
		this.END_METHOD("Chara");
	}

	// Use this for initialization
	public virtual void Start () {
		if (data.classtype == (int)CharaData.CharClassType.CHARACTER)
			mId += 10000;
	}
	public virtual void SetLayer(int order)
	{
		this.START_METHOD("SetLayer");
		layerOrder = order + LAYER_BASE;
		Renderer renderer = model.GetComponent<Renderer>();
		renderer.sortingOrder = layerOrder;
		this.END_METHOD("SetLayer");
	}

	public void SetLevel(int level)
	{
		this.START_METHOD("SetLevel");
		data.level = level;
		this.END_METHOD("SetLevel");
	}

	public void SetColor(Color color)
	{
		Renderer render = model.GetComponent<Renderer>();
		render.material.SetColor("_Color", color);
	}

    public void SetCamp(int camp)
    {
        this.START_METHOD("SetCamp");
        data.camp = camp;
        this.END_METHOD("SetCamp");
    }
    
    public virtual void DoAI()
    {
        this.START_METHOD("DoAI");
        this.END_METHOD("DoAI");
    }

    public virtual void CancelAI()
    {
        this.START_METHOD("CancelAI");
        this.END_METHOD("CancelAI");
    }


	public int CompareTo(Chara other)
	{
		return 0;
	}

	public virtual void Destroy()
	{
		GameObject.Destroy(model);
        data.isDirty = true;
	}

	public Vector3 GetPos()
	{
		return data.pos;
	}

    public virtual void OnAttackEnd()
    {

    }

    public void SetPose(CharaStatus.Pose pose)
    {
        this.START_METHOD("SetPose");
        data.pose = pose;
        bNeedChange = true;
        this.END_METHOD("SetPose");
    }
	public void SetPos(Vector3 pos)
	{
		this.START_METHOD("SetPos");
		data.pos = pos;
		bNeedChange = true;
		this.END_METHOD("SetPos");
	}

	public void SetDir(Vector3 rotate)
	{
		this.START_METHOD("SetDir");
		data.rotation = rotate;
		bNeedChange = true;
		this.END_METHOD("SetDir");
    }

    public virtual void OnBeHit(int damage)
    {

    }

    public virtual void OnPathComplete()
    {

    }

    public virtual void OnReachedTarget()
    {

    }

	// Update is called once per frame
	public virtual void Update () {
		if ( bNeedChange )
		{
			model.transform.localPosition = data.pos;
			model.transform.localRotation = Quaternion.Euler(data.rotation);
            status.CurPose = data.pose;
			bNeedChange = false;
            if ( !bInited )
            {
                DoAI();
                bInited = true;
            }
		}
	}
}
