﻿using MacroTools.Extensions;
using MacroTools.FactionSystem;
using MacroTools.ObjectiveSystem.Objectives.ControlPointBased;
using MacroTools.ObjectiveSystem.Objectives.FactionBased;
using MacroTools.ObjectiveSystem.Objectives.TimeBased;
using MacroTools.QuestSystem;

namespace WarcraftLegacies.Source.Quests.Stormwind
{
  public sealed class QuestDarkshire : QuestData
  {
    public QuestDarkshire() : base("Gnoll Troubles",
      "The town of Darkshire is under attack by Gnoll's, clear them out!",
      @"ReplaceableTextures\CommandButtons\BTNGnollArcher.blp")
    {
      AddObjective(new ObjectiveControlPoint(UNIT_N00V_DUSKWOOD));
      AddObjective(new ObjectiveExpire(600, Title));
      AddObjective(new ObjectiveSelfExists());
    }

    /// <inheritdoc/>
    public override string RewardFlavour =>
      "Darkshire has been liberated, and its military is now free to assist Stormwind.";

    /// <inheritdoc/>
    protected override string RewardDescription => "Control of all units in Darkshire";

    private static void GrantDarkshire(player whichPlayer)
    {
      var tempGroup = CreateGroup();

      //Transfer all Neutral Passive units in Darkshire
      GroupEnumUnitsInRect(tempGroup, Regions.DarkshireUnlock.Rect, null);
      var u = FirstOfGroup(tempGroup);
      while (true)
      {
        if (u == null) break;
        if (GetOwningPlayer(u) == Player(PLAYER_NEUTRAL_PASSIVE)) u.Rescue(whichPlayer);
        GroupRemoveUnit(tempGroup, u);
        u = FirstOfGroup(tempGroup);
      }

      DestroyGroup(tempGroup);
    }

    /// <inheritdoc/>
    protected override void OnFail(Faction completingFaction)
    {
      var rescuer = completingFaction.ScoreStatus == ScoreStatus.Defeated
        ? Player(PLAYER_NEUTRAL_AGGRESSIVE)
        : completingFaction.Player;

      GrantDarkshire(rescuer);
    }

    /// <inheritdoc/>
    protected override void OnComplete(Faction completingFaction)
    {
      GrantDarkshire(completingFaction.Player);
    }
  }
}