using System;
using UnityEngine;

[Serializable]

//The distance manager gets the center information of its attached object
public class DistanceManager : MonoBehaviour
{
    public float leftHitDistance;
    public float rightHitDistance;
    public float forwardHitDistance;
    public float backwardHitDistance;
    public float forwardLeftHitDistance;
    public float forwardRightHitDistance;
    public float backwardLeftHitDistance;
    public float backwardRightHitDistance;
    public string obstacleTag = "ObsSide";
    public float maxDistance = 10f;

    //Run distance calculator on update
    void Update()
    {
        DistanceCalculator();
    }

    public void DistanceCalculator()
    {
        // Cast a ray to the left
        RaycastHit leftHit;
        RaycastHit rightHit;
        RaycastHit forwardHit;
        RaycastHit backwardHit;
        RaycastHit forwardLeftHit;
        RaycastHit forwardRightHit;
        RaycastHit backwardLeftHit;
        RaycastHit backwardRightHit;

        if (Physics.Raycast(transform.position, -transform.right, out leftHit, maxDistance))
        {
            if (leftHit.collider.CompareTag(obstacleTag))
            {
                float distanceToObstacle = leftHit.distance;
                leftHitDistance = distanceToObstacle;
            }
        }

        if (Physics.Raycast(transform.position, transform.right, out rightHit, maxDistance))
        {
            if (rightHit.collider.CompareTag(obstacleTag))
            {
                float distanceToObstacle = rightHit.distance;
                rightHitDistance = distanceToObstacle;
            }
        }

        if (Physics.Raycast(transform.position, transform.forward, out forwardHit, maxDistance))
        {
            if (forwardHit.collider.CompareTag(obstacleTag))
            {
                float distanceToObstacle = forwardHit.distance;
                forwardHitDistance = distanceToObstacle;
            }
        }

        if (Physics.Raycast(transform.position, -transform.forward, out backwardHit, maxDistance))
        {
            if (backwardHit.collider.CompareTag(obstacleTag))
            {
                float distanceToObstacle = backwardHit.distance;
                backwardHitDistance = distanceToObstacle;
            }
        }

        if (Physics.Raycast(transform.position, (transform.forward - transform.right).normalized, out forwardLeftHit, maxDistance))
        {
            if (forwardLeftHit.collider.CompareTag(obstacleTag))
            {
                float distanceToObstacle = forwardLeftHit.distance;
                forwardLeftHitDistance = distanceToObstacle;
            }
        }

        if (Physics.Raycast(transform.position, (transform.forward + transform.right).normalized, out forwardRightHit, maxDistance))
        {
            if (forwardRightHit.collider.CompareTag(obstacleTag))
            {
                float distanceToObstacle = forwardRightHit.distance;
                forwardRightHitDistance = distanceToObstacle;
            }
        }

        if (Physics.Raycast(transform.position, (-transform.forward - transform.right).normalized, out backwardLeftHit, maxDistance))
        {
            if (backwardLeftHit.collider.CompareTag(obstacleTag))
            {
                float distanceToObstacle = backwardLeftHit.distance;
                backwardLeftHitDistance = distanceToObstacle;
            }
        }

        if (Physics.Raycast(transform.position, (-transform.forward + transform.right).normalized, out backwardRightHit, maxDistance))
        {
            if (backwardRightHit.collider.CompareTag(obstacleTag))
            {
                float distanceToObstacle = backwardRightHit.distance;
                backwardRightHitDistance = distanceToObstacle;
            }
        }

        // Debug.Log("Left: " + leftHitDistance +
        //           ", Right: " + rightHitDistance +
        //           ", Forward: " + forwardHitDistance +
        //           ", Backward: " + backwardHitDistance +
        //           ", ForwardLeft: " + forwardLeftHitDistance +
        //           ", ForwardRight: " + forwardRightHitDistance +
        //           ", BackwardLeft: " + backwardLeftHitDistance +
        //           ", BackwardRight: " + backwardRightHitDistance);
    }
}
