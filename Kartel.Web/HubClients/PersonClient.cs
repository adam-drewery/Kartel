using Kartel.Entities;
using Kartel.Web.HubClients.Base;
using Microsoft.AspNetCore.SignalR.Client;

namespace Kartel.Web.HubClients;

public class PersonClient : EntityClient<Person>
{
    public PersonClient(HubConnectionBuilder builder, IConfiguration config) : base(builder, config)
    {
    }
}