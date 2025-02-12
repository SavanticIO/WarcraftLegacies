﻿using MacroTools.FactionSystem;
using WarcraftLegacies.Shared.FactionObjectLimits;
using WarcraftLegacies.Source.Setup;

namespace WarcraftLegacies.Source.Factions
{
  public sealed class Nazjatar : Faction
  {
    /// <inheritdoc />
    public Nazjatar() : base("Nazjatar", PLAYER_COLOR_PURPLE, "|c00540081",
      @"ReplaceableTextures\CommandButtons\BTNNagaSummoner.blp")
    {
      ControlPointDefenderUnitTypeId = Constants.UNIT_U02T_CONTROL_POINT_DEFENDER_NAZJATAR;
    }

    /// <inheritdoc />
    public override void OnRegistered()
    {
      RegisterObjectLimits();
      SharedFactionConfigSetup.AddSharedFactionConfig(this);
    }

    private void RegisterObjectLimits()
    {
      foreach (var (objectTypeId, objectLimit) in NazjatarObjectLimitData.GetAllObjectLimits())
        ModObjectLimit(FourCC(objectTypeId), objectLimit.Limit);
    }
  }
}