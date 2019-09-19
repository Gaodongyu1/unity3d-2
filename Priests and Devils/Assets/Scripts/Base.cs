using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Engine
{
	public class Director: System.Object
	{
		private static Director director;
		public Scene_controller curren{ get; set;}
		public static Director get_Instance(){
			if (director == null)
			{
				director = new Director();
			}
			return director;
		}
	}

	public interface Scene_controller
	{
		void loadResources();
	}

	public interface User_action {
		void moveboat();
		void isClickChar (Character_model tem_char);
		void restart();
		void pause();
		void Coninu();
	}

	public class Move_model : MonoBehaviour
	{
		readonly float move_speed = 20;
		private int move_to;//0->not move, 1->to middle, 2->to destination
		private Vector3 dest;
		private Vector3 middle;
		public static int can_move = 0;//0->can move, 1->cant move

		void Update(){
			if (can_move == 1)
				return;
			else{
				if(move_to == 1){
					transform.position = Vector3.MoveTowards(transform.position, middle, move_speed*Time.deltaTime);
					if (transform.position == middle)
						move_to = 2;
				}
				else if(move_to == 2){
					transform.position = Vector3.MoveTowards(transform.position, dest, move_speed*Time.deltaTime);
					if (transform.position == dest)
						move_to = 0;
				}
			}
		}

		public void SetDestination(Vector3 _dest){
			if (can_move == 1)
				return;
			else{
				middle = _dest;
				dest = _dest;
				if (_dest.y < transform.position.y) {
					middle.y = transform.position.y;
				} else {
					middle.x = transform.position.x;
				}
				move_to = 1;
			}
		}
		public void reset(){
			if (can_move == 1)
				return;
			else{
				move_to = 0;
			}
		}
	}
	
    public class Character_model{
		readonly GameObject character;
		readonly Move_model  Cmove;
		readonly ClickGUI clickgui;
		readonly int Ctype;//0->priset, 1->devil
		private bool isOnboat;
		private Coast_model coastmodel;

		public Character_model(string Myname){
			if(Myname == "priest"){
				character = Object.Instantiate(Resources.Load("Prefabs/priest", typeof(GameObject)), new Vector3(6,0,0), Quaternion.identity,null) as GameObject;
				Ctype = 0;
			}
			else{
				character = Object.Instantiate(Resources.Load("Prefabs/devil", typeof(GameObject)), new Vector3(6,0,0), Quaternion.identity,null) as GameObject;
				Ctype = 1;
			}
			Cmove = character.AddComponent(typeof(Move_model )) as Move_model ;
			clickgui = character.AddComponent(typeof(ClickGUI)) as ClickGUI;
			clickgui.setController(this);
		}
		public int getType(){
			return Ctype;
		}
		public void setName(string name){
			character.name = name;
		}
		public string getName(){
			return character.name;
		}
		public void setPosition(Vector3 postion){
			character.transform.position = postion;
		}
		public void moveToPosition(Vector3 _dest){
			Cmove.SetDestination (_dest);
		}
		public void getOnBoat(Boat_model tem_boat){
			coastmodel = null;
			character.transform.parent = tem_boat.getGameObject ().transform;
			isOnboat = true;
		}
		public void getOnCoast(Coast_model coastCon){
			coastmodel = coastCon;
			character.transform.parent = null;
			isOnboat = false;
		}
		public bool _isOnBoat(){
			return isOnboat;
		}
		public Coast_model getcoastmodel(){
			return coastmodel;
		}
		public void reset(){
			Cmove.reset ();
			coastmodel = (Director.get_Instance().curren as My_Scene_controller).fromCoast;
			getOnCoast(coastmodel);
			setPosition (coastmodel.getEmptyPosition());
			coastmodel.getOnCoast (this);
		}
		public void Mypause(){
			Move_model.can_move = 1;
		}
		public void MyConti(){
			Move_model.can_move = 0;
		}
	}

	public class Coast_model{
		readonly GameObject coast;
		readonly Vector3 from_pos = new Vector3(6,1,0);
		readonly Vector3 to_pos = new Vector3(-6,1,0);
		readonly Vector3[] postion;
		readonly int flag;//-1->to, 1->from

		private Character_model[] passengerPlaner;

		public Coast_model(string to_or_from){
			postion = new Vector3[] {
				new Vector3 (3.5f, 2.25f, 0),
				new Vector3 (4.5f, 2.25f, 0),
				new Vector3 (5.5f, 2.25f, 0),
				new Vector3 (6.5f, 2.25f, 0),
				new Vector3 (7.5f, 2.25f, 0),
				new Vector3 (8.5f, 2.25f, 0)
			};
			passengerPlaner = new Character_model[6];
			if(to_or_from == "from"){
				coast = Object.Instantiate(Resources.Load("Prefabs/coast", typeof(GameObject)), from_pos, Quaternion.identity, null) as GameObject;
				coast.name = "from";
				flag = 1;
			}
			else{
				coast = Object.Instantiate(Resources.Load("Prefabs/coast", typeof(GameObject)), to_pos, Quaternion.identity, null) as GameObject;
				coast.name = "to";
				flag = -1;
			}
		}
		public int getflag(){
			return flag;
		}
		public Character_model getOffCoast(string object_name){
			for(int i = 0; i < passengerPlaner.Length; i++){
				if(passengerPlaner[i] != null && passengerPlaner[i].getName() == object_name){
					Character_model myCharacter = passengerPlaner[i];
					passengerPlaner[i] = null;
					return myCharacter;
				}
			}
			return null;
		}
		public int getEmptyIndex(){
			for(int i = 0; i < passengerPlaner.Length; i++){
				if(passengerPlaner[i] == null){
					return i;
				}
			}
			return -1;
		}
		public Vector3 getEmptyPosition(){
			int index = getEmptyIndex();
			Vector3 pos = postion[index];
			pos.x *= flag;
			return pos;
		}
		public void getOnCoast(Character_model myCharacter){
			int index = getEmptyIndex();
			passengerPlaner[index] = myCharacter;
		}
		public int[] getCharacterNum(){
			int[] count = {0,0};
			for(int i = 0; i < passengerPlaner.Length; i++){
				if(passengerPlaner[i] == null) continue;
				if(passengerPlaner[i].getType() == 0) count[0]++;
				else count[1]++;
			}
			return count;
		}
		public void reset(){
			passengerPlaner = new Character_model[6];
		}
	}
	
	public class Boat_model{
		readonly GameObject boat;
		readonly Move_model  Cmove;
		readonly Vector3 fromPos = new Vector3 (3, 1, 0);
		readonly Vector3 toPos = new Vector3 (-3, 1, 0);
		readonly Vector3[] from_pos;
		readonly Vector3[] to_pos;
		private int flag;//-1->to, 1->from
		private Character_model[] passenger = new Character_model[2];

		public Boat_model(){
			flag = 1;
			from_pos = new Vector3[]{ new Vector3 (4.5f, 1.5f, 0), new Vector3 (5.5f, 1.5f, 0) };
			to_pos = new Vector3[]{ new Vector3 (-5.5f, 1.5f, 0), new Vector3 (-4.5f, 1.5f, 0) };
			boat = Object.Instantiate (Resources.Load ("Prefabs/boat", typeof(GameObject)), fromPos, Quaternion.identity, null) as GameObject;
			boat.name = "boat";
			Cmove = boat.AddComponent (typeof(Move_model )) as Move_model ;
			boat.AddComponent (typeof(ClickGUI));
		}
		public void boatMove(){
			if (flag == 1) {
				Cmove.SetDestination (toPos);
				flag = -1;
			} else {
				Cmove.SetDestination (fromPos);
				flag = 1;
			}
		}
		public void getOnBoat(Character_model tem_cha){
			int index = getEmptyIndex ();
			passenger [index] = tem_cha;
		}
		public Character_model getOffBoat(string object_name){
			for (int i = 0; i < passenger.Length; i++) {
				if (passenger [i] != null && passenger [i].getName () == object_name) {
					Character_model temp_character = passenger [i];
					passenger [i] = null;
					return temp_character;
				}
			}
			return null;
		}
		public int getEmptyIndex(){
			for (int i = 0; i < passenger.Length; i++) {
				if (passenger [i] == null)
					return i;
			}
			return -1;
		}
		public bool IfEmpty(){
			for (int i = 0; i < passenger.Length; i++) {
				if (passenger [i] != null)
					return false;
			}
			return true;
		}
		public Vector3 getEmptyPosition(){
			Vector3 pos;
			int index = getEmptyIndex ();
			if (flag == 1) {
				pos = from_pos [index];
			}
            else {
				pos = to_pos [index];
			}
			return pos;
		}
		public GameObject getGameObject(){
			return boat;
		}
		public int getflag(){
			return flag;
		}
		public int[] getCharacterNum(){
			int[] count = { 0, 0 };
			for (int i = 0; i < passenger.Length; i++) {
				if (passenger [i] == null)
					continue;
				if (passenger [i].getType () == 0) {
					count [0]++;
				} 
                else {
					count [1]++;
				}
			}
			return count;
		}
		public void reset(){
			Cmove.reset ();
			if (flag == -1) {
				boatMove ();
			}
			passenger = new Character_model[2];
		}
		public void Mypause(){
			Move_model .can_move = 1;
		}
		public void MyConti(){
			Move_model .can_move = 0;
		}
	}
}