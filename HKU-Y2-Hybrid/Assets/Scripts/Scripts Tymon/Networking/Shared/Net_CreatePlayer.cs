[System.Serializable]
public class Net_CreatePlayer: NetMsg
{
    public string name;

    public Net_CreatePlayer(string name)
    {
        OperationCode = NetOperationCode.CreatePlayer;
        this.name = name;
    }
}
