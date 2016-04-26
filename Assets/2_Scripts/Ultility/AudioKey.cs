using UnityEngine;
using System.Collections;

public class AudioKey {
    private static AudioKey instance = new AudioKey();
    public enum UNIQUE_KEY {
        FOOTSTEP, BABY_CRYING, PLAYER_BREATH, PARENT_CALL, PLAYER_JUMP, PLAYER_DIE, PLAYER_SLIDE, PROXY_SCREAM, PLAYER_DOUBLE_SCREAM, CAMERA_NOISE, FEMALE_TAKING_DAMAGE, COLLISION, BUTTON_CLICK, MENU_BACKGROUND, DOG_ATTACK, PROXY_SMILE, DOG_PAIN, NONE, BOOST_INVINCIBLE, BOOST_LIGHT
    };


    private AudioKey() {

    }
}
