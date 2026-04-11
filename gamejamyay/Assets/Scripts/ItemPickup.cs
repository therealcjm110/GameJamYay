using UnityEngine;
using UnityEngine.InputSystem;

public class ItemPickup : MonoBehaviour
{
    public Transform hand;
    public float throwForce = 5f;

    private GameObject heldItem;
    private bool isNearItem = false;
    private GameObject nearbyItem = null;

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (isNearItem && heldItem == null)
            {
                PickUp(nearbyItem);
            }
            else if (heldItem != null)
            {
                Throw();
            }
        }
    }

    void PickUp(GameObject item)
    {
        heldItem = item;
        heldItem.transform.SetParent(hand);
        heldItem.transform.localPosition = Vector3.zero;

        Rigidbody2D itemRb = heldItem.GetComponent<Rigidbody2D>();
        if (itemRb != null)
        {
            itemRb.linearVelocity = Vector2.zero;
            itemRb.angularVelocity = 0f;
            itemRb.bodyType = RigidbodyType2D.Kinematic;
        }

        Debug.Log("Picked up: " + heldItem.name);
    }

    void Throw()
    {
        heldItem.transform.SetParent(null);

        Rigidbody2D itemRb = heldItem.GetComponent<Rigidbody2D>();
        if (itemRb != null)
        {
            itemRb.bodyType = RigidbodyType2D.Dynamic;

            Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
            mouseWorldPos.z = 0f;

            Vector2 throwDirection = ((Vector2)mouseWorldPos - itemRb.position).normalized;
            itemRb.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);
        }

        Debug.Log("Threw: " + heldItem.name);
        heldItem = null;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Item") && heldItem == null)
        {
            isNearItem = true;
            nearbyItem = other.gameObject;
            Debug.Log("Near item: " + nearbyItem.name);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Item"))
        {
            isNearItem = false;
            nearbyItem = null;
        }
    }
}