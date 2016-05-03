using UnityEngine;
using System.Collections;
public interface ISkill
{
	int ID{ get; set;}
	string Name{ get; set;}
	float DelayTime{ get; set;}
	void OnDie();
}

