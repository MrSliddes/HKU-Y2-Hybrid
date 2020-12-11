using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//https://www.youtube.com/watch?v=LVve99EuECM
public class MasterServer : MonoBehaviour
{
    // Constants
    private const int MAX_CONNECTIONS = 20;
    private const string SERVER_IP = "127.0.0.1"; // 127.0.0.1 is localHost // browser webgl http://localhost:55778/
    private const int SERVER_PORT = 8999;
    private const int SERVER_WEB_PORT = 8998;
    private const int BUFFER_SIZE = 1024; // Your bol.com box you can ship data in

    // Channels
    protected int reliableChannelId; // (!important) Example purchase an item
    protected int unreliableChannelId; // (Not really important) Example updating movement of other players

    // Host
    private int hostId;
    private int webHostId;

    // Logic
    private byte[] buffer = new byte[BUFFER_SIZE];
    /// <summary>
    /// Is the MasterServer initialized?
    /// </summary>
    private bool isInit;

    private void Start()
    {
        GlobalConfig config = new GlobalConfig();
        NetworkTransport.Init(config);

        // Host topology
        ConnectionConfig cc = new ConnectionConfig();
        reliableChannelId =  cc.AddChannel(QosType.Reliable);
        unreliableChannelId = cc.AddChannel(QosType.Unreliable);
        HostTopology topo = new HostTopology(cc, MAX_CONNECTIONS);

        // Adding hosts
        hostId = NetworkTransport.AddHost(topo, SERVER_PORT, SERVER_IP); // #EDIT
        webHostId = NetworkTransport.AddWebsocketHost(topo, SERVER_WEB_PORT);

        isInit = true;
        Debug.Log("Started MasterServer");
    }

    private void Update()
    {
        if(!isInit) return;

        int outHostId, outConnectionId, outChannelId;
        int receivedSize;
        byte error;

        NetworkEventType e = NetworkTransport.Receive(out outHostId, out outConnectionId, out outChannelId, buffer, buffer.Length, out receivedSize, out error);

        
        switch(e)
        {
            case NetworkEventType.ConnectEvent:
                // This is where we create a user
                Debug.Log("Connection from: " + outConnectionId + " through the channel: " + outChannelId);
                break;
            case NetworkEventType.DisconnectEvent:
                // This is where we destroy a user
                Debug.Log("Disconnection from: " + outConnectionId + " through the channel: " + outChannelId);
                break;
            case NetworkEventType.DataEvent:
                Debug.Log("Data from: " + outConnectionId + " through the channel: " + outChannelId + " Message: " + System.Text.Encoding.UTF8.GetString(buffer));
                break;
            case NetworkEventType.BroadcastEvent:
                break;
            case NetworkEventType.Nothing:
                break;
            default: Debug.LogError("This cant happen");
                break;
        }
    }
}
