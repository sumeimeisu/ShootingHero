﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class EnemyEmitter : MonoBehaviour {

	[SerializeField]
	public GameObject EmittionEnemy;
	[SerializeField]
	public GameObject EmittionBoss;

	[SerializeField]
	public TextAsset StageInformation;

	[SerializeField]
	public GameObject NextStage;

	float elapsedTime;

	struct StageItem { public bool IsBoss; public float Position, Angle, RaiseTime; }
	Queue<StageItem> stageItems = new Queue<StageItem> ();
	string bossName;

	bool createdNextStage = false;
	string nextStageName;

	// Use this for initialization
	void Start () {

		Regex monsterItem = new Regex ( "m (-?[0-9]+) (-?[0-9]+) ([0-9]+)" );
		Regex bossItem = new Regex ( "b ([0-9]+)" );

		string [] lines = StageInformation.text.Split ( '\n' );
		foreach ( string line in lines )
		{
			StageItem item = new StageItem ();
			switch ( line [ 0 ])
			{
				case 'm':
					{
						var match = monsterItem.Match ( line );
						item.IsBoss = false;
						item.Position = int.Parse ( match.Groups [ 1 ].Value ) / 480.0f * 4 - 2;
						item.Angle = float.Parse ( match.Groups [ 2 ].Value );
						item.RaiseTime = int.Parse ( match.Groups [ 3 ].Value ) / 1000.0f;
					}
					break;

				case 'b':
					{
						var match = bossItem.Match ( line );
						item.IsBoss = true;
						item.Position = 0;
						item.Angle = 0;
						item.RaiseTime = int.Parse ( match.Groups [ 1 ].Value ) / 1000.0f;
					}
					break;
			}
			stageItems.Enqueue ( item );
		}
	}
	
	// Update is called once per frame
	void Update () {

		elapsedTime += Time.deltaTime;

		do
		{
			if ( stageItems.Count == 0 ) break;
			if ( stageItems.Peek ().RaiseTime <= elapsedTime )
			{
				StageItem currentItem = stageItems.Dequeue ();
				GameObject mob = Instantiate ( currentItem.IsBoss ? EmittionBoss : EmittionEnemy );
				mob.transform.position = new Vector3 ( currentItem.Position, currentItem.IsBoss ? 5.6f : 4.4f, 0 );
				if ( !currentItem.IsBoss )
				{
					mob.GetComponent<EnemyController> ().Angle = currentItem.Angle;
				}
				else
				{
					bossName = mob.name;
					Debug.Log ( bossName );
				}
			}
			else
				break;
		} while ( true );

		if ( stageItems.Count == 0 && GameObject.Find ( bossName ) == null )
		{
			//Debug.Log ( "NextStage" );
			if ( !createdNextStage )
			{
				createdNextStage = true;

				if ( NextStage != null )
				{
					var nextStage = Instantiate ( NextStage );
					--nextStage.transform.GetChild ( 0 ).GetComponent<SpriteRenderer> ().sortingOrder;
					nextStageName = nextStage.name;
				}
			}

			var backgroundRenderer = gameObject.transform.GetChild ( 0 ).GetComponent<SpriteRenderer> ();
			backgroundRenderer.color = new Color ( 1, 1, 1, backgroundRenderer.color.a - ( 0.3f * Time.deltaTime ) );
			if ( backgroundRenderer.color.a <= 0 )
			{
				var nextStage = GameObject.Find ( nextStageName );
				if ( nextStage != null )
				{
					++nextStage.transform.GetChild ( 0 ).GetComponent<SpriteRenderer> ().sortingOrder;
				}
				Destroy ( gameObject );
			}
		}
	}
}
