namespace Rai4.Application.Services.Interfaces;

public interface IConnectedClientManager
{
    void AddClient(int id);
    void RemoveClient(int id);
    IReadOnlyList<int> GetConnectedClients();
}