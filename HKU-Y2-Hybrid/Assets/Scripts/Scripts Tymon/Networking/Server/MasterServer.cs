using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;

//https://www.youtube.com/watch?v=amy3L3pGWH0&list=PLDrcLKDFENwI0Bg6bwNuPcP0K-xDAAsrc&index=2
public class MasterServer : MonoBehaviour // for more complex stuff, create 2 scenes as the server code is also included into the client copy
{
    public string ip;

    private const int MAX_USERS = 100;
    private const int PORT = 4444; //26000
    private const int WEB_PORT = 4445;
    private const int BYTE_SIZE = 1024;
    private string SERVER_IP = "";

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
                ms.Position = 0;
                NetMsg msg = (NetMsg)formatter.Deserialize(ms);

                Debug.Log("Received data: " + msg.GetType().ToString());
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
        SERVER_IP = ip;
        Debug.Log("Starting server...");
        NetworkTransport.Init();

        ConnectionConfig cc = new ConnectionConfig();
        cc.AddChannel(QosType.Reliable);

        HostTopology topo = new HostTopology(cc, MAX_USERS);

        // Server only
        
        hostId = NetworkTransport.AddHost(topo, PORT, SERVER_IP);
        webHostId = NetworkTransport.AddWebsocketHost(topo, WEB_PORT, SERVER_IP); // Allows connection trough browser

        // Set array
        clientsActive = new bool[MAX_USERS];

        isInit = true;
        Debug.Log(string.Format("Opening connection on port {0} and webport {1} on ip {2}", PORT, WEB_PORT, SERVER_IP));
    }

    #region OnData

    /// <summary>
    /// Check what message it is and what to do with it
    /// </summary>
    /// <param name="connectionId">The client connection id</param>
    /// <param name="channelId"></param>
    /// <param name="receiveHostId"></param>
    /// <param name="msg"></param>
    private void OnData(int connectionId, int channelId, int receivingHostId, NetMsg msg)
    {
        Debug.Log("Receive message of type: " + msg.OperationCode);

        switch(msg.OperationCode)
        {
            case NetOperationCode.None: break;
            case NetOperationCode.CreatePlayer:
                // Create a player
                Net_CreatePlayer cp = (Net_CreatePlayer)msg;
                Debug.Log("Creating new player named: " + cp.Name);
                clientsActive[connectionId] = true;
                // Send to client that he joined the game
                SendClient(receivingHostId, connectionId, new Net_ClientJoined());

                // send to every client that a new player has joined, except the client that joined
                for(int i = 0; i < clientsActive.Length; i++)
                {
                    if(clientsActive[i] && i != connectionId)
                    {
                        SendClient(receivingHostId, i, new Net_PlayerJoined(connectionId, cp.Name));
                        Debug.Log("Send message to user: " + i);
                    }
                }
                break;

            default: Debug.LogWarning("NetMsg not included! " + msg.OperationCode);
                break;
        }
    }

    #endregion
}
