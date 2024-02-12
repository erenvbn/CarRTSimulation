using UnityEngine;

public class DistanceCalculator : MonoBehaviour
{
    // Define DistanceManager class outside of the DistanceCalculator class
    public class DistanceManager
    {
        public float leftObsDistance;
        public float rightObsDistance;
    }

    // Declare a reference to DistanceManager
    DistanceManager distanceManager = new DistanceManager();

    // Max distance for raycasting
    public float maxDistance = 10f;

    void Update()
    {
        // Cast a ray to the left
        RaycastHit leftHit;
        if (Physics.Raycast(transform.position, -transform.right, out leftHit, maxDistance))
        {
            if (leftHit.collider.CompareTag("ObsSideLeft"))
            {
                float distanceToLeftObstacle = leftHit.distance;
                distanceManager.leftObsDistance = distanceToLeftObstacle; // Add semicolon at the end
            }
        }

        // Cast a ray to the right
        RaycastHit rightHit;
        if (Physics.Raycast(transform.position, transform.right, out rightHit, maxDistance))
        {
            if (rightHit.collider.CompareTag("ObsSideRight"))
            {
                float distanceToRightObstacle = rightHit.distance;
                distanceManager.rightObsDistance = distanceToRightObstacle; // Add semicolon at the end
            }
        }
        Debug.Log($"L:{distanceManager.leftObsDistance}, R: {distanceManager.rightObsDistance}");
    }
}
