﻿using MacroTools.ArtifactSystem;
using MacroTools.Extensions;
using MacroTools.FactionSystem;
using MacroTools.LegendSystem;
using MacroTools.ObjectiveSystem.Objectives.LegendBased;
using MacroTools.ObjectiveSystem.Objectives.TimeBased;
using MacroTools.QuestSystem;
using static War3Api.Common;


namespace WarcraftLegacies.Source.Quests.Dalaran
{
  public sealed class QuestAegwynn : QuestData
  {

    public QuestAegwynn(LegendaryHero jaina, LegendaryHero antonidas) : base("Return from Exile",
      "A new generation of Mages are in dire need of council. The exiled Aegwynn, used to be a Guardian of Tirisfal. Grabbing her attention will require powerful wizards.",
      "ReplaceableTextures\\CommandButtons\\BTN.MagnaArchmage.blp")
    {
      AddObjective(new ObjectiveLegendLevel(antonidas, 7));
      AddObjective(new ObjectiveLegendLevel(jaina, 7));
      ResearchId = Constants.UPGRADE_R09F_QUEST_COMPLETED_RETURN_FROM_EXILE;
    }

    /// <inheritdoc/>
    protected override string RewardFlavour =>
      "Aegwynn will come back from exile to mentor Jaina.";

    /// <inheritdoc/>
    protected override string RewardDescription =>
      "Aegwynn will also be trainable at the altar.";
  }
}