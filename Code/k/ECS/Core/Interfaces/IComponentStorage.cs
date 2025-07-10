using System.Collections.Generic;

namespace Sandbox.k.ECS.Core.Interfaces;

public interface IComponentStorage
{
	public void Remove(int entity);
	public bool Has(int entity);
	IEnumerable<int> GetAllEntities();
}
