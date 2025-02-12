using MacroTools.Extensions;
using MacroTools.FactionSystem;
using WCSharp.Events;

namespace WarcraftLegacies.Source.Researches.Stormwind
{
  public static class TierCodeOfChivalry
  {
    private static void Research()
    {
      GetTriggerPlayer().GetFaction()?.ModObjectLimit(UNIT_H01B_OUTRIDER_STORMWIND, -Faction.UNLIMITED);
      GetTriggerPlayer().GetFaction()?.ModObjectLimit(UNIT_H054_STORMWIND_KNIGHT_STORMWIND, Faction.UNLIMITED);
    }

    public static void Setup()
    {
      PlayerUnitEvents.Register(ResearchEvent.IsFinished, Research, UPGRADE_R030_CODE_OF_CHIVALRY_ARATHOR_T3);
    }
  }
}