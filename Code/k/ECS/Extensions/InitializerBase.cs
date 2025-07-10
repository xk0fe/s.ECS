using System.Collections.Generic;

namespace Sandbox.k.ECS.Extensions;

public class InitializerBase : Component
{
	private readonly List<FeatureBase> _features = new List<FeatureBase>();
	
	protected void AddFeature( FeatureBase featureBase )
	{
		_features.Add( featureBase );
	}
	
	protected override void OnAwake()
	{
		base.OnAwake();
		foreach ( var feature in _features )
		{
			if ( !feature.IsEnabled ) continue;
			feature.Initialize();
		}
	}
	
	protected override void OnUpdate()
	{
		base.OnUpdate();
		var deltaTime = Time.Delta;
		foreach ( var feature in _features )
		{
			feature.Update( deltaTime );
		}
	}
}
