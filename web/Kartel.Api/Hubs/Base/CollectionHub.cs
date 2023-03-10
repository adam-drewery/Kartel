using System.Diagnostics.CodeAnalysis;

namespace Kartel.Api.Hubs.Base;

public abstract class CollectionHub<T> : BaseHub
{
	protected CollectionHub(Game game) : base(game) { }

	protected abstract Task<List<T>> LoadData(Guid id);

	[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Used implicitly by SignalR.")]
	public async Task<List<T>> Subscribe(Guid id)
	{
		await Groups.AddToGroupAsync(Context.ConnectionId, id.ToString());
		return await LoadData(id);
	}

	[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Used implicitly by SignalR.")]
	public Task Unsubscribe(Guid id)
	{
		return Groups.RemoveFromGroupAsync(Context.ConnectionId, id.ToString());
	}
}