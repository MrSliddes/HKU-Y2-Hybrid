/// <summary>
/// Tells the client that a player joined (create object for that player)
/// </summary>
[System.Serializable]
public class Net_PlayerJoined : NetMsg
{
    public int otherClientId;
    public string otherClientName;

    public Net_PlayerJoined(int otherClientId, string otherClientName)
    {
        OperationCode = NetOperationCode.PlayerJoinedGame;
        this.otherClientId = otherClientId;
        this.otherClientName = otherClientName;
    }
}
