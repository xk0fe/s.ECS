using Sandbox.k.ECS.Core;

namespace Sandbox.k.ECS.Extensions.Utils;

public static class EntityExtensions
{
	public static void AddComponent<T>( this int entity ) where T : struct
	{
		World.Default.AddComponent<T>( entity, default );
	}

	public static void SetComponent<T>( this int entity, T component ) where T : struct
	{
		var world = World.Default;
		if ( world.HasComponent<T>( entity ) )
		{
			world.RemoveComponent<T>( entity );
		}

		world.AddComponent( entity, component );
	}

	public static void RemoveComponent<T>( this int entity ) where T : struct
	{
		World.Default.RemoveComponent<T>( entity );
	}

	public static ref T GetComponent<T>( this int entity ) where T : struct
	{
		return ref World.Default.GetComponentRef<T>( entity );
	}

	public static bool HasComponent<T>( this int entity ) where T : struct
	{
		return World.Default.HasComponent<T>( entity );
	}

	public static bool TryGetComponent<T>( this int entity, out T component ) where T : struct
	{
		if ( World.Default.HasComponent<T>( entity ) )
		{
			component = World.Default.GetComponentRef<T>( entity );
			return true;
		}

		component = default;
		return false;
	}
}
