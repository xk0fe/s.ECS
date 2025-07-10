using System.Collections.Generic;

namespace Sandbox.k.ECS.Core;

public class EntityManager
{
	private int _nextEntityId;
	public HashSet<int> Entities { get; } = new();
	
	public int CreateEntity()
	{
		var entity = _nextEntityId++;
		Entities.Add(entity);
		return entity;
	}
	
	public void DestroyEntity(int entity) => Entities.Remove(entity);
	
	public bool IsAlive(int entity) => Entities.Contains(entity);
}
