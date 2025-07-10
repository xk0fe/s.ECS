using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sandbox.k.ECS.Core.Common;

/// <summary>
/// A sparse set data structure that efficiently stores entity-value pairs where entities are non-negative integers.
/// Provides O(1) lookup, insertion, and removal operations.
/// </summary>
/// <typeparam name="T">The type of values associated with entities</typeparam>
public class SparseSet<T> : IEnumerable<int>
{
    // The sparse array maps entity IDs to indices in the dense array
    private List<int> _sparse = new List<int>();
    
    // The dense array contains the actual entity IDs in the order they were added
    private List<int> _dense = new List<int>();
    
    // Stores values associated with entities
    private List<T> _values = new List<T>();

    /// <summary>
    /// Gets the number of entities in the set
    /// </summary>
    public int Count => _dense.Count;

    /// <summary>
    /// Adds an entity to the sparse set without an associated value
    /// </summary>
    /// <param name="entity">The entity ID to add</param>
    public void Add(int entity)
    {
        // Use default value for the type
        Add(entity, default);
    }

    /// <summary>
    /// Adds an entity to the sparse set with an associated value
    /// </summary>
    /// <param name="entity">The entity ID to add</param>
    /// <param name="value">The value to associate with the entity</param>
    public void Add(int entity, T value)
    {
        if (entity < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(entity), "Entity ID must be non-negative");
        }

        // Ensure the sparse array is large enough
        EnsureSparseCapacity(entity);

        // If entity already exists, update its value
        if (Contains(entity))
        {
            int index = _sparse[entity];
            _values[index] = value;
            return;
        }

        // Add entity to dense array and store its index in sparse array
        _sparse[entity] = _dense.Count;
        _dense.Add(entity);
        _values.Add(value);
    }

    /// <summary>
    /// Removes an entity from the sparse set
    /// </summary>
    /// <param name="entity">The entity ID to remove</param>
    public void Remove(int entity)
    {
        if (entity < 0 || entity >= _sparse.Count || !Contains(entity))
        {
            return;
        }

        // Get index of entity in dense array
        int denseIndex = _sparse[entity];
        
        // Get the last entity in dense array
        int lastEntity = _dense[_dense.Count - 1];
        
        // Move the last entity to the position of the removed entity
        _dense[denseIndex] = lastEntity;
        _values[denseIndex] = _values[_values.Count - 1];
        
        // Update sparse array
        _sparse[lastEntity] = denseIndex;
        
        // Remove the last element from dense array
        _dense.RemoveAt(_dense.Count - 1);
        _values.RemoveAt(_values.Count - 1);
    }

    /// <summary>
    /// Checks if an entity exists in the sparse set
    /// </summary>
    /// <param name="entity">The entity ID to check</param>
    /// <returns>True if the entity exists, false otherwise</returns>
    public bool Contains(int entity)
    {
	    return entity >= 0 && 
	           entity < _sparse.Count && 
	           _sparse[entity] >= 0 &&  // Check that we have a valid index (not -1)
	           _sparse[entity] < _dense.Count && 
	           _dense[_sparse[entity]] == entity;
    }

    /// <summary>
    /// Clears all entities from the sparse set
    /// </summary>
    public void Clear()
    {
        _sparse.Clear();
        _dense.Clear();
        _values.Clear();
    }

    /// <summary>
    /// Tries to get the value associated with an entity
    /// </summary>
    /// <param name="entity">The entity ID</param>
    /// <param name="value">The associated value if found, default otherwise</param>
    /// <returns>True if the entity exists, false otherwise</returns>
    public bool TryGetValue(int entity, out T value)
    {
        if (Contains(entity))
        {
            value = _values[_sparse[entity]];
            return true;
        }

        value = default;
        return false;
    }

    /// <summary>
    /// Gets or sets the value associated with an entity
    /// </summary>
    /// <param name="entity">The entity ID</param>
    /// <returns>The value associated with the entity</returns>
    public T this[int entity]
    {
        get
        {
            if (!Contains(entity))
            {
                throw new KeyNotFoundException($"Entity {entity} does not exist in the sparse set");
            }
            return _values[_sparse[entity]];
        }
        set
        {
            Add(entity, value);
        }
    }

    /// <summary>
    /// Gets all entity-value pairs in the sparse set
    /// </summary>
    /// <returns>An enumerable of KeyValuePair containing entities and their associated values</returns>
    public IEnumerable<KeyValuePair<int, T>> GetAll()
    {
        for (int i = 0; i < _dense.Count; i++)
        {
            yield return new KeyValuePair<int, T>(_dense[i], _values[i]);
        }
    }

    /// <summary>
    /// Gets an enumerator that iterates through the entities in the sparse set
    /// </summary>
    public IEnumerator<int> GetEnumerator()
    {
        return _dense.GetEnumerator();
    }

    /// <summary>
    /// Gets a non-generic enumerator that iterates through the entities in the sparse set
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Ensures the sparse array has enough capacity to store an entity with the given ID
    /// </summary>
    /// <param name="entity">The entity ID</param>
    private void EnsureSparseCapacity(int entity)
    {
        if (entity >= _sparse.Count)
        {
            // Calculate new capacity (at least entity + 1, but potentially more for efficiency)
            int newCapacity = Math.Max(entity + 1, _sparse.Count * 2);
            
            // Add placeholders to reach the new capacity
            var oldCount = _sparse.Count;
            for (int i = 0; i < newCapacity - oldCount; i++)
            {
                _sparse.Add(-1); // -1 indicates no mapping
            }
        }
    }
    
    public ref T GetRef(int entity)
    {
	    if (!Contains(entity))
		    throw new KeyNotFoundException($"Entity {entity} does not exist");
    
	    int index = _sparse[entity];
	    return ref CollectionsMarshal.AsSpan(_values)[index];
    }
}
