using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum MGState{
    IDLE,       // 
    INTRO,      // show instructions (wag, drag, etc)
    PLAYING,    // playing mg
    RESULT,     // show win/loss
    GAMEOVER    // self explanatory
}

public class GameManager : MonoBehaviour
{

    private MGState curState;

    public static GameManager Instance { get; private set; }

    void SetState(MGState newState){
    curState = newState;
    switch(newState){
        case MGState.IDLE:
            HandleIdle();
            break;
        case MGState.INTRO:
            HandleIntro();
            break;
        case MGState.PLAYING:
            HandlePlaying();
            break;
        case MGState.RESULT:
            HandleResult();
            break;
        case MGState.GAMEOVER:
            HandleGameover();
            break;
    }
}

    void Awake(){
        if (Instance != null && Instance != this){
            Destroy(gameObject);    // kill duplicates
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);  // may not need for one scene
    }

    [Header("Game Settings")]
    public int startingLives = 5;
    public float introDuration = 1.5f;
    public int roundsPerSpeedTier = 5;
    public float[] speedMults = { 1f, 1.3f, 1.6f, 2f };

    [Header("Minigame Pool")]
    public MinigameBase[] minigames;

    private MinigameBase activeMinigame;

    private int lives;
    private int round;
    private int score;
    private int speedTier;
    private float timer;
    private bool timerRunning;
    private string lastPlayedGame;  // no repeats (fingers crossed)

    public void StartGame(){
        lives = startingLives;
        round = 0;
        score = 0;
        speedTier = 0;
        SetState(MGState.IDLE);
        StartNextRound();
    }

    void StartNextRound(){
        round++;

        // calculate speed tier
        speedTier = Mathf.Min(round / roundsPerSpeedTier, speedMults.Length - 1);

        MinigameBase chosen = PickMinigame();
        ActivateMinigame(chosen);

        //SetState(MGState.INTRO);
    }

    MinigameBase PickMinigame(){
        MinigameBase[] choices = minigames.Where(m => m != activeMinigame).ToArray();

        return choices[Random.Range(0, choices.Length)];
    }

    public void RegisterMinigame(MinigameBase minigame){
        activeMinigame = minigame;
        activeMinigame.OnMGComplete += HandleMGComplete;
        // proceed intro
        StartCoroutine(IntroThenPlay());
    }

    IEnumerator IntroThenPlay(){
        // show minigame transition
        yield return new WaitForSeconds(introDuration);
        SetState(MGState.PLAYING);
    }

    void Update(){
        if (!timerRunning) return;

        timer -= Time.deltaTime;
        // update timer bar

        if (timer <= 0){
            timerRunning = false;
            activeMinigame.ForceFailure();  // times up 
        }
    }

    void HandleIdle(){
        
    }

    void HandleIntro(){
        // screen transition
    }

    void HandleGameover(){
        timerRunning = false;
        // show game over screen
        Debug.Log("Game over!");
    }

    void HandlePlaying(){
        float timeLimit = activeMinigame.BaseTimeLimit / speedMults[speedTier];
        timer = timeLimit;
        timerRunning = true;

        activeMinigame.StartMinigame(speedMults[speedTier]);
    }

    void HandleMGComplete(bool success){
        timerRunning = false;
        //activeMinigame.OnMGComplete -= HandleMGComplete;

        if (success) score++;
        else lives--;

        SetState(MGState.RESULT);
    }

    IEnumerator HandleResult(){
        // show success/failure
        yield return new WaitForSeconds(1.5f);

        DeactivateMinigame();

        if (lives <= 0)
            SetState(MGState.GAMEOVER);
        else
            StartNextRound();
    }

    void ActivateMinigame(MinigameBase minigame){
        activeMinigame = minigame;
        activeMinigame.gameObject.SetActive(true);
        activeMinigame.ResetMinigame();
        activeMinigame.OnMGComplete += HandleMGComplete;
        StartCoroutine(IntroThenPlay());
    }

    void DeactivateMinigame(){
        activeMinigame.OnMGComplete -= HandleMGComplete;
        activeMinigame.gameObject.SetActive(false);
        activeMinigame = null;
    }



}
