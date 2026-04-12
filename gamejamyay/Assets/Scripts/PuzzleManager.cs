using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    private bool puzzle1Solved = false;

    public void CompletePuzzle1()
    {
        if (puzzle1Solved) return;

        puzzle1Solved = true;

        Debug.Log("Puzzle 1 complete. Transitioning to next scene...");

        if (SceneTransition.Instance != null)
            SceneTransition.Instance.GoToNextScene();
        else
            Debug.LogWarning("SceneTransition instance not found! Make sure SceneTransition is in the scene.");
    }
}