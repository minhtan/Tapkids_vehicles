﻿using UnityEngine;
using System.Collections;

public class EventManager {
	public enum GUI{
		NOTIFY,
		SHOWSUGGESTION,
		NEXT,
		//
		TO_GARAGE,
		TO_MENU,
		SELECT_VEHICLE,
		UPDATE_VEHICLE,
		PURCHASE_VEHICLE,
		CHANGE_MATERIAL,
		//
		UPDATE_CREDIT,
		//
		TOGGLE_MENU_BTN,
		TOGGLE_PLAYER_PNL,
		TOGGLE_SFX_BTN,
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
		WHEEL_FRAME_TURN,
		MENU_BTN_TAP,
		MENU_BTN_DOWN,
		MENU_BTN_UP,
		MENU_BTN_HOLD
	}

	//
	public enum AR {
		LETTER_IMAGE_TRACKING,
		VEHICLE_IMAGE_TRACKING,
		MAP_IMAGE_TRACKING
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
