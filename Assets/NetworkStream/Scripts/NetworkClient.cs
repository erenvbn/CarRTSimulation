using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class NetworkClient : MonoBehaviour
{
    public string serverIP = "127.0.0.1"; // IP address of the server
    public int serverPort = 8888; // Port number to connect

    private TcpClient client;
    private NetworkStream stream;
    private byte[] receiveBuffer = new byte[1024]; // Buffer to store received data

    void Start()
    {
        ConnectToServer();
    }

    void ConnectToServer()
    {
        try
        {
            // Connect to the server
            client = new TcpClient(serverIP, serverPort);
            stream = client.GetStream();

            // Begin asynchronous reading
            stream.BeginRead(receiveBuffer, 0, receiveBuffer.Length, ReceiveData, null);
        }
        catch (Exception e)
        {
            Debug.LogError("Error connecting to server: " + e.Message);
        }
    }

    void ReceiveData(IAsyncResult ar)
    {
        try
        {
            int bytesRead = stream.EndRead(ar);
            if (bytesRead <= 0)
            {
                // Connection closed
                Debug.Log("Connection closed by server.");
                return;
            }

            // Convert received bytes to string
            string receivedData = Encoding.ASCII.GetString(receiveBuffer, 0, bytesRead);

            // Parse received data (Assuming data format: "x,y,z")
            string[] values = receivedData.Split(',');
            if (values.Length == 3)
            {
                float x = float.Parse(values[0]);
                float y = float.Parse(values[1]);
                float z = float.Parse(values[2]);

                // Update object's position
                transform.position = new Vector3(x, y, z);
            }

            // Continue asynchronous reading
            stream.BeginRead(receiveBuffer, 0, receiveBuffer.Length, ReceiveData, null);
        }
        catch (Exception e)
        {
            Debug.LogError("Error receiving data: " + e.Message);
        }
    }

    void OnDestroy()
    {
        // Clean up resources
        if (stream != null)
            stream.Close();
        if (client != null)
            client.Close();
    }
}
