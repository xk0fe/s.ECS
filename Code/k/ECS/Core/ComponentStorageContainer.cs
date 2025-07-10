using System;
using System.Collections.Generic;
using Sandbox.k.ECS.Core.Common;
using Sandbox.k.ECS.Core.Interfaces;

namespace Sandbox.k.ECS.Core;

public class ComponentStorageContainer
{
	private readonly Dictionary<Type, IComponentStorage> _storages = new();

	public void AddStorage<T>(ComponentStorage<T> storage) where T : struct
	{
		_storages.Add(typeof(T), storage);
	}
	
	public void SetStorage<T>(ComponentStorage<T> storage) where T : struct 
	{
		_storages[typeof(T)] = storage;
	}
	
	public ComponentStorage<T> GetStorage<T>() where T : struct 
	{
		return (ComponentStorage<T>)_storages[typeof(T)];
	}
	
	public bool HasStorage<T>() where T : struct
	{
		return _storages.ContainsKey(typeof(T));
	}
	
	public bool TryGetValue(Type type, out IComponentStorage storage)
	{
		return _storages.TryGetValue(type, out storage);
	}
	
	public IEnumerable<KeyValuePair<Type, IComponentStorage>> Values => _storages;
}
