using UnityEngine;

public class ManuelCarController : MonoBehaviour
{
    public float maxSpeed = 10.0f; // Maximum speed
    public float acceleration = 4.0f; // Acceleration rate
    public float deceleration = 4f; // Deceleration rate
    public float rotationSpeed = 100.0f;

    private float currentSpeed = 0.0f; // Current speed
    private float currentRotation = 0.0f; // Current rotation

    void Update()
    {
        // Manual control using arrow keys
        float translation = Input.GetAxis("Vertical") * currentSpeed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;
        transform.Translate(0, 0, translation);
        transform.Rotate(0, rotation, 0);

        // Update current rotation
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            // Rotate left (negative rotation)
            currentRotation -= rotationSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            // Rotate right (positive rotation)
            currentRotation += rotationSpeed * Time.deltaTime;
        }

        // If the car is moving forward
        if (Input.GetKey(KeyCode.UpArrow))
        {
            // Accelerate
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
        }
        // If the car is moving backward
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            // Decelerate
            currentSpeed = Mathf.MoveTowards(currentSpeed, -maxSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            // Slow down to stop when no arrow keys are pressed
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.deltaTime);
        }
    }

    // Method to get the current speed
    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    // Method to get the current rotation
    public float GetCurrentRotation()
    {
        return currentRotation;
    }
}
