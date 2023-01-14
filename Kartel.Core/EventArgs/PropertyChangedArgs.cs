using System;
using Kartel.Entities;

namespace Kartel.EventArgs;

public class PropertyChangedArgs
{
	public PropertyChangedArgs(Observable source, string propertyName, object newValue)
	{
		Source = source;
		SourceId = source.Id;
		PropertyName = propertyName;
		NewValue = newValue;
	}
		
	public Observable Source { get; }
		
	public Guid SourceId { get; }

	public string PropertyName { get; }

	public object NewValue { get; }
}