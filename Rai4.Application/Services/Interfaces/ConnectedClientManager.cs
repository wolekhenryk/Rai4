namespace Rai4.Application.Services.Interfaces;

public class ConnectedClientManager : IConnectedClientManager
{
    private readonly HashSet<int> _connectedClients = [];

    public void AddClient(int id)
    {
        lock (_connectedClients)
        {
            _connectedClients.Add(id);
        }
    }

    public void RemoveClient(int id)
    {
        lock (_connectedClients)
        {
            _connectedClients.Remove(id);
        }
    }

    public IReadOnlyList<int> GetConnectedClients()
    {
        lock (_connectedClients)
        {
            return _connectedClients.ToList();
        }
    }
}