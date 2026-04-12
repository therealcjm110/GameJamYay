using UnityEngine;

public class LeverTrigger : MonoBehaviour
{
    public Animator leverAnimator;

    private bool playerNearby = false;
    private bool activated = false;

    void Update()
    {
        if (playerNearby && !activated && Input.GetKeyDown(KeyCode.E))
        {
            activated = true;
            leverAnimator.SetTrigger("Pull");
            Debug.Log("Lever pulled");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }
}