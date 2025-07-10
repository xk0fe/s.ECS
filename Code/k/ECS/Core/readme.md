# k.ECS Core Documentation

This document provides an overview of the core components of the k.ECS (Entity Component System) framework.

## Overview

k.ECS is a lightweight, high-performance Entity Component System designed for game development. It follows the ECS architecture pattern, separating data (components) from behavior (systems) and using entities as simple identifiers to group components together.

## Core Components

### World

The `World` class is the main container for all entities and components. It manages the lifecycle of entities and components, including creation, destruction, and updates.

Key features:
- Entity creation and destruction
- Component management (add, remove, get)
- Entity filtering and querying
- Support for multiple worlds (e.g., client/server, different game states)

```csharp
// Create a new entity
World world = World.Default;
int entityId = world.CreateEntity();

// Add a component to an entity
world.AddComponent(entityId, new PositionComponent { X = 10, Y = 20 });

// Get a component from an entity
var position = world.GetComponent<PositionComponent>(entityId);

// Query entities with specific components
var entitiesWithPosition = world.GetEntitiesWith<PositionComponent>();
```

### EntityManager

The `EntityManager` is responsible for creating, destroying, and managing entities. It provides methods to create and destroy entities and track which entities are alive.

Key features:
- Entity ID generation
- Entity lifecycle management
- Entity existence tracking

```csharp
// Create a new entity
int entityId = entityManager.CreateEntity();

// Check if an entity exists
bool isAlive = entityManager.IsAlive(entityId);

// Destroy an entity
entityManager.DestroyEntity(entityId);
```

### ComponentStorage

`ComponentStorage<T>` is responsible for storing and managing components of a specific type. It provides methods to add, remove, and retrieve components from entities.

Key features:
- Efficient component storage using a sparse set data structure
- O(1) lookup, insertion, and removal operations
- Reference access to components for direct modification

```csharp
// Add a component to an entity
componentStorage.Add(entityId, new HealthComponent { Value = 100 });

// Get a component
var health = componentStorage.Get(entityId);

// Get a reference to a component for direct modification
ref var healthRef = ref componentStorage.GetRef(entityId);
healthRef.Value -= 10;
```

### ComponentStorageContainer

The `ComponentStorageContainer` manages all component storages in the world. It provides methods to add, get, and check for component storages.

Key features:
- Type-safe component storage management
- Dynamic creation of component storages
- Access to all component storages

```csharp
// Add a component storage
componentStorageContainer.AddStorage<PositionComponent>(new ComponentStorage<PositionComponent>());

// Get a component storage
var positionStorage = componentStorageContainer.GetStorage<PositionComponent>();

// Check if a component storage exists
bool hasHealthStorage = componentStorageContainer.HasStorage<HealthComponent>();
```

### EntityFilter

`EntityFilter` class is responsible for filtering entities based on their components. It provides methods to create and manage filters, allowing you to query entities based on their components.

Key features:
- Component-based entity filtering
- Support for "with" and "without" conditions
- Fluent API for building complex filters

```csharp
// Create a filter for entities with Position and Velocity components
var filter = new EntityFilter(world)
    .With<PositionComponent>()
    .With<VelocityComponent>();

// Get entities that match the filter
foreach (var entity in filter)
{
    // Process entities with both Position and Velocity components
}

// Create a filter for entities with Position but without Health
var filter2 = new EntityFilter(world)
    .With<PositionComponent>()
    .Without<HealthComponent>();
```

### SparseSet

The `SparseSet<T>` is a specialized data structure that efficiently stores entity-value pairs where entities are non-negative integers. It provides O(1) lookup, insertion, and removal operations.

Key features:
- O(1) operations for all basic operations
- Memory-efficient storage
- Support for reference access to values

```csharp
// Add an entity-value pair
sparseSet.Add(entityId, value);

// Check if an entity exists
bool exists = sparseSet.Contains(entityId);

// Get a value
var value = sparseSet[entityId];

// Get a reference to a value for direct modification
ref var valueRef = ref sparseSet.GetRef(entityId);
valueRef.SomeProperty = newValue;
```

## Best Practices

1. **Use structs for components**: Components should be structs to maximize performance and minimize memory allocation.

2. **Keep components data-only**: Components should only contain data, not behavior. Behavior should be implemented in systems.

3. **Use ref returns for component access**: When modifying components, use the ref return methods to avoid unnecessary copying.

4. **Batch operations**: When possible, batch operations on entities to minimize overhead.

5. **Use filters for queries**: Use the Filter class to query entities based on their components rather than iterating through all entities.

6. **Reuse filters**: Create filters once and reuse them rather than creating new filters for each query.

## Example Usage

```csharp
// Create a world
var world = World.Default;

// Create an entity
int entityId = world.CreateEntity();

// Add components to the entity
world.AddComponent(entityId, new PositionComponent { X = 10, Y = 20 });
world.AddComponent(entityId, new VelocityComponent { X = 5, Y = 0 });
world.AddComponent(entityId, new HealthComponent { Value = 100 });

// Create a filter for entities with Position and Velocity components
var filter = new Filter(world)
    .With<PositionComponent>()
    .With<VelocityComponent>();

// Process entities with Position and Velocity components
foreach (var entity in filter)
{
    // Get references to components for direct modification
    ref var position = ref world.GetComponentRef<PositionComponent>(entity);
    ref var velocity = ref world.GetComponentRef<VelocityComponent>(entity);
    
    // Update position based on velocity
    position.X += velocity.X;
    position.Y += velocity.Y;
}
```