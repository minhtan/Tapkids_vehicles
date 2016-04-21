using UnityEngine;
using System.Collections;

public class EventManager {
	public enum GUI{
		NOTIFY,
		SELECTCAR,
		TOGGLE_INGAME,
		TOGGLE_TUTORIAL,
		//
		TOGGLE_SOUND,
		TOGGLE_MUSIC,
		//
		ADDLETTER,
		REMOVELETTER,
		//
		DROPBUTTON
	}

	//
	public enum AR {
		LETTERTRACKING,
		IMAGETRACKING,
		MAPTRACKING
	}

	public enum GameState {
		INITGAME,
		STARTGAME,
		PAUSEGAME,
		RESETGAME,
		GAMEOVER
	}

	public enum Vehicle {
		COLLECTLETTER,
		DROPLETTER,
		GATHERLETTER
	}
}
