# k.ECS

A lightweight, high-performance Entity Component System (ECS) for [s&box](https://sbox.game/), designed for developers who want efficient, modular, and scalable game logic.  
For full documentation, visit [sbox.game/kfe/kecs](https://sbox.game/kfe/kecs/).

---

## Overview

**k.ECS** enables efficient game development by separating data (components) from behavior (systems) and using entities as simple IDs to group components. It is built for performance, clarity, and easy integration with s&box projects.

---

## Core Concepts

- **Entities:** Simple integer IDs, act as containers for components.
- **Components:** Pure data structs (no logic), attached to entities.
- **Systems:** Implement game logic, process entities with required components.
- **World:** Contains and manages all entities, components, and systems. Supports multiple worlds for different game states or client/server logic.

---

## Core Components

### World

- Manages entity and component lifecycles.
- Entity creation, destruction, and component management.
- Entity filtering for efficient queries.
- Example:
  ```csharp
  var world = World.Default;
  int entity = world.CreateEntity();
  world.AddComponent(entity, new PositionComponent { X = 10, Y = 20 });
  ```

### EntityManager

- Creates/destroys entities and tracks their existence.

### ComponentStorage<T>

- Efficiently stores components for each entity.
- O(1) add, remove, get, and reference operations.

### ComponentStorageContainer

- Manages all component storages in a world.
- Type-safe and dynamic.

### EntityFilter

- Query/filter entities by their components.
- Fluent API for complex queries.
  ```csharp
  var filter = new EntityFilter(world).With<PositionComponent>().With<VelocityComponent>();
  foreach (var entity in filter) { ... }
  ```

### SparseSet<T>

- Highly efficient storage for entity-value pairs.
- O(1) operations, memory efficient, reference access.

---

## Extensions

- **FeatureBase:** Groups systems related to a feature (e.g., "Combat").
- **SystemBase:** Implements logic for a filtered set of entities.
  ```csharp
  public class AttackSystem : SystemBase
  {
      private EntityFilter _filter;
      public override bool Initialize() => _filter = new EntityFilter(World.Default).With<AttackComponent>();
      public override void Update(float deltaTime)
      {
          foreach (var entity in _filter)
          {
              ref var attack = ref entity.Get<AttackComponent>();
              // attack logic
          }
      }
  }
  ```

---

## Best Practices

1. Use structs for components (no classes).
2. Data-only components—no logic.
3. Use `ref` returns for direct modification.
4. Batch entity/component operations.
5. Use and reuse filters for queries.

---

## Example Usage

```csharp
var world = World.Default;
int entityId = world.CreateEntity();
world.AddComponent(entityId, new PositionComponent { X = 10, Y = 20 });
world.AddComponent(entityId, new VelocityComponent { X = 5, Y = 0 });
world.AddComponent(entityId, new HealthComponent { Value = 100 });

var filter = new Filter(world)
    .With<PositionComponent>()
    .With<VelocityComponent>();

foreach (var entity in filter)
{
    ref var pos = ref world.GetComponentRef<PositionComponent>(entity);
    ref var vel = ref world.GetComponentRef<VelocityComponent>(entity);
    pos.X += vel.X;
    pos.Y += vel.Y;
}
```

---

## Documentation

- See the full API and guides at [https://sbox.game/kfe/kecs/](https://sbox.game/kfe/kecs/)
- For questions or issues, use [GitHub Issues](https://github.com/xk0fe/s.ECS/issues)

---

## License

MIT (or see LICENSE file)
