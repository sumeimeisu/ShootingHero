﻿using Assets.Scripts.Static;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharBulletCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnCollisionEnter2D ( Collision2D collision )
	{
		if ( collision.gameObject.tag == "Enemy" )
		{
			InGameParameter.DiscountPoint ( 0, collision.gameObject.GetComponent<EnemyController> ().OutRangeDamage );
			Instantiate ( Resources.Load<GameObject> ( "Prefabs/Effects/BoomEffect" ) ).transform.position = gameObject.transform.position;
		}
		else if ( collision.gameObject.tag == "Enemy Bullet" )
		{
			InGameParameter.DiscountPoint ( collision.gameObject.GetComponent<EnemyBulletController> ().Damage );
			Instantiate ( Resources.Load<GameObject> ( "Prefabs/Effects/BoomEffect" ) ).transform.position = gameObject.transform.position;
		}
	}
}
