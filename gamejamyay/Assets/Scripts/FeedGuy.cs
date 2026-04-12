using UnityEngine;

public class FeedGuy : MonoBehaviour
{
    public GameObject sadGuy;
    public GameObject happyGuy;
    public string foodTag = "Item";

    private bool completed = false;

    private void Start()
    {
        if (happyGuy != null)
            happyGuy.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (completed) return;

        if (other.CompareTag(foodTag))
        {
            completed = true;

            if (sadGuy != null)
                sadGuy.SetActive(false);

            if (happyGuy != null)
                happyGuy.SetActive(true);

            Destroy(other.gameObject);

            Debug.Log("Guy is now happy!");
        }
    }
}
