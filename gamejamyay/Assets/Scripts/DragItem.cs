using UnityEngine;
using UnityEngine.InputSystem;

public class DragItem : MonoBehaviour
{
    private bool isDragging = false;
    private bool isInDropZone = false;

    private Vector3 offset;
    private Rigidbody2D rb;

    private Transform currentDropZone;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Mouse.current == null || Camera.main == null)
            return;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorldPos.z = 0f;

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);

            if (hit != null && hit.gameObject == gameObject)
            {
                isDragging = true;
                offset = transform.position - mouseWorldPos;

                if (rb != null)
                {
                    rb.linearVelocity = Vector2.zero;
                    rb.angularVelocity = 0f;
                    rb.bodyType = RigidbodyType2D.Kinematic;
                }
            }
        }

        if (isDragging && Mouse.current.rightButton.isPressed)
        {
            transform.position = mouseWorldPos + offset;
        }

        if (isDragging && Mouse.current.rightButton.wasReleasedThisFrame)
        {
            isDragging = false;

            if (rb != null)
                rb.bodyType = RigidbodyType2D.Dynamic;

            if (isInDropZone && currentDropZone != null)
            {
                transform.position = currentDropZone.position;
                Debug.Log(gameObject.name + " dropped in zone");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DropZone"))
        {
            isInDropZone = true;
            currentDropZone = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("DropZone"))
        {
            isInDropZone = false;
            currentDropZone = null;
        }
    }
}