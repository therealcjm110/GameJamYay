using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RattleMinigame : MinigameBase
{
    [Header("Rattle Settings")]
    public int rattlesRequired = 4;
    public float minRattleDistance = 80f;

    [Header("References")]
    // public Image charImage;     // image to swap
    public Sprite leftSprite;   // left sprite
    public Sprite rightSprite;  // right sprite
    public TextMeshProUGUI rattleCountText;

    private int rattleCount = 0;
    private float lastX;
    private float rattleOriginX;
    private int curDirection;
    private bool started = false;
    private bool minigameComplete = false;

    void Update(){
        if (minigameComplete) return;

        float mouseX = Input.mousePosition.x;

        // get starting pos on first frame of movement
        if (!started){
            if (Mathf.Abs(mouseX - lastX) > 0.1f){
                started = true;
                rattleOriginX = mouseX;
                curDirection = mouseX > lastX ? 1 : -1;
            }
            lastX = mouseX;
            return;
        }

        int newDirection = mouseX > lastX ? 1 : -1;

        // detect direction reversal
        if (newDirection != curDirection){
            SwapSprite(newDirection);

            float rattleDistance = Mathf.Abs(mouseX - rattleOriginX);

            if (rattleDistance >= minRattleDistance){
                rattleCount++;
                rattleOriginX = mouseX;    // reset rattle origin to cur pos
                UpdateUI();

                if (rattleCount >= rattlesRequired)
                    CompleteMinigame();
            }

            curDirection = newDirection;
        }

        lastX = mouseX;
    }

    void SwapSprite(int direction){
        if (leftSprite == null || rightSprite == null){
            Debug.LogWarning("images not assigned! idiot!");
            return;
        }
        leftSprite.SetActive(direction < 0);
        rightSprite.SetActive(direction > 0);
    }

   void UpdateUI(){
        if (rattleCountText != null)
            rattleCountText.text = $"rattles: {rattleCount} / {rattlesRequired}";
    }

    void CompleteMinigame(){
        minigameComplete = true;
        //if (statusText != null)
        //    statusText.text = "Done! :D";
        Debug.Log("rattle MG complete!");
        Complete(true);
    }

    public override void StartMinigame(float speedMult){
        rattlesRequired = Mathf.RoundToInt(3 * speedMult);
        minRattleDistance = 80f / speedMult;   // maybe not needed?
    }

    public override void ResetMinigame(){
        rattleCount = 0;
        started = false;
        minigameComplete = false;

        if (leftSprite != null)
            leftSprite.SetActive(true);
        if (rightSprite != null) 
            rightSprite.SetActive(false);

        UpdateUI();
    }
}
