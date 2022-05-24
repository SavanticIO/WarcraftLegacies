using System.Collections.Generic;
using AzerothWarsCSharp.MacroTools;
using AzerothWarsCSharp.MacroTools.QuestSystem;
using AzerothWarsCSharp.Source.Quests.Fel_Horde;
using WCSharp.Shared.Data;
using static AzerothWarsCSharp.Source.Setup.FactionSetup.FelHordeSetup;
using static War3Api.Common;

namespace AzerothWarsCSharp.Source.Setup.QuestSetup
{
  public static class FelHordeQuestSetup
  {
    public static void Setup()
    {
      QuestData newQuest = FactionFelHorde.AddQuest(new QuestKillDraenei());
      FactionFelHorde.StartingQuest = newQuest;
      FactionFelHorde.AddQuest(new QuestKilsorrow(Regions.KilsorrowUnlock,
        PreplacedUnitSystem.GetUnit(Constants.UNIT_O017_KIL_SORROW_FORTRESS)));
      FactionFelHorde.AddQuest(new QuestHellfireCitadel(Regions.HellfireUnlock, new List<unit>
      {
        PreplacedUnitSystem.GetUnit(Constants.UNIT_N081_UNFOCUSED_DEMON_GATE_T0, Regions.DemonGate3.Center),
        PreplacedUnitSystem.GetUnit(Constants.UNIT_N081_UNFOCUSED_DEMON_GATE_T0, Regions.Demongate_1.Center)
      }));
      FactionFelHorde.AddQuest(new QuestBlackrock(Regions.BlackrockUnlock));
      FactionFelHorde.AddQuest(new QuestFelHordeKillIronforge());
      FactionFelHorde.AddQuest(new QuestFelHordeKillStormwind());
      FactionFelHorde.AddQuest(new QuestGuldansLegacy());
    }
  }
}