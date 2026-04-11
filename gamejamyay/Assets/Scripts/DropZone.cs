using UnityEngine;

public class DropZone : MonoBehaviour
{
    public string correctTag = "Item";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(correctTag))
        {
            Debug.Log(other.name + " entered the drop zone");
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag(correctTag))
        {
            Debug.Log(other.name + " is inside the drop zone");
        }
    }
}