using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class DataSender : MonoBehaviour
{
    public string serverIP = "127.0.0.1"; // IP address of the server
    public int serverPort = 8888; // Port number to connect

    private TcpClient client;
    private NetworkStream stream;
    private bool running = true;

    void Start()
    {
        ConnectToServer();
        StartSendingData();
    }

    void ConnectToServer()
    {
        try
        {
            // Connect to the server
            client = new TcpClient(serverIP, serverPort);
            stream = client.GetStream();
        }
        catch (Exception e)
        {
            Debug.LogError("Error connecting to server: " + e.Message);
        }
    }

    void StartSendingData()
    {
        // Start a separate thread for sending data
        Thread senderThread = new Thread(SendDataLoop);
        senderThread.Start();
    }

    void SendDataLoop()
    {
        while (running)
        {
            try
            {
                // Generate sample movement data (replace this with your actual data)
                float x = UnityEngine.Random.Range(-10f, 10f);
                float y = UnityEngine.Random.Range(-10f, 10f);
                float z = UnityEngine.Random.Range(-10f, 10f);

                // Format the movement data as a string
                string data = $"{x},{y},{z}";

                // Convert the string to bytes and send it over the network
                byte[] bytes = Encoding.ASCII.GetBytes(data);
                stream.Write(bytes, 0, bytes.Length);

                // Log a message on the main thread
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    Debug.Log("Data sent: " + data);
                });

                // Sleep for a short duration to control the sending rate
                Thread.Sleep(100); // Adjust this value as needed
            }
            catch (Exception e)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    Debug.LogError("Error sending data: " + e.Message);
                });
                break;
            }
        }
    }

    void OnDestroy()
    {
        // Clean up resources
        if (stream != null)
            stream.Close();
        if (client != null)
            client.Close();

        running = false; // Stop the sender thread
    }
}
