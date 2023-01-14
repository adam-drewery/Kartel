﻿using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Kartel.Json;

public class PrivatePropertyContractResolver : DefaultContractResolver 
{
	protected override JsonProperty CreateProperty(
		MemberInfo member,
		MemberSerialization memberSerialization)
	{
		//TODO: Maybe cache
		var prop = base.CreateProperty(member, memberSerialization);

		if (prop.Writable) return prop;
		var property = member as PropertyInfo;
		if (property == null) return prop;
		var hasPrivateSetter = property.GetSetMethod(true) != null;
		prop.Writable = hasPrivateSetter;

		return prop;
	}
}