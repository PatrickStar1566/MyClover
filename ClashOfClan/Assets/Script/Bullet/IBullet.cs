using UnityEngine;
using System.Collections;
public interface IBullet
{
	int ID{ get; set;}
	float DelayTime{ get; set;}
	void OnHited();
}

