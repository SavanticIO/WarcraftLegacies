﻿using System.Collections.Generic;
using MacroTools.Extensions;
using MacroTools.FactionSystem;
using MacroTools.LegendSystem;
using MacroTools.ObjectiveSystem.Objectives.FactionBased;
using MacroTools.ObjectiveSystem.Objectives.LegendBased;
using MacroTools.ObjectiveSystem.Objectives.QuestBased;
using MacroTools.ObjectiveSystem.Objectives.TimeBased;
using MacroTools.Powers;
using MacroTools.QuestSystem;
using WarcraftLegacies.Source.Setup.Legends;
using WCSharp.Shared.Data;
using static War3Api.Common;

namespace WarcraftLegacies.Source.Quests.Lordaeron
{
  /// <summary>
  /// Capture <see cref="LegendNeutral.Caerdarrow"/> to unlock the capital city of Lordaeron.
  /// </summary>
  public sealed class QuestCapitalCity : QuestData
  {
    private readonly List<unit> _rescueUnits;
    private readonly unit _unitToMakeInvulnerable;
    private readonly LegendaryHero _uther;
    private readonly Capital _capitalPalace;
    private const string RewardPowerName = "Dominion";

    /// <summary>
    /// Initializes a new instance of the <see cref="QuestCapitalCity"/> class.
    /// </summary>
    public QuestCapitalCity(Rectangle rescueRect, unit unitToMakeInvulnerable, LegendaryHero uther, LegendaryHero arthas, Capital caerDarrow, Capital capitalPalace, IEnumerable<QuestData> prequisites) :
      base("Hearthlands",
        "The territories of Lordaeron are fragmented. Regain control of the old Alliance's hold to secure the kingdom.",
        "ReplaceableTextures\\CommandButtons\\BTNCastle.blp")
    {
      AddObjective(new ObjectiveControlCapital(caerDarrow, false));
      foreach (var prequisite in prequisites)
        AddObjective(new ObjectiveCompleteQuest(prequisite));
      AddObjective(new ObjectiveControlLegend(arthas, false));
      AddObjective(new ObjectiveExpire(1472, Title));
      AddObjective(new ObjectiveSelfExists());
      ResearchId = Constants.UPGRADE_R04Y_QUEST_COMPLETED_HEARTHLANDS;
      _unitToMakeInvulnerable = unitToMakeInvulnerable;
      _uther = uther;
      _capitalPalace = capitalPalace;
      bool RescueUnitFilter(unit whichUnit) => GetUnitTypeId(whichUnit) != Constants.UNIT_N08F_UNDERCITY_ENTRANCE;
      _rescueUnits = rescueRect.PrepareUnitsForRescue(RescuePreparationMode.HideNonStructures, RescueUnitFilter);
      Required = true;
    }

    /// <inheritdoc/>
    protected override string RewardFlavour =>
      "The Capital City of Lordaeron has joined Arthas.";

    /// <inheritdoc/>
    protected override string RewardDescription => $"Gain control of all units in the Capital City, Gain Uther, and acquire the {RewardPowerName} Power";

    /// <inheritdoc/>
    protected override void OnFail(Faction completingFaction)
    {
      Player(PLAYER_NEUTRAL_AGGRESSIVE).RescueGroup(_rescueUnits);
    }

    /// <inheritdoc/>
    protected override void OnComplete(Faction completingFaction)
    {
      var rewardPower = new ControlLevelPerTurnBonus(0.5f)
      {
        IconName = @"ShieldOfUnification",
        Name = RewardPowerName
      };
      
      completingFaction.AddPower(rewardPower);
      completingFaction.Player?.DisplayPowerAcquired(rewardPower);

      if (_uther.Unit == null)
      {
        _uther.ForceCreate(completingFaction.Player, Regions.King_Arthas_crown.Center,
          90);
        _uther.Unit?
          .SetLevel(5, false);
      }

        completingFaction.Player?.RescueGroup(_rescueUnits);
      SetUnitInvulnerable(_unitToMakeInvulnerable, true);
      if (GetLocalPlayer() == completingFaction.Player)
        PlayThematicMusic("war3mapImported\\CapitalCity.mp3");
      _uther.AddUnitDependency(_capitalPalace.Unit);
    }
  }
}