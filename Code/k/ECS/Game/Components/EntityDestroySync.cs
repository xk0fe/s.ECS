using Sandbox.k.ECS.Core;

namespace Sandbox.k.ECS.Game.Components;

public class EntityDestroySync : Component
{
	protected override void OnDestroy()
	{
		base.OnDestroy();
		var entityLink = GetComponent<EntityProviderLink>();
		if (entityLink.IsValid())
		{
			var world = World.Default;
			world.DestroyEntity(entityLink.EntityId);
			entityLink.EntityId = -1;
		}
	}
}
