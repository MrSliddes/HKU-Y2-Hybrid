[System.Serializable]
public class Net_ClientJoined : NetMsg
{
    // send players already joined + server info to client

    public Net_ClientJoined()
    {
        OperationCode = NetOperationCode.ClientJoinedGame;
    }
}
