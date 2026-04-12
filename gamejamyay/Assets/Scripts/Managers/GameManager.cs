using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
        Debug.Log($"State: {curState} -> {newState}");
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
        // Debug.Log($"Awake called, Instance is null: {Instance == null}");
        // if (Instance != null && Instance != this){
        //     Debug.Log("test :D");
        //     Destroy(gameObject);    // kill duplicates
        //     return;
        // }
        Instance = this;

    

        //Debug.Log($"Minigames in pool: {minigames.Length}");
        foreach (var mg in minigames)
           Debug.Log($" - {mg.Title} active : ({mg.gameObject.activeSelf})");
    }

    [Header("Game Settings")]
    public int startingLives = 5;
    public float introDuration = 1.5f;
    public int roundsPerSpeedTier = 5;
    public float[] speedMults = { 1f, 1.3f, 1.6f, 2f };

    [Header("Minigame Pool")]
    public MinigameBase[] minigames;

    // temporary
    [Header("UI")]
    public GameObject StartButton;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI scoreText;
    public Slider timerSlider;

    private MinigameBase activeMinigame;

    private int lives;
    private int round;
    private int score;
    private int speedTier;
    private float timer;
    private bool timerRunning;
    private string lastPlayedGame;  // no repeats (fingers crossed)
    private void Start()
    {
        StartGame();
    }
    public void StartGame(){
        Debug.Log("StartGame called");

        // temp bug fix
        // minigames = FindObjectsByType<MinigameBase>(FindObjectsSortMode.None);
        Debug.Log($"Minigames found: {minigames.Length}");

        //StartButton.SetActive(false);     // add back once debugging is over (unless, yknow, if it aint broke...)
        lives = startingLives;
        round = 0;
        score = 0;
        speedTier = 0;


        //SetState(MGState.IDLE);
        UpdateUI();     // test
        StartNextRound();
        
    }

    void StartNextRound(){
        round++;

        // calculate speed tier
        speedTier = Mathf.Min(round / roundsPerSpeedTier, speedMults.Length - 1);

        UpdateUI();     // test

        MinigameBase chosen = PickMinigame();
        ActivateMinigame(chosen);

        //SetState(MGState.INTRO);
    }

    // testing ui
    void UpdateUI(){
        if (livesText != null) livesText.text = $"Lives: {lives}";
        if (roundText != null) roundText.text = $"Round: {round}";
        if (scoreText != null) scoreText.text = $"Score: {score}"; 
    }

    MinigameBase PickMinigame(){
        MinigameBase[] choices = minigames.Where(m => m != activeMinigame).ToArray();

        // if all minigames were filtered out, use full pool
        if (choices.Length == 0)
            choices = minigames;

        return choices[Random.Range(0, choices.Length)];
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

        if (timerSlider != null)
            timerSlider.value = timer / (activeMinigame.BaseTimeLimit / speedMults[speedTier]); 

        if (timer <= 0){
            timerRunning = false;
            activeMinigame.ForceFailure();  // times up 
        }
    }

    void HandleIdle(){
        StartNextRound();
    }

    void HandleIntro(){
        StartCoroutine(IntroThenPlay());
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
        Debug.Log($"Round {round} complete - success: {success}, lives {lives}, score: {score}");

        timerRunning = false;
        //activeMinigame.OnMGComplete -= HandleMGComplete;

        if (success) score++;
        else lives--;

        UpdateUI();     // test
        SetState(MGState.RESULT);
    }

    void HandleResult(){
        StartCoroutine(HandleResultRoutine());
    }

    IEnumerator HandleResultRoutine(){
        // show success/failure
        yield return new WaitForSeconds(1.5f);

        DeactivateMinigame();
        StopAllCoroutines();

        if (lives <= 0)
            SetState(MGState.GAMEOVER);
        else
            StartNextRound();
    }

    void ActivateMinigame(MinigameBase minigame){
        Debug.Log($"Activating: {minigame.Title}");
        activeMinigame = minigame;
        activeMinigame.gameObject.SetActive(true);
        activeMinigame.ResetMinigame();

        activeMinigame.OnMGComplete -= HandleMGComplete;
        activeMinigame.OnMGComplete += HandleMGComplete;

        SetState(MGState.INTRO);
    }

    void DeactivateMinigame(){
        activeMinigame.OnMGComplete -= HandleMGComplete;
        activeMinigame.gameObject.SetActive(false);
        activeMinigame = null;
    }



}
