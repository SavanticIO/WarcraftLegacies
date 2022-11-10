using System.Collections.Generic;
using MacroTools.Extensions;
using MacroTools.FactionSystem;
using MacroTools.QuestSystem;
using MacroTools.QuestSystem.UtilityStructs;
using MacroTools.Wrappers;
using WarcraftLegacies.Source.Setup;
using WarcraftLegacies.Source.Setup.Legends;
using WCSharp.Shared.Data;
using static War3Api.Common;

namespace WarcraftLegacies.Source.Quests.Druids
{
  public sealed class QuestMalfurionAwakens : QuestData
  {
    private readonly List<unit> _moongladeUnits = new();

    public QuestMalfurionAwakens(Rectangle moonglade) : base("Awakening of Stormrage",
      "Ever since the War of the Ancients ten thousand years ago, Malfurion Stormrage and his druids have slumbered within the Barrow Den. Now, their help is required once again.",
      "ReplaceableTextures\\CommandButtons\\BTNFurion.blp")
    {
      AddObjective(new ObjectiveAcquireArtifact(ArtifactSetup.ArtifactHornofcenarius));
      AddObjective(new ObjectiveArtifactInRect(ArtifactSetup.ArtifactHornofcenarius, Regions.Moonglade,
        "The Barrow Den"));
      AddObjective(new ObjectiveExpire(1440));
      AddObjective(new ObjectiveSelfExists());
      foreach (var unit in new GroupWrapper().EnumUnitsInRect(moonglade).EmptyToList())
      {
        SetUnitInvulnerable(unit, true);
        _moongladeUnits.Add(unit);
      }
    }

    protected override string CompletionPopup => "Malfurion has emerged from his deep slumber in the Barrow Den.";

    protected override string RewardDescription => "Gain the hero Malfurion and the artifact G'hanir";

    protected override void OnFail(Faction completingFaction)
    {
      foreach (var unit in _moongladeUnits) unit.Rescue(Player(PLAYER_NEUTRAL_AGGRESSIVE));
    }

    protected override void OnComplete(Faction completingFaction)
    {
      foreach (var unit in _moongladeUnits) unit.Rescue(completingFaction.Player);
      if (LegendDruids.LegendMalfurion.Unit == null)
      {
        LegendDruids.LegendMalfurion.ForceCreate(completingFaction.Player, Regions.Moonglade.Center,
          270);
        SetHeroLevel(LegendDruids.LegendMalfurion.Unit, 3, false);
        LegendDruids.LegendMalfurion.Unit.AddItemSafe(ArtifactSetup.ArtifactGhanir.Item);
      }
      else
      {
        SetItemPosition(ArtifactSetup.ArtifactGhanir.Item, GetUnitX(GetTriggerUnit()), GetUnitY(GetTriggerUnit()));
      }
    }
  }
}