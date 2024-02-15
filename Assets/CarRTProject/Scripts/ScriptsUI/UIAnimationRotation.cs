using UnityEngine;

public class RotateAnimation : MonoBehaviour
{
    public float rotationSpeed = 200f; // Adjust the rotation speed as needed

    void Update()
    {
        // Rotate the object around its Z-axis with deltaTime for smooth animation
        transform.Rotate(-Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
