# FeatureBase
Feature is a class that contains a list of systems related to a specific ... feature. 
For example, a feature could be "combat", and the systems related to that feature could be "attack", "defend", etc.

Example:
```cs
public class CombatFeature : FeatureBase
{
    public CombatFeature()
    {
        Add(new AttackSystem());
        Add(new DefenseSystem());
    }
}
```

# SystemBase
System is a class that contains a filter and operates on entities that match that filter.

Example:
```cs
public class AttackSystem : SystemBase
{
    private EntityFilter _filter;
    
    public override bool Initialize()
    {
        _filter = new EntityFilter( World.Default ).With<AttackComponent>();
    }
    
    public override void Update(float deltaTime)
    {
        foreach (var entity in _filter)
        {
            ref var attackComponent = ref entity.Get<AttackComponent>();
            // Perform attack logic here
        }
    }
}
```

# InitializerBase
Initializer base class allows us to easily manage game features.

Example:
```cs
public class MyInitializer : InitializerBase
{
    protected override void OnAwake()
    {
        AddFeature( new CombatFeature() );
        base.OnAwake();
    }
}
```