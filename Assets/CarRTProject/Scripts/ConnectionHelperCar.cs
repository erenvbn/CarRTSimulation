using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using NativeWebSocket;

[Serializable]
public class DistanceManagerData
{
    public float leftHitDistance;
    public float rightHitDistance;
    public float forwardHitDistance;
    public float backwardHitDistance;
    public float forwardLeftHitDistance;
    public float forwardRightHitDistance;
    public float backwardLeftHitDistance;
    public float backwardRightHitDistance;
}

[Serializable]
public class ConnectionHelperCar : MonoBehaviour
{
    public Text connectionStatusText;
    public bool manuelCarControllerToggle;
    public bool sendPictureStream;
    public bool sendDistanceData;
    public float dataSendFrequencyInSeconds = 0.3f;
    public SimpleCarController simpleCarController;
    public ManuelCarController manuelCarController;
    public Speedometer speedometer;

    [Serializable]
    public class ServerMessage
    {
        public float speed;
        public float rotation;
        public bool resetRequested;
    }

    [Serializable]
    public class CarData
    {
        public CarLocation carLocation;
        public bool isCrashed;
        public long timeStamp;
        public string screenshotBase64;
        public DistanceManagerData distanceManagerData;
        // public float targetSpeed;
        public float currentSpeed;
        public float rotation;
        public bool resetRequested;
    }

    [Serializable]
    public class CarLocation
    {
        public float x;
        public float y;
        public float z;
    }
    private CarData carData = new CarData();
    private CarLocation carLocation = new CarLocation();
    public ScreenshotController screenshotController;
    public DistanceManager distanceManager;
    public UIController uiController;

    [SerializeField]
    private string _wsUrl = "ws://localhost:5005";
    public string wsUrl
    {
        get { return _wsUrl; }
        set
        {
            _wsUrl = value;
            // Debug.Log(_wsUrl);
        }
    }

    private WebSocket websocket;

    async void Start()
    {
        // Find the MainCamera object in the scene to reach its screenshot controller
        GameObject mainCameraObject = GameObject.FindGameObjectWithTag("MainCamera");
        if (mainCameraObject != null)
        {
            screenshotController = mainCameraObject.GetComponent<ScreenshotController>();
        }
        else
        {
            Debug.LogError("MainCamera object not found in the scene.");
        }

        speedometer = FindObjectOfType<Speedometer>();

        manuelCarController = FindObjectOfType<ManuelCarController>();
        manuelCarController.enabled = false;


        if (manuelCarController == null)
        {
            Debug.LogError("ManuelCarController not found in the scene!");
        }
        else
        {
            // Debug.Log("ManuelCarController found!");
        }

        simpleCarController = FindObjectOfType<SimpleCarController>();
        if (simpleCarController == null)
        {
            Debug.LogError("SimpleCarController not found in the scene!");
        }
        else
        {
            // Debug.Log("SimpleCarController found!");
        }

        // Assign the existing distanceManager field by reaching DistanceManager in the car
        distanceManager = GetComponent<DistanceManager>();
        manuelCarControllerToggle = false;
        sendPictureStream = false;
        sendDistanceData = true;
        // Debug.Log(manuelCarControllerToggle);
        StartWebSocket();
        await websocket.Connect();
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
#endif
    }

    public void ToggleCarControllers(bool enableManuelController)
    {
        if (enableManuelController)
        {
            // Enable ManuelCarController and disable SimpleCarController
            manuelCarControllerToggle = true;

            manuelCarController.enabled = true;
            simpleCarController.enabled = false;
            // Debug.Log("ManuelCarController enabled");
        }
        else
        {
            // Enable SimpleCarController and disable ManuelCarController
            manuelCarControllerToggle = false;

            manuelCarController.enabled = false;
            simpleCarController.enabled = true;
            // Debug.Log("SimpleCarController enabled");
        }
    }

    public async void StartWebSocket()
    {
        uiController.ShowLoadingIcon();

        if (websocket == null || websocket.State != WebSocketState.Open)
        {
            websocket = new WebSocket(wsUrl);

            websocket.OnOpen += () =>
            {
                Debug.Log("Connection open!");
                uiController.HideLoadingIcon();
            };

            websocket.OnError += (e) =>
            {
                Debug.Log("Error! " + e);
                uiController.HideLoadingIcon();
                uiController.UpdateConnectionErrorMessage("Error! " + e);
            };

            websocket.OnMessage += (bytes) =>
            {
                var message = System.Text.Encoding.UTF8.GetString(bytes);
                var serverData = JsonUtility.FromJson<ServerMessage>(message);
                Debug.Log("Message received from the server: " + serverData.resetRequested);
                carData.resetRequested = serverData.resetRequested;

                if (serverData != null && !carData.resetRequested)
                {
                    if (!manuelCarControllerToggle)
                    {
                        simpleCarController.TargetSpeed = serverData.speed;
                        simpleCarController.TargetRotation = serverData.rotation;
                    }
                }
                else
                {
                    RestartScene();
                    // Time.timeScale = 1;
                }

                // Format the JSON message with indentation for better readability
                var formattedMessage = JsonUtility.ToJson(serverData, true);

                // Update the UI with the formatted message
                uiController.UpdateIncomingMessage(formattedMessage);
            };


            // Connect to the WebSocket server
            await websocket.Connect();

            // Check if the ConnectionHelperCar object is still valid before starting the coroutine
            if (this != null)
            {
                StartCoroutine(SendDataCoroutine());
            }
        }
        else
        {
            Debug.Log("Websocket connection was not found");
            uiController.HideLoadingIcon();
            uiController.UpdateConnectionErrorMessage("Websocket connection was not found");
            uiController.txtConnectionError.gameObject.SetActive(false);
        }
    }

    private void SetCarLocation()
    {
        carLocation.x = transform.position.x;
        carLocation.y = transform.position.y;
        carLocation.z = transform.position.z;
        carData.carLocation = carLocation;
    }

    private void SetCarDistanceToObs()
    {
        if (sendDistanceData)
        {
            carData.distanceManagerData = new DistanceManagerData
            {
                leftHitDistance = distanceManager.leftHitDistance,
                rightHitDistance = distanceManager.rightHitDistance,
                forwardHitDistance = distanceManager.forwardHitDistance,
                backwardHitDistance = distanceManager.backwardHitDistance,
                forwardLeftHitDistance = distanceManager.forwardLeftHitDistance,
                forwardRightHitDistance = distanceManager.forwardRightHitDistance,
                backwardLeftHitDistance = distanceManager.backwardLeftHitDistance,
                backwardRightHitDistance = distanceManager.backwardRightHitDistance
            };
        }
        else
        {
            carData.distanceManagerData = null;
        }
    }
    private IEnumerator SendDataCoroutine()
    {
        while (true)
        {
            SendWebSocketMessage();
            yield return new WaitForSeconds(dataSendFrequencyInSeconds);
        }
    }

    public async void SendWebSocketMessage()
    {
        if (websocket != null && websocket.State == WebSocketState.Open)
        {
            SetCarDistanceToObs();
            // Update car location
            SetCarLocation();
            // Set crash status
            carData.isCrashed = GetComponent<CrashController>().Crashed;

            if (!manuelCarControllerToggle)
            {
                // Only update speed and rotation when manuelCarControllerToggle is false
                carData.currentSpeed = speedometer.GetCurrentSpeed();
                carData.rotation = simpleCarController.CurrentRotation;
            }
            else
            {
                // If manuelCarControllerToggle is true, use the Speedometer's current speed
                carData.currentSpeed = speedometer.GetCurrentSpeed();
                carData.rotation = manuelCarController.GetCurrentRotation();
            }
            // Unix timestamp in milliseconds
            carData.timeStamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;

            byte[] screenshotBytes = null;

            if (sendPictureStream)
            {
                screenshotBytes = screenshotController.CaptureScreenshot();
                if (screenshotBytes != null)
                    carData.screenshotBase64 = Convert.ToBase64String(screenshotBytes);
                else
                {
                    Debug.LogError("Failed to capture screenshot.");
                }
            }

            var jsonString = JsonUtility.ToJson(carData, true);

            // Debug.Log("Sending message: " + jsonString);
            uiController.UpdateOutgoingMessage(jsonString);

            // Send the stringified JSON to the server
            await websocket.SendText(jsonString);
        }

    }

    private void RestartScene()
    {
        Debug.Log("Restarting scene...");
        Time.timeScale = 0;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        manuelCarControllerToggle = false;
    }

    private async void OnApplicationQuit()
    {
        uiController.UpdateConnectionErrorMessage("Application Quitting");
        await websocket.Close();
    }
}
