using UnityEngine;

public class FingerPointer : MonoBehaviour
{
    [Header("Settings")]
    public bool flipSprite = false;     // adjust if finger points wrong direction at rest
    public float minAngle = -20f;
    public float maxAngle = 20f;
    public float angleOffset = -90f;    // adjust for sprite direction

    void Update(){
        // get mouse pos in world
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = transform.position.z;

        // get direction from finger pivot to mouse
        Vector3 direction = mouseWorld - transform.position;

        // calculate angle
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;  // change 90f as needed

        // clamp to allowed range
        angle = Mathf.Clamp(angle, 90f + minAngle, 90f + maxAngle);

        angle -= 90f;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
