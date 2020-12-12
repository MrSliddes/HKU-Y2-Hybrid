using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;

//https://www.youtube.com/watch?v=amy3L3pGWH0&list=PLDrcLKDFENwI0Bg6bwNuPcP0K-xDAAsrc&index=2
public class MasterServer : MonoBehaviour // for more complex stuff, create 2 scenes as the server code is also included into the client copy
{
    private const int MAX_USERS = 100;
    private const int PORT = 26000;
    private const int WEB_PORT = 26001;
    private const int BYTE_SIZE = 1024;

    private byte reliableChannel;
    private byte error;
    private int hostId;
    private int webHostId;

    private bool isInit = false;

    private bool[] clientsActive;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        Init();
    }

    private void Update()
    {
        UpdateMessagePump();
    }

    public void ShutDown()
    {
        isInit = false;
        NetworkTransport.Shutdown();
    }

    public void UpdateMessagePump()
    {
        if(!isInit) return;

        int receiveHostId; // Is this from web? or standalone
        int connectionId; // which user is sending me this?
        int channelId; // Which lane is he sending that message from?

        byte[] receiveBuffer = new byte[BYTE_SIZE];
        int dataSize;

        NetworkEventType type = NetworkTransport.Receive(out receiveHostId, out connectionId, out channelId, receiveBuffer, BYTE_SIZE, out dataSize, out error);
        switch(type)
        {
            case NetworkEventType.DataEvent:
                Debug.Log("Data");
                // Convert binary back to data
                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStream ms = new MemoryStream(receiveBuffer);
                NetMsg msg = (NetMsg)formatter.Deserialize(ms);

                OnData(connectionId, channelId, receiveHostId, msg);
                break;
            case NetworkEventType.ConnectEvent:
                Debug.Log(string.Format("User {0} has connected through host {1}.", connectionId, receiveHostId));
                break;
            case NetworkEventType.DisconnectEvent:
                Debug.Log(string.Format("User {0} has disconnected.", connectionId));
                clientsActive[connectionId] = false;
                break;
            case NetworkEventType.Nothing:
                break;
            case NetworkEventType.BroadcastEvent:
                break;
            default:
                break;
        }
    }

    #region Send

    public void SendClient(int receiveHostId, int connectionId, NetMsg msg)
    {
        // This is where we hold our data
        byte[] buffer = new byte[BYTE_SIZE];

        // Convert data into a byte[]
        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream ms = new MemoryStream(buffer);
        formatter.Serialize(ms, msg);

        // Send to standalone or host
        if(receiveHostId == 0)
        {
            // Send to client
            NetworkTransport.Send(hostId, connectionId, reliableChannel, buffer, BYTE_SIZE, out error);
        }
        else
        {
            // Send to webhost client
            NetworkTransport.Send(webHostId, connectionId, reliableChannel, buffer, BYTE_SIZE, out error);
        }
    }

    #endregion

    private void Init()
    {
        Debug.Log("Starting server...");
        NetworkTransport.Init();

        ConnectionConfig cc = new ConnectionConfig();
        cc.AddChannel(QosType.Reliable);

        HostTopology topo = new HostTopology(cc, MAX_USERS);

        // Server only
        hostId = NetworkTransport.AddHost(topo, PORT, null);
        webHostId = NetworkTransport.AddWebsocketHost(topo, WEB_PORT, null); // Allows connection trough browser

        // Set array
        clientsActive = new bool[MAX_USERS];

        isInit = true;
        Debug.Log(string.Format("Opening connection on part {0} and webport {1}", PORT, WEB_PORT));
    }

    #region OnData

    /// <summary>
    /// Check what message it is and what to do with it
    /// </summary>
    /// <param name="connectionId"></param>
    /// <param name="channelId"></param>
    /// <param name="receiveHostId"></param>
    /// <param name="msg"></param>
    private void OnData(int connectionId, int channelId, int receiveHostId, NetMsg msg)
    {
        Debug.Log("Receive message of type: " + msg.OperationCode);

        switch(msg.OperationCode)
        {
            case NetOperationCode.None: break;
            case NetOperationCode.CreatePlayer:
                // Create a player
                Debug.Log("Creating new player");
                clientsActive[connectionId] = true;
                // send to every client that a new player has joined


                break;

            default:
                break;
        }
    }

    #endregion
}
