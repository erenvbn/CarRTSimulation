using System;
using UnityEngine;

[Serializable]
public class CrashController : MonoBehaviour
{
    private bool isCrashed = false;
    public bool Crashed => isCrashed;

    //Create an empty instance for uiController
    private UIController uiController;

    void Start()
    {
        // Getting the canvas element from the editor to fill the uiController
        uiController = FindObjectOfType<UIController>();
        if (uiController == null)
        {
            Debug.LogError("UIController not found.");
        }
    }

    void Update()
    {
        if (!isCrashed)
        {
            // Debug.Log("Not crashed");
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        // Check if the collided GameObject has the tag "ObsSide"
        if (other.gameObject.CompareTag("ObsSide"))
        {
            isCrashed = true;
            uiController?.ToggleCrashText(true);
            Debug.Log($"Crashed with object tagged as: {other.gameObject.tag} with {other.gameObject.name}");
        }
        else
        {
            isCrashed = false;
            // Debug.Log("Not crashed yet");
        }
    }
}
