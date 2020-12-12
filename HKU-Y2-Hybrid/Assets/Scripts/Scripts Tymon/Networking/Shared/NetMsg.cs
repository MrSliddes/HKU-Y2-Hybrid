using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The client and server use this to identify messages. This is not secure as a client can use this to send false information
[System.Serializable] // All msg have to be serilizable
public abstract class NetMsg
{
    /// <summary>
    /// Single number that will show what the message is
    /// </summary>
    public byte OperationCode { set; get; }

    public NetMsg()
    {
        OperationCode = NetOperationCode.None;
    }

}

public static class NetOperationCode
{
    public const int None = 0;
    public const int CreatePlayer = 1;
    public const int ClientJoinedGame = 2; // message back to client that he joined game
    public const int PlayerJoinedGame = 3; // message to other clients that already where in the game that a new player joined
}