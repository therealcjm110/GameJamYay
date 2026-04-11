using UnityEngine;


public enum MGState{
    IDLE,       // 
    INTRO,      // show instructions (wag, drag, etc)
    PLAYING,    // playing mg
    RESULT,     // show win/loss
    GAMEOVER    // self explanatory
}

public class GameManager : MonoBehaviour
{
    public int lives;
    public int score;
    public int round;
}
