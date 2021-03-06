﻿using Assets.Scripts.Static;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour {

	[SerializeField]
	public GameObject HP1, HP2, HP3;
	[SerializeField]
	public GameObject Bomb;

	[SerializeField]
	public GameObject Gage, FlickerGage;

	[SerializeField]
	public GameObject PowerLevel;

	[SerializeField]
	public Sprite Lv1, Lv2, Lv3, Lv4, Lv5;

	[SerializeField]
	public GameObject PointFont;

	[SerializeField]
	public GameObject GameOverOverlay;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		HP1.GetComponent<Image> ().fillAmount = Mathf.Max ( InGameParameter.CharacterHitPoint - 20, 0 ) / 10.0f;
		HP2.GetComponent<Image> ().fillAmount = Mathf.Max ( Mathf.Min ( InGameParameter.CharacterHitPoint - 10, 10 ), 0 ) / 10.0f;
		HP3.GetComponent<Image> ().fillAmount = Mathf.Max ( Mathf.Min ( InGameParameter.CharacterHitPoint, 10 ), 0 ) / 10.0f;

		if ( InGameParameter.CharacterChangeGage > 100 )
			InGameParameter.CharacterChangeGage = 100;
		Gage.GetComponent<Image> ().fillAmount = InGameParameter.CharacterChangeGage / 100.0f;

		FlickerGage.SetActive ( InGameParameter.CharacterChangeGage >= 100 );
		Bomb.SetActive ( InGameParameter.CharacterHasBomb || InGameParameter.IsCharacterChanged );

		Sprite powerLevelSprite = null;
		int powerLevel = InGameParameter.CharacterPowerLevel;
		if ( InGameParameter.IsCharacterChanged )
			powerLevel = 5;
		switch ( powerLevel )
		{
			case 1: powerLevelSprite = Lv1; break;
			case 2: powerLevelSprite = Lv2; break;
			case 3: powerLevelSprite = Lv3; break;
			case 4: powerLevelSprite = Lv4; break;
			case 5: powerLevelSprite = Lv5; break;
		}
		PowerLevel.GetComponent<SpriteRenderer> ().sprite = powerLevelSprite;

		PointFont.GetComponent<ShFont> ().Value = InGameParameter.CurrentPoint;

		if ( InGameParameter.CharacterHitPoint <= 0 )
			GameOverOverlay.SetActive ( true );
	}

	public void DoBomb ()
	{
		if ( !InGameParameter.CharacterHasBomb )
			if ( !InGameParameter.IsCharacterChanged )
				return;

		GetComponent<AudioSource> ().Play ();
		GameObject.Find ( "Character" ).GetComponent<CharBulletEmitter> ().SendMessage ( "DoBomb" );
	}

	public void DoChange ()
	{
		if ( InGameParameter.CharacterChangeGage < 100 ) return;

		GameObject.Find ( "Character" ).GetComponent<CharController> ().SendMessage ( "DoChange" );
	}

	public void GoToMenu ()
	{
		Time.timeScale = 1;
		Initiate.Fade ( "MenuScene", Color.black, 0.8f );
	}
}
