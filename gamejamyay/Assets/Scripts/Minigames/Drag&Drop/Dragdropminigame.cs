using UnityEngine;

public class DragDropMinigame : MinigameBase
{
    [Header("Drag Item")]
    public GameObject dragItem;

    [Header("Win Card (optional)")]
    public GameObject winCard;
    public float winCardDelay = 1f;

    private Vector3 itemStartPos;
    private bool startPosSaved = false;
    private bool completed = false;

    public override void StartMinigame(float speedMult)
    {
        completed = false;
        if (!startPosSaved && dragItem != null)
        {
            itemStartPos = dragItem.transform.position;
            startPosSaved = true;
        }
    }

    public override void ResetMinigame()
    {
        completed = false;

        if (dragItem != null)
        {
            dragItem.SetActive(true);
            if (startPosSaved)
                dragItem.transform.position = itemStartPos;
        }

        if (winCard != null)
            winCard.SetActive(false);
    }

    public void OnItemDropped()
    {
        if (completed) return;
        completed = true;

        Debug.Log("DragDrop minigame complete!");
        StartCoroutine(WinRoutine());
    }

    System.Collections.IEnumerator WinRoutine()
    {
        if (winCard != null)
        {
            yield return new WaitForSeconds(winCardDelay);
            winCard.SetActive(true);
            yield return new WaitForSeconds(1f);
        }
        Complete(true);
    }
}