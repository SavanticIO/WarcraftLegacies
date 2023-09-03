﻿using System.Collections.Generic;
using MacroTools.Extensions;
using MacroTools.FactionSystem;
using MacroTools.ObjectiveSystem.Objectives.FactionBased;
using MacroTools.ObjectiveSystem.Objectives.TimeBased;
using MacroTools.ObjectiveSystem.Objectives.UnitBased;
using MacroTools.QuestSystem;
using WCSharp.Shared.Data;
using static War3Api.Common;

namespace WarcraftLegacies.Source.Quests.Sentinels
{
  /// <summary>
  /// Unlocks Feathermoon Stronghold upon securing the area and bringing a hero to it.
  /// </summary>
  public sealed class QuestFeathermoon : QuestData
  {
    private readonly List<unit> _rescueUnits;

    /// <summary>
    /// Initializes a new instance of <see cref="QuestFeathermoon"/>.
    /// </summary>
    public QuestFeathermoon(Rectangle rescueRect) : base("Shores of Feathermoon",
      "Feathermoon Stronghold stood guard for ten thousand years, it is time to relieve the guards from their duty.",
      @"ReplaceableTextures\CommandButtons\BTNBearDen.blp")
    {
      AddObjective(new ObjectiveAnyUnitInRect(rescueRect, "Feathermoon Stronghold", true));
      AddObjective(new ObjectiveKillAllInArea(new List<Rectangle> { Regions.FeathermoonCreeps }, "around Feathermoon Stronghold"));
      AddObjective(new ObjectiveExpire(1485, Title));
      AddObjective(new ObjectiveSelfExists());
      ResearchId = Constants.UPGRADE_R06M_QUEST_COMPLETED_FEATHERMOON_STRONGHOLD;
      _rescueUnits = rescueRect.PrepareUnitsForRescue(RescuePreparationMode.Invulnerable);
      Required = true;
    }

    /// <inheritdoc />
    protected override string RewardFlavour =>
      "Feathermoon Stronghold has been relieved and has joined the Sentinels in their war effort";

    /// <inheritdoc />
    protected override string RewardDescription =>
      "Control of all units in Feathermoon Stronghold";

    /// <inheritdoc />
    protected override void OnFail(Faction completingFaction)
    {
      Player(PLAYER_NEUTRAL_AGGRESSIVE).RescueGroup(_rescueUnits);
    }

    /// <inheritdoc />
    protected override void OnComplete(Faction completingFaction)
    {
      completingFaction.Player?.RescueGroup(_rescueUnits);
      if (GetLocalPlayer() == completingFaction.Player) 
        PlayThematicMusic("war3mapImported\\SentinelTheme.mp3");
    }
  }
}