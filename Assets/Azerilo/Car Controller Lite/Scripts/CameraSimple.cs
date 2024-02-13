using UnityEngine;

public class CarFollowerCamera : MonoBehaviour
{
    public Transform car;  // Reference to the car GameObject

    void LateUpdate()
    {
        if (car == null)
            return;

        // Set the camera's position to match the car's position
        transform.position = car.position;
    }
}
