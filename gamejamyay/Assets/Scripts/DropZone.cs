using UnityEngine;

public class DropZone : MonoBehaviour
{
    public string correctTag = "Item";
    public DragDropMinigame minigame;

    private bool completed = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (completed) return;

        if (other.CompareTag(correctTag))
        {
            completed = true;
            Debug.Log(other.name + " dropped in zone");

            if (minigame != null)
                minigame.OnItemDropped();
        }
    }

    public void Reset()
    {
        completed = false;
    }
}