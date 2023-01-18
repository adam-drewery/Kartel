namespace Kartel.Api.Hubs.Base.Interfaces;

public interface IEntityHub<T>
{
	Task<T> Subscribe(Guid id);

	Task Unsubscribe(Guid id);
}