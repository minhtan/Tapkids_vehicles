using UnityEngine;
using System.Collections;

public class AudioKey {
    private static AudioKey instance = new AudioKey();
    public enum UNIQUE_KEY {
		BACKGROUD,

		// all game
		CORRECT_WORD,
		INCORRECT_WORD,

		// car game
		CARGAME_COLLECT_LETTER,
		CARGAME_DROP_LETTER,

		// gui
		BUTTON_CLICK,
		COUNTDOWN,
		TIMEOUT,
		GAME_WIN,
		GAME_LOSE,


		// AR
		SCAN_LETTER,
		SCAN_MAP
    };

	private AudioKey() {

    }
}
