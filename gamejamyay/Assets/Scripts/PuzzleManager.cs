using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public Rigidbody2D playerRb;
    public Transform puzzle2SpawnPoint;

    private bool puzzle1Solved = false;

    public void CompletePuzzle1()
    {
        if (puzzle1Solved) return;

        puzzle1Solved = true;

        if (playerRb != null && puzzle2SpawnPoint != null)
        {
            playerRb.linearVelocity = Vector2.zero;
            playerRb.position = puzzle2SpawnPoint.position;
        }

        Debug.Log("Puzzle 1 complete. Player moved to Puzzle 2.");
    }
}
