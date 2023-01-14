using Kartel.Web.HubClients.Base;
using Microsoft.AspNetCore.SignalR.Client;

namespace Kartel.Web.HubClients;

public class GameClient : HubClient
{
    public GameClient(HubConnectionBuilder builder, IConfiguration config) : base(builder, config)
    {
        Connection.On<DateTime>("ReceiveTime", s =>
        {
            OnReceiveTime(s);
            return Task.CompletedTask;
        });
    }

    protected virtual void OnReceiveTime(DateTime time) => ReceiveTime?.Invoke(this, time);

    public event EventHandler<DateTime> ReceiveTime;
}