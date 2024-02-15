using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPrefSaveController : MonoBehaviour
{
    public TMP_InputField socketUrlInput;
    private string savedURL;

    void Start()
    {
        // Retrieve the saved URL or use a default value
        savedURL = PlayerPrefs.GetString("WebSocketURL", "ws://localhost:5005");
        socketUrlInput.text = savedURL;
        UpdatePlaceholderText();
    }

    public void SaveURL()
    {
        string newURL = socketUrlInput.text;
        // Save the new URL
        PlayerPrefs.SetString("WebSocketURL", newURL);
        PlayerPrefs.Save();

        // Update the saved URL variable and placeholder text
        savedURL = newURL;
        UpdatePlaceholderText();
    }

    private void UpdatePlaceholderText()
    {
        // Set the placeholder text to the saved URL
        socketUrlInput.placeholder.GetComponent<TextMeshProUGUI>().text = savedURL;
    }
}
