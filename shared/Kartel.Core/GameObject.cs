using System.Runtime.Serialization;
using Kartel.Entities;
using Serilog;

namespace Kartel;

[DataContract]
public abstract class GameObject : Observable
{
	protected GameObject() => Log = Serilog.Log.ForContext(GetType());

	protected ILogger Log { get; }
	
	internal Game Game { get; set; }

	public override int GetHashCode() => Id.GetHashCode();
}