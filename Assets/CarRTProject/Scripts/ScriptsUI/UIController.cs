using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class UIController : MonoBehaviour
{
    //add from the editor, the gameobject with the connectionhelpercar component
    public ConnectionHelperCar connectionHelperCar;
    public ManuelCarController manuelCarController;
    public TextMeshProUGUI txtCrash;
    public TextMeshProUGUI consoleIncomingText;
    public TextMeshProUGUI consoleOutgoingText;
    public TextMeshProUGUI txtConnectionError;
    public UnityEngine.UI.Button stopButton;
    public UnityEngine.UI.Button playButton;
    public UnityEngine.UI.Button restartButton;
    public TMP_InputField socketUrlInput;
    public TMP_InputField dataSenderFrequencyInput;
    public UnityEngine.UI.Toggle togglePictureStream;
    public UnityEngine.UI.Toggle toggleDistanceDataStream;
    public UnityEngine.UI.Toggle toggleManuelCarController;
    public GameObject loadingIcon;

    public void ShowLoadingIcon()
    {
        loadingIcon.SetActive(true);
    }

    public void HideLoadingIcon()
    {
        loadingIcon.SetActive(false);
    }



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
        socketUrlInput.gameObject.SetActive(false);
        dataSenderFrequencyInput.gameObject.SetActive(false);
        togglePictureStream.gameObject.SetActive(false);
        toggleManuelCarController.gameObject.SetActive(false);
        //getting the game object that is related with that component (toggleDistanceDataStream)
        toggleDistanceDataStream.gameObject.SetActive(false);
        connectionHelperCar.StartWebSocket();
    }

    public void onResetButtonClick()
    {
        Time.timeScale = 0;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        connectionHelperCar.manuelCarControllerToggle = false;
    }

    public void onStopButtonClick()
    {
        Time.timeScale = 0;
    }

    public void SetWebSocketUrl()
    {
        if (connectionHelperCar != null && socketUrlInput != null)
        {
            connectionHelperCar.wsUrl = socketUrlInput.text;
        }
    }


    public void TogglePictureStream(bool toggle)
    {
        connectionHelperCar.sendPictureStream = toggle;
        Debug.Log(toggle);
    }

    public void ToggleDistanceDataStream(bool toggle)
    {
        connectionHelperCar.sendDistanceData = toggle;
        Debug.Log(toggle);
    }

    public void ToggleManuelCarController(bool toggle)
    {
        connectionHelperCar.ToggleCarControllers(toggle);
        Debug.Log(toggle);
    }

    public void SetDataSendFrequency()
    {
        string inputText = dataSenderFrequencyInput.text;
        if (float.TryParse(inputText, out float frequency))
        {
            connectionHelperCar.dataSendFrequencyInSeconds = frequency;
        }
        else
        {
            Debug.LogError("Failed to parse input text as a float.");
        }
    }

    public void UpdateIncomingMessage(string message)
    {
        consoleIncomingText.text = message;
    }
    public void UpdateOutgoingMessage(string message)
    {
        consoleOutgoingText.text = message;
    }

    public void UpdateConnectionErrorMessage(string message)
    {
        txtConnectionError.gameObject.SetActive(true);
        txtConnectionError.text = message;
    }


}