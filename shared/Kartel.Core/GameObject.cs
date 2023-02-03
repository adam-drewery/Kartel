using System.Runtime.Serialization;
using Kartel.Entities;
using Kartel.Observables;
using Serilog;

namespace Kartel;

[DataContract]
public abstract class GameObject : Observable
{
	protected GameObject(IGame game)
	{
		Game = game;
		Log = Serilog.Log.ForContext(GetType());
	}

	protected ILogger Log { get; }
	
	internal IGame Game { get; set; }

	public override int GetHashCode() => Id.GetHashCode();
}