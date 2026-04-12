using UnityEngine;

public class FeedAtMouth : MonoBehaviour
{
    public CheerUpMinigame minigame;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Lollipop"))
        {
            minigame.OnLollipopFed();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}