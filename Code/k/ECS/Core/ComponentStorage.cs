using System.Collections.Generic;
using Sandbox.k.ECS.Core.Common;
using Sandbox.k.ECS.Core.Interfaces;

namespace Sandbox.k.ECS.Core;

public class ComponentStorage<T> : IComponentStorage where T : struct
{
	private readonly SparseSet<T> _components = new();
    
	public void Add(int entity, T component) => _components.Add(entity, component);
	public void Remove(int entity) => _components.Remove(entity);
	public bool Has(int entity) => _components.Contains(entity);
	public T Get(int entity) => _components[entity];
	public ref T GetRef(int entity) => ref _components.GetRef(entity);
	
	public IEnumerable<KeyValuePair<int, T>> GetAllComponents() => _components.GetAll();
	public IEnumerable<int> GetAllEntities() => _components;
}
