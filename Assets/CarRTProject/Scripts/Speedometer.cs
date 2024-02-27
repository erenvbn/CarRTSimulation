using UnityEngine;

public class Speedometer : MonoBehaviour
{
    public Rigidbody carRigidbody;
    private Vector3 previousPosition;
    public float currentSpeedKPH;

    void Start()
    {
        // Initialize the previous position with the current position
        previousPosition = transform.position;
    }

    void Update()
    {
        // Check if carRigidbody is assigned
        if (carRigidbody != null)
        {
            // Calculate displacement since the last frame
            Vector3 displacement = transform.position - previousPosition;

            // Calculate speed in km/h
            currentSpeedKPH = displacement.magnitude / Time.deltaTime;

            // Log the speed to the console
            // Debug.Log(currentSpeedKPH.ToString("F2") + " km/h");

            // Update the previous position for the next frame
            previousPosition = transform.position;
        }
        else
        {
            // If carRigidbody is not assigned, log an error
            Debug.LogError("Car Rigidbody is not assigned in the CarSpeedometer script.");
        }
    }

    public float GetCurrentSpeed()
    {
        return currentSpeedKPH;
    }
}
