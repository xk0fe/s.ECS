using Sandbox.k.ECS.Core;

namespace Sandbox.k.ECS.Extensions.Utils;

public static class Filter
{
	/// <summary>
	/// Sugar for creating a default filter. So now instead of EntityFilter( World.Default ) you can just call Filter.Default()
	/// </summary>
	/// <returns></returns>
	public static EntityFilter Default()
	{
		return new EntityFilter( World.Default );
	}
}
