using System.Runtime.Serialization;
using Kartel.Entities;

namespace Kartel;

[DataContract]
public abstract class GameObject : Observable
{
	internal Game Game { get; set; }

	public override int GetHashCode() => Id.GetHashCode();
}