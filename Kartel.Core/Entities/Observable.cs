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
    [DataMember] public Guid Id { get; set; } = Guid.NewGuid();
    
    public void Write<T>(T value, [CallerMemberName] string caller = "")
    {
        Properties[caller] = value;
        PropertyChanged?.Invoke(this, new PropertyChangedArgs(this, caller, value));
    }

    public T Read<T>([CallerMemberName] string caller = "")
    {
        if (Properties.TryGetValue(caller, out var result))
            return (T)result;

        return default;
    }
    
    public readonly IDictionary<string, object> Properties = new ConcurrentDictionary<string, object>();
		
    public event EventHandler<PropertyChangedArgs> PropertyChanged;
}