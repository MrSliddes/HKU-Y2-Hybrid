using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Client : MonoBehaviour
{
    // Constants
    private const int MAX_CONNECTIONS = 20;
    private const string SERVER_IP = "127.0.0.1"; // 127.0.0.1 is localHost
    private const int SERVER_PORT = 8999;
    private const int SERVER_WEB_PORT = 8998;
    private const int BUFFER_SIZE = 1024; // Your bol.com box you can ship data in

    // Channels
    protected int reliableChannelId; // (!important) Example purchase an item
    protected int unreliableChannelId; // (Not really important) Example updating movement of other players

    // Host
    private int hostId;
    private int connectionId;

    // Logic
    private byte error;
    private byte[] buffer = new byte[BUFFER_SIZE];
    private bool isConnected;

    private void Start()
    {
        GlobalConfig config = new GlobalConfig();
        NetworkTransport.Init(config);

        // Host topology
        ConnectionConfig cc = new ConnectionConfig();
        reliableChannelId = cc.AddChannel(QosType.Reliable);
        unreliableChannelId = cc.AddChannel(QosType.Unreliable);
        HostTopology topo = new HostTopology(cc, MAX_CONNECTIONS);

        // Connecting to host
        hostId = NetworkTransport.AddHost(topo, 0);

#if UNITY_WEBGL
        // WebGL client
        connectionId = NetworkTransport.Connect(hostId, SERVER_IP, SERVER_WEB_PORT, 0, out error);

#else
        // Standalone Client
        connectionId = NetworkTransport.Connect(hostId, SERVER_IP, SERVER_PORT, 0, out error);
#endif
    }
}
