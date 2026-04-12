using UnityEngine;
public class LeverSwap : MonoBehaviour
{
    public GameObject leverUp;
    public GameObject leverDown;

    private bool playerNearby = false;
    private bool activated = false;

    void Start()
    {
        if (leverDown != null)
            leverDown.SetActive(false);
    }

    void Update()
    {
        if (playerNearby && !activated && Input.GetKeyDown(KeyCode.E))
        {
            activated = true;

            if (leverDown != null)
                leverDown.SetActive(true);

            if (leverUp != null)
                Destroy(leverUp);

            Debug.Log("Lever pulled");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerNearby = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerNearby = false;
    }
}