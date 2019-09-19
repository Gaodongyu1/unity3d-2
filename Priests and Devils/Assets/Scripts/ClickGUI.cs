using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Engine;

public class ClickGUI : MonoBehaviour {

	// Use this for initialization
	User_action action;
	Character_model character;

	public void setController(Character_model tem){
		character = tem;
	}
	void Start(){
		action = Director.get_Instance().curren as User_action;
	}
	void OnMouseDown(){
		if (gameObject.name == "boat") {
			action.moveboat ();
		} else {
			action.isClickChar (character);
		}
	}
}