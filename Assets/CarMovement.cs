using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 120f;

    void Update()
    {
        // Forward/backward movement (W/S)
        float move = Input.GetAxis("Vertical");

        // Rotation (A/D)
        float turn = Input.GetAxis("Horizontal");

        // Move forward/backward
        transform.Translate(Vector3.forward * move * moveSpeed * Time.deltaTime);

        // Rotate left/right
        transform.Rotate(Vector3.up * turn * rotationSpeed * Time.deltaTime);
    }
}