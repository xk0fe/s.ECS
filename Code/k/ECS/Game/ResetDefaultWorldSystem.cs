using Sandbox.k.ECS.Core;

namespace Sandbox.k.ECS.Game;

public class ResetDefaultWorldSystem : GameObjectSystem
{
	public ResetDefaultWorldSystem(Scene scene) : base(scene)
	{
		World.ResetDefault();
		Log.Info( "ECS - Default World reset called!" );
	}
}
