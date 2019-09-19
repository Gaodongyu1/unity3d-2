using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Engine;

public class UserGUI : MonoBehaviour {
	private User_action action;
	private GUIStyle MyStyle;
	private GUIStyle MyButtonStyle;
	public int if_win_or_not;

	void Start(){
		action = Director.get_Instance ().curren as User_action;

		MyStyle = new GUIStyle ();
		MyStyle.fontSize = 40;
		MyStyle.normal.textColor = new Color (255f, 0, 0);
		MyStyle.alignment = TextAnchor.MiddleCenter;

		MyButtonStyle = new GUIStyle ("button");
		MyButtonStyle.fontSize = 30;
	}

	void reStart(){
		if (GUI.Button (new Rect (Screen.width/2-Screen.width/8, Screen.height/2+100, 150, 50), "Restart", MyButtonStyle)) {
			if_win_or_not = 0;
			action.restart ();
			Move_model.can_move = 0;
		}
	}

	void IsPause(){
		if (GUI.Button (new Rect (Screen.width / 2 - 350, Screen.height / 2 + 100, 150, 50), "Pause", MyButtonStyle)) {
			if (Move_model.can_move == 0) {
				action.pause ();
				Move_model.can_move = 1;
			} 
		} else if (GUI.Button (new Rect (Screen.width-Screen.width/3, Screen.height / 2 + 100, 150, 50), "Continue", MyButtonStyle)) {
			if (Move_model.can_move == 1) {
				action.Coninu();
				Move_model.can_move = 0;
			}
		}
	}

	void OnGUI(){
		IsPause ();
		reStart ();
		if(Move_model.can_move == 1)
			GUI.Label (new Rect (Screen.width/2-Screen.width/8, 50, 100, 50), "Pausing", MyStyle);
		if (if_win_or_not == -1) {
			GUI.Label (new Rect (Screen.width/2-Screen.width/8, 50, 100, 50), "Game Over", MyStyle);
			IsPause ();
			reStart ();
		}
        else if (if_win_or_not == 1) {
			GUI.Label (new Rect (Screen.width/2-Screen.width/8, 50, 100, 50), "You Win", MyStyle);
			IsPause ();
			reStart ();
		}
	}
}