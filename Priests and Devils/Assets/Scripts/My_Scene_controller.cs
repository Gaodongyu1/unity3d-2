using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Engine;

public class My_Scene_controller : MonoBehaviour, Scene_controller, User_action{
	readonly Vector3 water_pos = new Vector3 (0, 0.5f, 0);
	UserGUI user;
	public Coast_model fromCoast;
	public Coast_model toCoast;
	public Boat_model boat;
	private List<Character_model> team;

	void Awake(){
		Director director = Director.get_Instance();
		director.curren = this;
		user = gameObject.AddComponent<UserGUI>() as UserGUI;
		team = new List<Character_model>();
		loadResources();
	}

	public void loadResources(){
		GameObject water = Instantiate (Resources.Load ("Prefabs/water", typeof(GameObject)), water_pos, Quaternion.identity, null) as GameObject;
		water.name = "water";
		fromCoast = new Coast_model ("from");
		toCoast = new Coast_model ("to");
		boat = new Boat_model ();
		for (int i = 0; i < 3; i++) {
			Character_model tem = new Character_model ("priest");
			tem.setName ("priest" + i);
			tem.setPosition (fromCoast.getEmptyPosition ());
			tem.getOnCoast (fromCoast);
			fromCoast.getOnCoast (tem);
			team.Add (tem);
		}
		for (int i = 0; i < 3; i++) {
			Character_model tem = new Character_model ("devil");
			tem.setName ("devil" + i);
			tem.setPosition (fromCoast.getEmptyPosition ());
			tem.getOnCoast (fromCoast);
			fromCoast.getOnCoast (tem);
			team.Add (tem);
		}
	}

	public void moveboat(){
		if (boat.IfEmpty ())
			return;
		boat.boatMove ();
		user.if_win_or_not = checkGameOver();
	}

	public void isClickChar (Character_model tem_char){
		if (Move_model.can_move == 1)
			return;
		if (tem_char._isOnBoat ()) {
			Coast_model tem_coast;
			if (boat.getflag() == -1) {
				tem_coast = toCoast;
			} 
            else {
				tem_coast = fromCoast;
			}
			boat.getOffBoat (tem_char.getName ());
			tem_char.moveToPosition (tem_coast.getEmptyPosition ());
			tem_char.getOnCoast (tem_coast);
			tem_coast.getOnCoast (tem_char);
		} 
        else {
			Coast_model tem_coast2 = tem_char.getcoastmodel();
			if (boat.getEmptyIndex () == -1)
				return;
			if (boat.getflag() != tem_coast2.getflag())
				return;
			tem_coast2.getOffCoast (tem_char.getName());
			tem_char.moveToPosition (boat.getEmptyPosition ());
			tem_char.getOnBoat (boat);
			boat.getOnBoat (tem_char);
		}
		user.if_win_or_not = checkGameOver();
	}

	public void restart(){
		boat.reset ();
		fromCoast.reset ();
		toCoast.reset ();
		foreach (Character_model i in team) {
			i.reset ();
		}
		Move_model.can_move = 0;
	}

	public void pause(){
		boat.Mypause ();
		foreach (Character_model i in team) {
			i.Mypause();
		}
	}

	public void Coninu (){
		boat.MyConti ();
		foreach (Character_model i in team) {
			i.MyConti();
		}
	}
    
	private int checkGameOver(){
		if (Move_model.can_move == 1)
			return 0;
		int from_priest = 0;
		int from_devil = 0;
		int to_priest = 0;
		int to_devil = 0;

		int[] from_count = fromCoast.getCharacterNum ();
		from_priest = from_count [0];
		from_devil = from_count [1];

		int[] to_count = toCoast.getCharacterNum ();
		to_priest = to_count [0];
		to_devil = to_count [1];

		if (to_devil + to_priest == 6)
			return 1;//you win
		int[] boat_count = boat.getCharacterNum();
		if (boat.getflag() == 1) {
			from_priest += boat_count [0];
			from_devil += boat_count [1];
		} else {
			to_priest += boat_count [0];
			to_devil += boat_count [1];
		}
		if (from_priest < from_devil && from_priest > 0)
			return -1;//you lose
		if(to_priest < to_devil && to_priest > 0)
			return -1;//you lose
		return 0;//not yet finish
	}

}