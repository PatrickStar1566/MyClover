using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillManager : UnityAllSceneSingleton<SkillManager> {

    Dictionary<int, ISkill> cammandSkills = new Dictionary<int, ISkill>();
    public void CalcSkillDamage(Chara attacker, Chara defender)
    {
        if ( attacker.GetCurrentSkillId() == 0 )
        {
            defender.OnBeHit(attacker.GetAttackPower());
        }
        else
        {
            // Use Skill;
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
