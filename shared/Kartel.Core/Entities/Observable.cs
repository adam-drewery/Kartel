using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Kartel.EventArgs;

namespace Kartel.Entities;

[DataContract]
public class Observable
{
    [DataMember] public Guid Id { get; } = Guid.NewGuid();

    protected void Write<T>(T? value, [CallerMemberName] string caller = "")
    {
        if (Properties.TryGetValue(caller, out var current))
            if (current?.Equals(value) ?? value == null)
                return;
        
        Properties[caller] = value;
        PropertyChanged?.Invoke(this, new PropertyChangedArgs(this, caller, value));
    }

    protected T? Read<T>([CallerMemberName] string caller = "")
    {
        if (Properties.TryGetValue(caller, out var result))
            return (T?)result;

        return default;
    }

    protected void OnPropertyChanged(string caller, object? value)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedArgs(this, caller, value));
    }
    
    protected void OnPropertyChanged(string caller, object? value, QueueChangeType queueChangeType)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedArgs(this, caller, value) { QueueChangeType = queueChangeType });
    }
    
    public readonly IDictionary<string, object?> Properties = new ConcurrentDictionary<string, object?>();
		
    public event EventHandler<PropertyChangedArgs>? PropertyChanged;
}