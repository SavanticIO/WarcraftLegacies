﻿using WCSharp.Shared.Data;
using static War3Api.Common;
using static MacroTools.Libraries.GeneralHelpers;

namespace MacroTools.Extensions
{
  /// <summary>
  /// A useful set of extension methods for <see cref="Point"/>s.
  /// </summary>
  public static class PointExtensions
  {
    /// <summary>
    /// Returns true if units can path over the <see cref="Point"/>.
    /// </summary>
    public static bool IsPathable(this Point whichPoint, pathingtype pathingType) =>
      !IsTerrainPathable(whichPoint.X, whichPoint.Y, pathingType);

    /// <summary>
    /// Returns the terrain type at the <see cref="Point"/>.
    /// </summary>
    public static int TerrainType(this Point whichPoint) => GetTerrainType(whichPoint.X, whichPoint.Y);
    
    /// <summary>
    /// Removes trees in a radius around a unit
    /// </summary>
    public static void RemoveDestructablesInRadius(this Point whichPoint, float radius)
    {
      EnumDestructablesInCircle(radius, whichPoint,
        () =>
        {
          RemoveDestructable(GetEnumDestructable());
        });
    }
  }
}