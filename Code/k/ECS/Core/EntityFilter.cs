using System;
using System.Collections;
using System.Collections.Generic;

namespace Sandbox.k.ECS.Core;

public class EntityFilter : IEnumerable<int>
{
	private World _world;
	private List<Type> _with = new(); 
	private List<Type> _without = new(); 
	private List<int> _entities = new();
	
	public EntityFilter( World world )
	{
		_world = world;
	}

	public EntityFilter With<T>() where T : struct
	{
		_with.Add(typeof(T));
		return this;
	}

	public EntityFilter Without<T>() where T : struct
	{
		_without.Add(typeof(T));
		return this;
	}

	public IEnumerable<int> GetEntities()
	{
		var allEntities = _world.EntityManager.Entities;
		_entities.Clear();
		foreach ( var entity in allEntities )
		{
			if ( _world.EntityManager.IsAlive(entity) )
			{
				bool hasAll = true;
				foreach ( var type in _with )
				{
					if ( !_world.HasComponent(type, entity) )
					{
						hasAll = false;
						break;
					}
				}

				if ( !hasAll ) continue;

				foreach ( var type in _without )
				{
					if ( _world.HasComponent(type, entity) )
					{
						hasAll = false;
						break;
					}
				}

				if ( hasAll )
					_entities.Add(entity);
			}
		}

		return _entities;
	}

	public void Clear()
	{
		_with.Clear();
		_without.Clear();
	}

	public IEnumerator<int> GetEnumerator()
	{
		foreach ( var entity in GetEntities() )
		{
			yield return entity;
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
