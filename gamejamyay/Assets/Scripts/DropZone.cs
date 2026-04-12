using UnityEngine;

public class DropZone : MonoBehaviour
{
    public string correctTag = "Item";
    public PuzzleManager puzzleManager;

    private bool completed = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (completed) return;

        if (other.CompareTag(correctTag))
        {
            completed = true;

            Debug.Log(other.name + " entered the drop zone");
            Debug.Log("Puzzle 1 complete");

            if (puzzleManager != null)
            {
                puzzleManager.CompletePuzzle1();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (completed) return;

        if (other.CompareTag(correctTag))
        {
            Debug.Log(other.name + " is inside the drop zone");
        }
    }
}