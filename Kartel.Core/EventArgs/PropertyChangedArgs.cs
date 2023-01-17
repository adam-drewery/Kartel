using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Kartel.Entities;

namespace Kartel.EventArgs;

public class PropertyChangedArgs
{
	public PropertyChangedArgs(Observable source, string propertyName, object newValue)
		: this(source.Id, propertyName, newValue)
	{
		Source = source;
	}
		
	public PropertyChangedArgs(Guid sourceId, string propertyName, object newValue)
	{
		SourceId = sourceId;
		PropertyName = propertyName;
		NewValue = newValue;
	}
		
	public Observable Source { get; }
		
	public Guid SourceId { get; }

	public string PropertyName { get; }

	public object NewValue { get; }

	public void ApplyTo(object target)
	{
		if (target == null) throw new ArgumentNullException(nameof(target));
		
		PropertyInfo property = null;
		var parts = PropertyName.Split('.');

		if (!parts.Any())
			throw new InvalidDataException($"Property path {PropertyName} not valid.");

		for (var index = 0; index < parts.Length; index++)
		{
			var propertyName = parts[index];
			property = GetProperty(target, propertyName);

			if (property == null)
				throw new InvalidDataException(
					$"Couldn't find part {propertyName} of path {PropertyName} on object {target.GetType()}.");

			// Don't do this if its the last part. We need the target to be the parent object of the property
			if (index != parts.Length - 1)
				target = property.GetValue(target);
		}

		if (property == null)
			throw new MissingMemberException($"Failed to find property with name {PropertyName} on type {target}");
		
		property.SetValue(target, NewValue);
	}

	private static PropertyInfo GetProperty(object target, string propertyName)
	{
		return target.GetType().GetProperty(propertyName,
			BindingFlags.Instance
			| BindingFlags.FlattenHierarchy
			| BindingFlags.NonPublic
			| BindingFlags.Public);
	}
}