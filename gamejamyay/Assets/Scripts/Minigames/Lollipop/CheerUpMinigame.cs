using UnityEngine;

public class CheerUpMinigame : MinigameBase
{
    [Header("Creature Sprites")]
    public GameObject sadCreature;
    public GameObject happyCreature;

    [Header("Cheer Card")]
    public GameObject cheerCard;
    public float cheerCardDelay = 1f;

    [Header("Lollipop")]
    public GameObject lollipop;

    private Vector3 lollipopStartPos;
    private bool startPosSaved = false;
    private bool completed = false;

    public override void StartMinigame(float speedMult)
    {
        completed = false;

        if (!startPosSaved && lollipop != null)
        {
            lollipopStartPos = lollipop.transform.position;
            startPosSaved = true;
        }

        if (lollipop != null)
            lollipop.SetActive(true);
    }

    public override void ResetMinigame()
    {
        completed = false;

        if (sadCreature != null) sadCreature.SetActive(true);
        if (happyCreature != null) happyCreature.SetActive(false);
        if (cheerCard != null) cheerCard.SetActive(false);

        if (lollipop != null)
        {
            lollipop.SetActive(false);
            if (startPosSaved)
                lollipop.transform.position = lollipopStartPos;
        }
    }

    public void OnLollipopFed()
    {
        if (completed) return;
        completed = true;

        if (sadCreature != null) sadCreature.SetActive(false);
        if (happyCreature != null) happyCreature.SetActive(true);
        if (lollipop != null) lollipop.SetActive(false);

        StartCoroutine(ShowCardThenComplete());
    }

    System.Collections.IEnumerator ShowCardThenComplete()
    {
        yield return new WaitForSeconds(cheerCardDelay);
        if (cheerCard != null) cheerCard.SetActive(true);
        yield return new WaitForSeconds(1f);
        Complete(true);
    }
}