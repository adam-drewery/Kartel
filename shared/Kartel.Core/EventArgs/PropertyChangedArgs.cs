using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Kartel.Entities;
using Kartel.Observables;

namespace Kartel.EventArgs;

public class PropertyChangedArgs
{
	public PropertyChangedArgs(Observable source, string propertyName, object? newValue)
		: this(source.Id, propertyName, newValue) => Source = source;

	public PropertyChangedArgs(Guid sourceId, string propertyName, object? newValue)
	{
		SourceId = sourceId;
		PropertyName = propertyName;
		NewValue = newValue;
	}
	
	[IgnoreDataMember]
	public Observable? Source { get; }
		
	public Guid SourceId { get; }

	public string PropertyName { get; }

	public object? NewValue { get; }
	
	public QueueChangeType? QueueChangeType { get; set; } 

	public void ApplyTo(object target)
	{
		var subTarget = target;
		PropertyInfo? property = null;
		var parts = PropertyName.Split('.');

		if (!parts.Any())
			throw new InvalidDataException($"Property path {PropertyName} not valid.");

		for (var index = 0; index < parts.Length; index++)
		{
			if (subTarget == null)
				throw new InvalidDataException($"Couldn't find target to apply path {PropertyName} to.");
			
			var propertyName = parts[index];
			property = GetProperty(subTarget, propertyName);

			if (property == null)
				throw new InvalidDataException(
					$"Couldn't find part {propertyName} of path {PropertyName} on object {subTarget.GetType()}.");

			// Don't do this if its the last part. We need the target to be the parent object of the property
			if (index != parts.Length - 1)
				subTarget = property.GetValue(subTarget);
		}

		if (property == null)
			throw new MissingMemberException($"Failed to find property with name {PropertyName} on type {subTarget}");
		
		if (property.PropertyType.IsAssignableTo(typeof(ObservableQueue)))
		{
			var queue = (ObservableQueue?)property.GetValue(subTarget);

			if (queue == null)
				throw new MissingMemberException($"Failed to find property of type {typeof(ObservableQueue)} with name {PropertyName}");

			if (!QueueChangeType.HasValue)
				throw new InvalidDataException($"Queue change type was not set for property {PropertyName}.");
			
			switch (QueueChangeType.Value)
			{
				case EventArgs.QueueChangeType.Add:

					if (NewValue == null)
						throw new InvalidDataException("Cannot enqueue a null object.");
					
					queue.EnqueueObject(NewValue);
					break;
				
				case EventArgs.QueueChangeType.Remove:
					queue.DequeueObject();
					break;
				
				case EventArgs.QueueChangeType.Clear:
					queue.Clear();
					break;
				
				default:
					throw new ArgumentOutOfRangeException();
			}
			
			return;
		}
		
		property.SetValue(subTarget, NewValue);
	}

	private static PropertyInfo? GetProperty(object target, string propertyName)
	{
		return target.GetType().GetProperty(propertyName,
			BindingFlags.Instance
			| BindingFlags.FlattenHierarchy
			| BindingFlags.NonPublic
			| BindingFlags.Public);
	}
}