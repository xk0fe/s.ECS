namespace Sandbox.k.ECS.Game.Components;

public class EntityProviderLink : Component
{
	[ReadOnly, Property] public int EntityId { get; set; }
}
