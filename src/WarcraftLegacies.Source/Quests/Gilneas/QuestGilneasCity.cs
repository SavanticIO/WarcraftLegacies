﻿using MacroTools.FactionSystem;
using MacroTools.QuestSystem;
using System.Collections.Generic;
using static War3Api.Common;
using MacroTools.Extensions;
using MacroTools.ObjectiveSystem.Objectives.FactionBased;
using MacroTools.ObjectiveSystem.Objectives.TimeBased;
using MacroTools.ObjectiveSystem.Objectives.UnitBased;
using WarcraftLegacies.Source.Rocks;
using MacroTools;

namespace WarcraftLegacies.Source.Quests.Gilneas
{
  /// <summary>
  /// Kill Worgen in and around Gilneas and upgrade the main building to Tier 3 in order to unlock Gilneas City and the Greymane Wall
  /// </summary>
  public sealed class QuestGilneasCity : QuestData
  {
    private readonly List<unit> _rescueUnits;
    private readonly unit _gilneasDoor;

    /// <summary>
    /// Initializes a new instance of the <see cref="QuestGilneasCity"/> class.
    /// </summary>
    public QuestGilneasCity(PreplacedUnitSystem preplacedUnitSystem) : base("Liberation of Gilneas", "Gilneas has been under the curse of the Worgen. Eliminate all of them to free Gilneas of the curse.", "ReplaceableTextures\\CommandButtons\\BTNGilneasCathedral.blp")
    {
      _gilneasDoor = preplacedUnitSystem.GetUnit(Constants.UNIT_H02K_GREYMANE_S_GATE_CLOSED);
      AddObjective(new ObjectiveKillXUnit(Constants.UNIT_O02J_WORGEN_GILNEAS, 8));
      AddObjective(new ObjectiveKillXUnit(Constants.UNIT_O038_WORGEN_BLOOD_SHAMAN_WORGEN_HERO, 3));
      AddObjective(new ObjectiveUpgrade(Constants.UNIT_H02C_CASTLE_GILNEAS_T3, Constants.UNIT_H01R_TOWN_HALL_GILNEAS_T1));
      AddObjective(new ObjectiveExpire(1300, "Liberation of Gilneas"));
      AddObjective(new ObjectiveSelfExists());

      _rescueUnits = Regions.GilneasUnlock5.PrepareUnitsForRescue(RescuePreparationMode.HideNonStructures, RescuableFilter);
      _rescueUnits.AddRange(Regions.GilneasUnlock6.PrepareUnitsForRescue(RescuePreparationMode.HideNonStructures, RescuableFilter));
      ResearchId = Constants.UPGRADE_R02R_QUEST_COMPLETED_LIBERATION_OF_GILNEAS;
      Required = true;
    }

    /// <inheritdoc/>
    protected override string RewardFlavour => "Every worgen has been eliminated, the curse is lifting!";

    /// <inheritdoc/>
    protected override string RewardDescription => "Gain control of the Greymane Wall and Gilneas City. Enable to train Genn Greymane and the Worgen units";

    /// <inheritdoc/>
    protected override void OnComplete(Faction whichFaction)
    {
      if (whichFaction.Player == null) 
        return;
      _gilneasDoor
        .SetInvulnerable(false);
      SetUnitOwner(_gilneasDoor, whichFaction.Player, true);

      whichFaction.Player.RescueGroup(_rescueUnits);
      RockSystem.Register(new RockGroup(Regions.GilneasUnlock5, FourCC("LTrc"), 1));
      if (GetLocalPlayer() == whichFaction.Player)
        PlayThematicMusic("war3mapImported\\GilneasTheme1.mp3");
    }

    /// <inheritdoc/>
    protected override void OnFail(Faction whichFaction) => 
      Player(PLAYER_NEUTRAL_AGGRESSIVE).RescueGroup(_rescueUnits);
    
    private static bool RescuableFilter(unit filterUnit) => filterUnit.GetTypeId() != Constants.UNIT_O05Q_GREYMANETOWER_GILNEAS_REAL_TOWER;
  }
}
