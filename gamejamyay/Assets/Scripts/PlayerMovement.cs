using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;    

    private Rigidbody2D rb;
    private Vector2 movement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {

        movement = Vector2.zero;

        movement = Vector2.zero;

        if ((Keyboard.current.wKey.isPressed) || (Keyboard.current.upArrowKey.isPressed))
            movement.y += 1;

        if ((Keyboard.current.sKey.isPressed) || (Keyboard.current.downArrowKey.isPressed))
            movement.y -= 1;

        if ((Keyboard.current.aKey.isPressed) || (Keyboard.current.leftArrowKey.isPressed))
            movement.x -= 1;

        if ((Keyboard.current.dKey.isPressed) || (Keyboard.current.rightArrowKey.isPressed))
            movement.x += 1;

        movement = movement.normalized;
    }
    void FixedUpdate()
    {
        Vector2 newPos = rb.position + movement * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);
    }

}
