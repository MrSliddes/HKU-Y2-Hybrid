using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Text.RegularExpressions;

//https://www.youtube.com/watch?v=amy3L3pGWH0&list=PLDrcLKDFENwI0Bg6bwNuPcP0K-xDAAsrc&index=2
public class Client : MonoBehaviour
{
    // Join server menu
    public GameObject connectToServerButton;
    public GameObject menuConnectToServer;
    public GameObject menuJoinGame;

    private const int MAX_USER = 100;
    private const int PORT = 26000;
    private const int WEB_PORT = 26001;
    private const int BYTE_SIZE = 1024;
    private string SERVER_IP = "127.0.0.1"; // 127.0.0.1 is local host

    private byte reliableChannel;
    private byte error;
    private int hostId;
    private int connectionId;

    private bool isInit = false;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        //Init();
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
                Debug.Log("Connected to server.");
                // Enter name, then join game
                menuConnectToServer.SetActive(false);
                menuJoinGame.SetActive(true);
                break;
            case NetworkEventType.DisconnectEvent:
                Debug.Log("Disconnected from server.");
                connectToServerButton.SetActive(true);
                menuConnectToServer.SetActive(true);
                menuJoinGame.SetActive(false);
                break;
            case NetworkEventType.Nothing:
                break;
            case NetworkEventType.BroadcastEvent:
                break;
            default:
                break;
        }
    }

    public void JoinServer(TextMeshProUGUI t)
    {
        //if(Regex.IsMatch(t.text, @" ,")) return;
        connectToServerButton.SetActive(false);
        SERVER_IP = (string)t.text.Trim((char)8203); // Remove stupid invis char
        print("server ip by client: " + SERVER_IP);
        Init();
    }

    public void JoinGame(TextMeshProUGUI t)
    {
        SendServer(new Net_CreatePlayer(t.text));

        // Client now needs conformation that he joined
    }

    #region Send

    public void SendServer(NetMsg msg)
    {
        // This is where we hold our data
        byte[] buffer = new byte[BYTE_SIZE];

        // Convert data into a byte[]
        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream ms = new MemoryStream(buffer);
        formatter.Serialize(ms, msg);

        // Send to server
        NetworkTransport.Send(hostId, connectionId, reliableChannel, buffer, BYTE_SIZE, out error);
    }

    // Messages
    public void MsgCreatePlayer()
    {
        SendServer(new Net_CreatePlayer("name"));
    }

    #endregion

    private void Init()
    {
        isInit = false;
        Debug.Log("Starting client...");
        NetworkTransport.Init();

        ConnectionConfig cc = new ConnectionConfig();
        cc.AddChannel(QosType.Reliable);

        HostTopology topo = new HostTopology(cc, MAX_USER);

        // Client only code
        hostId = NetworkTransport.AddHost(topo, 0);
#if UNITY_WEBGL && !UNITY_EDITOR
        // Webclient client
        connectionId = NetworkTransport.Connect(hostId, SERVER_IP, WEB_PORT, 0, out error);
        Debug.Log("Connecting from web");
#else
        // Standalone client
        connectionId = NetworkTransport.Connect(hostId, SERVER_IP, PORT, 0, out error);
        Debug.Log("Connecting from standalone " + error);
#endif
        isInit = true;
        Debug.Log(string.Format("Attempting to connect on {0}...", SERVER_IP));
    }

    /// <summary>
    /// Receive from server
    /// </summary>
    /// <param name="connectionId"></param>
    /// <param name="channelId"></param>
    /// <param name="receiveHostId"></param>
    /// <param name="msg"></param>
    private void OnData(int connectionId, int channelId, int receiveHostId, NetMsg msg)
    {
        Debug.Log("Receive message of type: " + msg.OperationCode);
    }

}

