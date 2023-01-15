using System;
using System.IO;
using System.Linq;
using System.Reflection;
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

	public void ApplyTo(object target)
	{
		var property = GetProperty(target, PropertyName);

		if (property == null)
			throw new MissingMemberException($"Failed to find property with name {PropertyName} on type {target}");
		
		property.SetValue(target, NewValue);
	}

	private static PropertyInfo GetProperty(object target, string path)
	{
		PropertyInfo property = null;
		var parts = path.Split('.');

		if (!parts.Any())
			throw new InvalidDataException($"Property path {path} not valid.");
		
		foreach(var propertyName in parts)
		{
			property = target.GetType().GetProperty(propertyName, BindingFlags.Instance);
			
			if (property == null)
				throw new InvalidDataException($"Couldn't find part {propertyName} of path {path}.");
			
			target = property.GetValue(target);
		}

		return property;
	}
}