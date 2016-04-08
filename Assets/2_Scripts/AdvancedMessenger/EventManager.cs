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
		TOGGLE_MUSIC
	}

	//
	public enum AR {
		LETTERTRACKING,
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
		COLLECT,
		GATHER
	}
}
