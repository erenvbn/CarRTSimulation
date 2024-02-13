using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    //add from the editor, the gameobject with the connectionhelpercar component
    public ConnectionHelperCar connectionHelperCar;

    //Create a public field for txtCrash object

    public TextMeshProUGUI txtCrash;
    public UnityEngine.UI.Button stopButton;
    public UnityEngine.UI.Button playButton;
    public UnityEngine.UI.Button restartButton;

    // Start is called before the first frame update
    void Start()
    {

        if (stopButton == null)
        {
            Debug.LogError("stopButton component is not assigned");
        }

        if (playButton == null)
        {
            Debug.LogError("playButton component is not assigned");
        }

        if (restartButton == null)
        {
            Debug.LogError("restartButton component is not assigned");
        }

        if (txtCrash == null)
        {
            Debug.LogError("TextMeshProUGUI component not assigned.");
        }

        if (connectionHelperCar == null)
        {
            Debug.LogError("ConnectionHelperCar component not assigned.");
        }
    }

    void Update()
    {

    }

    public void ToggleCrashText(bool isActive)
    {
        // Ensure that txtCrash is not null before accessing gameObject
        if (txtCrash != null)
        {
            txtCrash.gameObject.SetActive(isActive);
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component not assigned.");
        }
    }

    public void onPlayButtonClick()
    {
        Time.timeScale = 1;
        connectionHelperCar.StartWebSocket();
    }

    public void onResetButtonClick()
    {
        Time.timeScale = 0;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void onStopButtonClick()
    {
        Time.timeScale = 0;
    }
}