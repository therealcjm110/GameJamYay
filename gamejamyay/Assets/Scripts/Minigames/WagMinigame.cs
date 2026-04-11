using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WagMinigame : MinigameBase
{

    [Header("Wag Settings")]
    public int wagsRequired = 3;
    public float minWagDistance = 80f;

    [Header("UI")]
    public TextMeshProUGUI wagCountText;
    public TextMeshProUGUI statusText;

    //public string Title = "Wag";

    private int wagCount = 0;
    private float lastX;
    private float wagOriginX;   // where wag started
    private int curDirection;   // +1 = moving right, -1 moving left
    private bool minigameComplete = false;
    private bool started = false;

    void Update(){
        if (minigameComplete) return;

        float mouseX = Input.mousePosition.x;

        // get starting pos on first frame of movement
        if (!started){
            if (Mathf.Abs(mouseX - lastX) > 0.1f){
                started = true;
                wagOriginX = mouseX;
                curDirection = mouseX > lastX ? 1 : -1;
            }
            lastX = mouseX;
            return;
        }

        int newDirection = mouseX > lastX ? 1 : -1;

        // detect direction reversal
        if (newDirection != curDirection){
            float wagDistance = Mathf.Abs(mouseX - lastX);

            if (wagDistance >= minWagDistance){
                wagCount++;
                wagOriginX = mouseX;    // reset wag origin to cur pos
                UpdateUI();

                if (wagCount >= wagsRequired)
                    CompleteMinigame();
            }

            curDirection = newDirection;
        }

        lastX = mouseX;
    }

    void UpdateUI(){
        if (wagCountText != null)
            wagCountText.text = $"Wags: {wagCount} / {wagsRequired}";
    }

    void CompleteMinigame(){
        minigameComplete = true;
        if (statusText != null)
            statusText.text = "Done! :D";
        Debug.Log("Wag MG complete!");
        Complete(true);
    }

    // call from gamemanager to reset between rounds
    public void ResetWagDetector(int newWagsRequired, float newMinDistance){
        wagsRequired = newWagsRequired;
        minWagDistance = newMinDistance;
        wagCount = 0;
        started = false;
        minigameComplete = false;
        lastX = Input.mousePosition.x;
        UpdateUI();
    }

    public override void StartMinigame(float speedMult){
        wagsRequired = Mathf.RoundToInt(3 * speedMult);
        minWagDistance = 80f / speedMult;   // maybe not needed?
    }

    public override void ResetMinigame(){
        wagCount = 0;
        started = false;
        minigameComplete = false;
    }

    void OnWagComplete() => Complete(true);

}
