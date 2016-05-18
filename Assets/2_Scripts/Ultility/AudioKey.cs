using UnityEngine;
using System.Collections;

public class AudioKey {
    private static AudioKey instance = new AudioKey();
    public enum UNIQUE_KEY {
		// all game
		CORRECT_WORD,
		INCORRECT_WORD,

		// car game
		CARGAME_COLLECT_LETTER,
		CARGAME_DROP_LETTER,

		// gui
		BUTTON_CLICK

    };


    private AudioKey() {

    }
}
