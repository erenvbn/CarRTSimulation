using UnityEngine;

public class CrashController : MonoBehaviour
{
    private bool isCrashed = false;
    public bool Crashed => isCrashed;

    public void OnCollisionEnter(Collision other)
    {
        // Check if the collided GameObject has the tag "ObsSide"
        if (other.gameObject.CompareTag("ObsSideLeft") || other.gameObject.CompareTag("ObsSideRight"))
        {
            isCrashed = true;
            Debug.Log($"Crashed with object tagged as: {other.gameObject.tag} with {other.gameObject.name}");
        }
        else
        {
            isCrashed = false;
            Debug.Log("Not crashed yet");
        }
    }

    void Update()
    {
        if (!isCrashed)
        {
            // Debug.Log("Not crashed");
        }
    }
}
