﻿using MacroTools.FactionSystem;
using MacroTools.LegendSystem;
using MacroTools.ObjectiveSystem.Objectives.UnitBased;
using MacroTools.QuestSystem;

namespace WarcraftLegacies.Source.Quests.Sentinels
{
  /// <summary>
  /// Destroy Thunderbluff to unlock a hero and a demi hero.
  /// </summary>
  public sealed class QuestSentinelsKillCthun : QuestData
  {

    /// <summary>
    /// Initializes a new instance of the <see cref="QuestSentinelsKillCthun"/> class.
    /// </summary>
    public QuestSentinelsKillCthun(Legend cthun) : base("Gates of Ahn'Qiraj",
      "The Qiraji have bursted out of their underground city. They need to be ridden from the surface of Kalimdor.",
      @"ReplaceableTextures\CommandButtons\BTNCthunIcon.blp")
    {
      AddObjective(new ObjectiveKillUnit(cthun.Unit));
      ResearchId = UPGRADE_R052_QUEST_COMPLETED_GATES_OF_AHN_QIRAJ_SENTINELS;
      
    }
    /// <inheritdoc/>
    public override string RewardFlavour =>
      "C'thun, the Qiraji god, has been killed. Their hordes have dispersed";

    /// <inheritdoc/>
    protected override string RewardDescription => $"Learn to train {GetObjectName(UNIT_E022_MOON_RIDER_SENTINELS)}s from the {GetObjectName(UNIT_EDOS_ROOST_SENTINEL_SPECIALIST)}s and research {GetObjectName(UPGRADE_REMG_UPGRADE_MOON_GLAIVE_LIGHT_BLUE_RESEARCH)}'s second level from the {GetObjectName(UNIT_E00L_WAR_ACADEMY_SENTINEL_BARRACKS)}";

    /// <inheritdoc/>
    protected override void OnComplete(Faction completingFaction)
    {
      SetPlayerTechResearched(completingFaction.Player, ResearchId, 1);

    }
    /// <inheritdoc/>
    protected override void OnAdd(Faction whichFaction)
    {
      whichFaction.ModObjectLimit(ResearchId, Faction.UNLIMITED);
    }
  }
}