using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LumenWorks.Framework.IO.Csv;


public class DataManager : UnityAllSceneSingleton<DataManager>, IMessageObject
{
	public static readonly string PathURL =
#if UNITY_ANDROID && !UNITY_EDITOR
		"jar:file://" + Application.dataPath + "!/assets/";
//#elif UNITY_IPHONE
//		Application.dataPath + "/Raw/";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
	"file://" + Application.dataPath + "/StreamingAssets" + "/";
	//#else
	//	string.Empty;
#endif
	public bool HasDoneResource = false;

	private static DataManager _Inst;
	public static void Init()
	{
		_Inst = DataManager.Instance;
	}

	void Start()
	{
		StartCoroutine(LoadMainGameObject(PathURL));
	}
	private IEnumerator LoadMainGameObject(string path)
	{

		this.PRINT("path" + path);
		string temppath = path + "csv.assetbundle";
		WWW bundle = new WWW(temppath);

		yield return bundle;
		Object[] objs = bundle.assetBundle.LoadAllAssets(typeof(soCsv));
		this.PRINT("count : " + objs.Length);
		for (int i = 0, max = objs.Length; i < max; i++)
		{
			this.PRINT("name : " + objs[i].name);
			Object obj = objs[i];
			soCsv csv = obj as soCsv;
			if (csv.name != "buildings" && csv.name != "hero" && csv.name != "skills")
			{
				continue;
			}
			// 作为临时存储介质;
			MemoryStream ms = new MemoryStream(csv.content);
			if (ms == null)
			{
				Debug.LogWarning("covert csv failed");
				continue;
			}
			StreamReader sr = new StreamReader(ms, Encoding.Default, true);
			TextReader tr = sr as TextReader;
			if (tr == null)
			{
				Debug.LogWarning("text reader is null");
				continue;
			}
			CsvReader cr = new CsvReader(tr, true);
			if (cr == null)
			{
				Debug.LogWarning("CsvReader is null");
				continue;
			}

			if (csv.name == "buildings")
			{
				ReadBuilding(cr);
				//CSVFileHelper.ReadCsv(cr, ReadBuilding);
			}
			else if (csv.name == "hero")
			{
				ReadHero(cr);
			}
			else if (csv.name == "skills")
			{
				ReadSkill(cr);
			}
		}

		bundle.assetBundle.Unload(false);

		temppath = path + "txt.assetbundle";
		bundle = new WWW(temppath);
		yield return bundle;
		TextAsset asset = bundle.assetBundle.LoadAsset("string", typeof(TextAsset)) as TextAsset;
		StringReader reader = new StringReader(asset.text);
		string line = reader.ReadLine();
		while (line != null)
		{
			string[] str = line.Split(new char[] { '=' }, System.StringSplitOptions.RemoveEmptyEntries);
			StringConfManager.Instance.AddStringConf(str[0], str[1]);
			this.PRINT(str[0] + "=" + str[1]);
			line = reader.ReadLine();
		}

		bundle.assetBundle.Unload(false);
		GameManager.Instance.curStatus = GameManager.Status.LOAD_SCENE;
		this.PRINT("=======================> data loaded <=====================");
	}

	private void ReadBuilding(CsvReader cr)
	{
		int fieldCount = cr.FieldCount;
		string[] headers = cr.GetFieldHeaders();
		//cr.ReadNextRecord ();// the real head
		while (cr.ReadNextRecord())
		{
			int i = 0;

			BuildingConf conf = new BuildingConf();
			conf.id = int.Parse(cr[i++]);
			conf.name = cr[i++];
			conf.type = int.Parse(cr[i++]);
			string[] buildRange = cr[i++].Split(';');
			conf.buildRange[0] = int.Parse(buildRange[0]);
			conf.buildRange[1] = int.Parse(buildRange[1]);
			conf.life = int.Parse(cr[i++]);
			conf.hitRate = int.Parse(empty2number(cr[i++]));
			conf.attackSpeed = float.Parse(empty2number(cr[i++]));
			string[] attack = cr[i++].Split(new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries);
			if (attack.Length != 0)
			{
				conf.attack[0] = int.Parse(attack[0]);
				conf.attack[1] = int.Parse(attack[1]);
			}
			string[] attackRange = cr[i++].Split(new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries);
			if (attackRange.Length != 0)
			{
				conf.attackRange[0] = float.Parse(attackRange[0]);
				conf.attackRange[1] = float.Parse(attackRange[1]);
			}
			conf.attackMode = int.Parse(empty2number(cr[i++]));
			conf.damageRange = float.Parse(empty2number(cr[i++]));
			conf.cooldownHit = int.Parse(empty2number(cr[i++]));
			conf.cooldownTime = float.Parse(empty2number(cr[i++]));
			conf.buffId = int.Parse(empty2number(cr[i++]));
			conf.level = int.Parse(empty2number(cr[i++]));
			conf.desc = cr[i++];
			conf.atlas = cr[i++];
			BuildingConfManager.Instance.AddBuildingConf(conf);
		}
		//BuildingConfManager.Instance.GetBuildingConfById (0);
	}
	private void ReadHero(CsvReader cr)
	{
		int fieldCount = cr.FieldCount;
		string[] headers = cr.GetFieldHeaders();
		//cr.ReadNextRecord ();// the real head
		while (cr.ReadNextRecord())
		{
			int i = 0;

			HeroConf conf = new HeroConf();
			conf.id = int.Parse(cr[i++]);
			conf.name = cr[i++];
			conf.type = int.Parse(cr[i++]);
			conf.level = int.Parse(cr[i++]);
			conf.exportname = cr[i++];
			conf.exportnameNpc = cr[i++];
			conf.moveSpeed = float.Parse(cr[i++]);
			conf.hitPoint = int.Parse(cr[i++]);
			conf.attackRange = float.Parse(cr[i++]);
			conf.attackSpeed = float.Parse(cr[i++]);
			conf.attackPower = int.Parse(cr[i++]);
			conf.attackRadius = float.Parse(cr[i++]);
			conf.activeSkill = int.Parse(cr[i++]);
			// more param has...
			HeroConfManager.Instance.AddHeroConf(conf);

		}
	}
	private void ReadSkill(CsvReader cr)
	{
		int fieldCount = cr.FieldCount;
		string[] headers = cr.GetFieldHeaders();
		//cr.ReadNextRecord ();// the real head
		while (cr.ReadNextRecord())
		{
			int i = 0;

			SkillConf conf = new SkillConf();
			conf.id = int.Parse(cr[i++]);
			conf.name = cr[i++];
			conf.skillType = int.Parse(cr[i++]);
			conf.kind = int.Parse(cr[i++]);
			conf.isManual = int.Parse(cr[i++]);
			conf.skillLimit = int.Parse(cr[i++]);
			conf.skillTime = int.Parse(cr[i++]);
			conf.summonID = int.Parse(empty2number(cr[i++]));
			conf.summonNum = int.Parse(empty2number(cr[i++]));
			conf.skillRange = int.Parse(empty2number(cr[i++]));
			SkillConfManager.Instance.AddSkillConf(conf);
		}

	}
	private string empty2number(string val)
	{
		return val == "" ? "0" : val;
	}

}

public class StringConfManager
{
	private static StringConfManager _Instance;
	public Dictionary<string, string> stringConfs = new Dictionary<string, string>();
	public static StringConfManager Instance
	{
		get
		{
			if (_Instance == null)
				_Instance = new StringConfManager();
			return _Instance;
		}
	}
	public void AddStringConf(string name, string txt)
	{
		stringConfs[name] = txt;
	}
	public string GetStringbyName(string name)
	{
		if (stringConfs.ContainsKey(name))
		{
			return stringConfs[name];
		}
		return null;
	}
}
public class SkillConf
{
	public int id;
	public string name;
	public int skillType;//1.command, 2.hero,3.positive
	public int kind;//10.call
	public int isManual;
	public int skillLimit;
	public int skillTime;//during time
	public int summonID;
	public int summonNum;
	public int skillRange;

}
public class SkillConfManager
{
	private static SkillConfManager _Instance;
	public Dictionary<int, SkillConf> skillConfs = new Dictionary<int, SkillConf>();
	public static SkillConfManager Instance
	{
		get
		{
			if (_Instance == null)
				_Instance = new SkillConfManager();
			return _Instance;
		}
	}
	public void AddSkillConf(SkillConf conf)
	{
		skillConfs.Add(conf.id, conf);
	}
	public SkillConf GetSkillConfById(int id)
	{
		if (skillConfs.ContainsKey(id))
		{
			return skillConfs[id];
		}
		return null;
	}
}
public class HeroConf
{
	public int id;
	public string name;
	public int type;
	public int level;
	public string exportname;
	public string exportnameNpc;
	public float moveSpeed;
	public int hitPoint;
	public float attackRange;
	public float attackSpeed;
	public int attackPower;
	public float attackRadius;
	public int activeSkill;

}
public class HeroConfManager
{
	private static HeroConfManager _Instance;
	public Dictionary<int, HeroConf> heroConfs = new Dictionary<int, HeroConf>();
	public static HeroConfManager Instance
	{
		get
		{
			if (_Instance == null)
				_Instance = new HeroConfManager();
			return _Instance;
		}
	}
	public void AddHeroConf(HeroConf conf)
	{
		heroConfs.Add(conf.id, conf);
	}
	public HeroConf GetHeroConfByType(/*CharaData.charModel type*/int type)
	{
		foreach (int id in heroConfs.Keys)
		{
			if (heroConfs[id].type == (int)type)
				return heroConfs[id];
		}
		return null;
	}
	public HeroConf GetHeroConfById(int id)
	{
		if (heroConfs.ContainsKey(id))
		{
			return heroConfs[id];
		}
		return null;
	}

}
public class BuildingConf
{
	public int id;
	public string name;
	public int type;
	public int[] buildRange = new int[2];
	public int life;
	public int hitRate;
	public float attackSpeed;
	public int[] attack = new int[2];
	public float[] attackRange = new float[2];
	public int attackMode;
	public float damageRange;
	public int cooldownHit;
	public float cooldownTime;
	public int buffId;
	public int level;
	public string desc;
	public string atlas;
}
public class BuildingConfManager
{
	private static BuildingConfManager _Instance;
	public Dictionary<int, BuildingConf> buildingConfs = new Dictionary<int, BuildingConf>();
	public static BuildingConfManager Instance
	{
		get
		{
			if (_Instance == null)
				_Instance = new BuildingConfManager();
			return _Instance;
		}
	}
	public void AddBuildingConf(BuildingConf conf)
	{
		//	Debug.LogError ("conf.id" + conf.id);
		if (buildingConfs.ContainsKey(conf.id))
			return;
		buildingConfs.Add(conf.id, conf);
	}
	public BuildingConf GetBuildingConfById(int id)
	{
		if (buildingConfs.ContainsKey(id))
		{
			return buildingConfs[id];
		}
		return null;
	}

}

