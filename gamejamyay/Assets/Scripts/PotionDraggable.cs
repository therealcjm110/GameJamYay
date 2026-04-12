using UnityEngine;
using UnityEngine.InputSystem;

public class PotionDraggable : MonoBehaviour
{
    [Header("This potion's color name (Red, Yellow, or Blue)")]
    public string colorName;

    private bool isDragging = false;
    private Vector3 startPosition;
    private Vector3 offset;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    void Update()
    {
        if (Mouse.current == null || Camera.main == null) return;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorldPos.z = 0f;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log(gameObject.name + " dropped at: " + transform.position);

            Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);
            if (hit != null && hit.gameObject == gameObject)
            {
                isDragging = true;
                offset = transform.position - mouseWorldPos;
                if (rb != null)
                {
                    rb.linearVelocity = Vector2.zero;
                    rb.bodyType = RigidbodyType2D.Kinematic;
                }
            }
        }

        if (isDragging && Mouse.current.leftButton.isPressed)
        {
            transform.position = mouseWorldPos + offset;
        }

        if (isDragging && Mouse.current.leftButton.wasReleasedThisFrame)
        {
            isDragging = false;
            if (rb != null) rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bowl"))
        {
            // Snap to bowl
            transform.position = other.transform.position;

            // Tell the mixer
            PotionMixer.Instance.PotionDropped(colorName);
        }
    }

    public void ReturnToStart()
    {
        transform.position = startPosition;
        if (rb != null) rb.bodyType = RigidbodyType2D.Dynamic;
    }
}
