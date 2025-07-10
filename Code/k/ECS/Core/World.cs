using System;
using System.Collections.Generic;
using System.Linq;

namespace Sandbox.k.ECS.Core;

public class World
{
	private readonly EntityManager _entityManager = new();
	private readonly ComponentStorageContainer _componentStorages = new();
	private readonly Dictionary<int, int> _entityComponentCounts = new();
	private static World _default;

	public ComponentStorageContainer ComponentStorages => _componentStorages;
	public EntityManager EntityManager => _entityManager;
	public static World Default => _default ??= new();
	
	public static void ResetDefault() => _default = new();
	
	// Entity management
	public int CreateEntity() => _entityManager.CreateEntity();
	
	public void DestroyEntity(int entity)
	{
		if (!_entityManager.IsAlive(entity)) return;

		foreach ( var (_, storage) in _componentStorages.Values )
		{
			storage.Remove( entity );
		}
		
		_entityComponentCounts.Remove(entity);
		_entityManager.DestroyEntity(entity);
	}
	
	// Component management
	public void AddComponent<T>(int entity, T component) where T : struct
	{
		var storage = GetOrCreateStorage<T>();
		if ( storage == null )
		{
			Log.Error( $"Could not create storage for component {typeof(T).Name}" );
			return;
		}
		storage.Add(entity, component);
		IncrementComponentCount(entity);
	}
	
	public void RemoveComponent<T>(int entity) where T : struct
	{
		if (HasComponent<T>(entity))
		{
			GetStorage<T>().Remove(entity);
			DecrementComponentCount(entity);
		}
	}
	
	private void IncrementComponentCount(int entity)
	{
		if (!_entityComponentCounts.ContainsKey(entity))
			_entityComponentCounts[entity] = 0;
		_entityComponentCounts[entity]++;
	}
	
	private void DecrementComponentCount(int entity)
	{
		if (!_entityComponentCounts.ContainsKey(entity))
			return;

		_entityComponentCounts[entity]--;
		if (_entityComponentCounts[entity] <= 0)
		{
			_entityComponentCounts.Remove(entity);
			DestroyEntity(entity);
		}
	}
	
	public bool HasComponent(Type type, int entity)
	{
		if (!_componentStorages.TryGetValue(type, out var storage))
			return false;
			
		return storage.Has(entity);
	}
	
	public bool HasComponent<T>(int entity) where T : struct
	{
		if (!_componentStorages.TryGetValue(typeof(T), out var storage))
			return false;
			
		return storage.Has(entity);
	}
	
	public T GetComponent<T>(int entity) where T : struct => GetStorage<T>().Get(entity);
	
	public ref T GetComponentRef<T>(int entity) where T : struct => ref GetStorage<T>().GetRef(entity);
	
	// Storage management
	private ComponentStorage<T> GetStorage<T>() where T : struct
	{
		if (!_componentStorages.TryGetValue(typeof(T), out var storage))
			throw new KeyNotFoundException($"Trying to get component {typeof(T).Name} that does not exist on the entity");
			
		return (ComponentStorage<T>)storage;
	}
	
	private ComponentStorage<T> GetOrCreateStorage<T>() where T : struct
	{
		if (!_componentStorages.TryGetValue(typeof(T), out var storage))
		{
			var instance = new ComponentStorage<T>();
			storage = instance;
			_componentStorages.AddStorage( instance );
		}
		
		return (ComponentStorage<T>)storage;
	}
	
	// Systems query shortcuts
	public IEnumerable<int> GetEntitiesWith<T>() where T : struct
	{
		if (!_componentStorages.TryGetValue(typeof(T), out var storage))
			return Enumerable.Empty<int>();
			
		return ((ComponentStorage<T>)storage).GetAllEntities();
	}
	
	public IEnumerable<int> GetEntitiesWithout<T>() where T : struct
	{
		var entities = new HashSet<int>();
		foreach ( var (key, storage) in _componentStorages.Values )
		{
			if ( key == typeof(T) ) continue;
			foreach ( var entity in storage.GetAllEntities() )
			{
				entities.Add( entity );
			}
		}
		
		return entities;
	}
	
	public IEnumerable<int> GetEntitiesWith<T1, T2>() where T1 : struct where T2 : struct => GetEntitiesWith<T1>().Intersect(GetEntitiesWith<T2>());
	
	public IEnumerable<int> GetFilter(IEnumerable<int> withFilter, IEnumerable<int> withoutFilter) => withFilter.Except(withoutFilter);
}
