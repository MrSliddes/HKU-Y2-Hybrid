/// <summary>
/// Send to server that it needs to create a new player (client wants to join)
/// </summary>
[System.Serializable]
public class Net_CreatePlayer: NetMsg
{
    public string Name { set; get; }


    public Net_CreatePlayer()
    {
        OperationCode = NetOperationCode.CreatePlayer;
    }
}