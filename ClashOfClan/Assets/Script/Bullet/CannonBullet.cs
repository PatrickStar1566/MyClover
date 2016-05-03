using UnityEngine;
using System.Collections;

public class CannonBullet : MonoBehaviour,IBullet
{
	
	public int ID{ get; set;}
	public float DelayTime{ get; set;}
	public float speed = 5;  
	public Chara parent{ get; set;}
	private float distanceToTarget;  
	private bool move = true; 
	private GameObject curTarget;
	
	void Start ()  
	{  

	}  
	public void Fire(GameObject target)
	{
		curTarget = target;		
		Transform house = transform.GetChild (0);
		house.gameObject.GetComponent<Renderer>().sortingOrder = 4;
		distanceToTarget = Vector3.Distance (this.transform.position, target.transform.position);  
		StartCoroutine (Shoot (target));  
	}
	IEnumerator Shoot (GameObject target)  
	{  
		
		while (move) {  
			if(target != null)
			{
				Vector3 targetPos = target.transform.position;  
				this.transform.LookAt (targetPos); 
				float currentDist = Vector3.Distance (this.transform.position, target.transform.position);  

				if (currentDist < 0.5f) 
				{
					move = false;  
					OnHited ();
				}
				  this.transform.Translate (Vector3.forward * Mathf.Min (speed * Time.deltaTime, currentDist)); 

				yield return null;
				
			}
			else
			{
				move = false;
				GameObject.Destroy(gameObject);
			}
		}  
		//
	}  
	public void OnHited()
	{
		CharaStatus status = curTarget.GetComponent<CharaStatus> ();
		SkillManager.Instance.CalcSkillDamage (parent, status.Parent);
		if (status.Parent.Life <= 0)
        {
			Cannon cannon = parent as Cannon;
			cannon.curEnemy = null;
		}

		GameObject.Destroy(gameObject);
	}
}

