using System.Collections.Generic;

namespace Sandbox.k.ECS.Extensions;

public class FeatureBase
{
	public bool IsEnabled { get; set; } = true;
	
	private readonly List<SystemBase> _systems = new List<SystemBase>();
	private bool _isInitialized;
	
	public void AddSystem( SystemBase systemBase )
	{
		_systems.Add( systemBase );
	}
	
	public virtual void Initialize()
	{
		foreach ( var system in _systems )
		{
			system.Initialize();
		}
		_isInitialized = true;
	}

	public virtual void Update( float deltaTime )
	{
		if ( !_isInitialized || !IsEnabled ) return;
		foreach ( var system in _systems )
		{
			system.Update( deltaTime );
		}
	}
}
