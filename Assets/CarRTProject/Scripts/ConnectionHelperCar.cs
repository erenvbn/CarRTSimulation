using System;
using UnityEngine;
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
    public bool sendPictureStream;
    public bool sendDistanceData;

    [Serializable]
    public class ServerMessage
    {
        public float speed;
        public float rotation;
    }

    [Serializable]
    public class CarData
    {
        public CarLocation carLocation;
        public bool isCrashed;
        public long timeStamp;
        public string screenshotBase64;
        public DistanceManagerData distanceManagerData;
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

    [SerializeField]
    private string _wsUrl = "ws://localhost:5005";
    public string wsUrl
    {
        get { return _wsUrl; }
        set
        {
            _wsUrl = value;
            Debug.Log(_wsUrl);
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

        // Assign the existing distanceManager field by reaching DistanceManager in the car
        distanceManager = GetComponent<DistanceManager>();
        sendPictureStream = true;
        sendDistanceData = true;
        StartWebSocket();

        InvokeRepeating("SendWebSocketMessage", 0.0f, 0.3f);
        await websocket.Connect();
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
#endif
    }

    public async void StartWebSocket()
    {
        if (websocket == null || websocket.State != WebSocketState.Open)
        {
            // Initialize the WebSocket connection
            websocket = new WebSocket(wsUrl);

            websocket.OnOpen += () =>
            {
                Debug.Log("Connection open!");
            };

            websocket.OnError += (e) =>
            {
                Debug.Log("Error! " + e);
            };

            websocket.OnMessage += (bytes) =>
            {
                var message = System.Text.Encoding.UTF8.GetString(bytes);
                var serverData = JsonUtility.FromJson<ServerMessage>(message);
                if (serverData != null)
                {
                    FindObjectOfType<CarController>().TargetSpeed = serverData.speed;
                    FindObjectOfType<CarController>().TargetRotation = serverData.rotation;
                }
            };

            // Connect to the WebSocket server
            await websocket.Connect();
        }
        else
        {
            Debug.Log("Websocket connection was not found");
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

    async void SendWebSocketMessage()
    {
        if (websocket.State == WebSocketState.Open)
        {
            SetCarDistanceToObs();
            // Update car location
            SetCarLocation();
            // Set crash status
            carData.isCrashed = GetComponent<CrashController>().Crashed;

            if (GetComponent<CrashController>().Crashed)
            {
                Debug.Log("Car crashed");
                // Time.timeScale = 0;
                // RestartCar();
            }
            else
            {
                Debug.Log("Car not crashed");
            }


            // Unix timestamp in milliseconds
            carData.timeStamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
            // Debug.Log("Sending message: " + carData.carLocation.x + ", " + carData.carLocation.y + ", " + carData.carLocation.z + ", " + carData.isCrashed + ", " + carData.timeStamp);
            // Serialize the JSON object to a string

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

            var jsonString = JsonUtility.ToJson(carData);
            Debug.Log("Sending message: " + jsonString);

            // Send the stringified JSON to the server
            await websocket.SendText(jsonString);
        }
    }

    private void RestartCar()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        Time.timeScale = 1f;
    }


    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }
}
