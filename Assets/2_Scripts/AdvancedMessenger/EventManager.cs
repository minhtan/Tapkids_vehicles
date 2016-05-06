using UnityEngine;
using System.Collections;

public class EventManager {
	public enum GUI{
		NOTIFY,
		NEXT,
		//
		ENTER_GARAGE,
		EXIT_GARAGE,
		SELECT_VEHICLE,
		UPDATE_VEHICLE,
		PURCHASE_VEHICLE,
		//
		UPDATE_CREDIT,
		//
		TOGGLE_INGAME,
		TOGGLE_TUTORIAL,
		//
		TOGGLE_SOUND,
		TOGGLE_MUSIC,
		//
//		ADD_LETTER,
		REMOVE_LETTER,
		//
		DROPBUTTON,
		// 
		COUNTDOWN,
		MENU_WHEEL_TURN,
		MENU_WHEEL_RELEASE,
		MENU_WHEEL_FRAME_TURN
	}

	//
	public enum AR {
		LETTER_TRACKING,
		VEHICLE_TRACKING,
		MAP_TRACKING
	}

	public enum GameState {
		INIT,
		START,
		PAUSE,
		RESET,
		GAMEOVER,
		EXIT_TO_MENU
	}

	public enum Vehicle {
		COLLECT_LETTER,
		DROP_LETTER,
		GATHER_LETTER
	}
}
