﻿using MacroTools.Extensions;
using MacroTools.FactionSystem;
using MacroTools.LegendSystem;
using MacroTools.ObjectiveSystem.Objectives.MetaBased;
using MacroTools.ObjectiveSystem.Objectives.TimeBased;
using MacroTools.ObjectiveSystem.Objectives.UnitBased;
using MacroTools.QuestSystem;
using System;
using WarcraftLegacies.Source.Setup.FactionSetup;
using WCSharp.Shared.Data;
using static War3Api.Common;

namespace WarcraftLegacies.Source.Quests.Warsong
{
  public sealed class QuestBloodpact : QuestData
  {
    private readonly LegendaryHero _mannoroth;
    private readonly LegendaryHero _grom;

    /// <summary>
    /// Initializes a new instance of the class <see cref="QuestBloodpact"/>.
    /// </summary>
    public QuestBloodpact(LegendaryHero Mannoroth, LegendaryHero grom)
      : base("The Bloodpact",
        "The Warsong is still vulnerable to the tentation of Mannoroth's Blood. If they drink it from the Fountain, they would have a surge of power. Although, Thrall would certainly hurry to save his friend Grom from the corruption.",
        "ReplaceableTextures\\CommandButtons\\BTNBloodFury.blp")
    {
      AddObjective(new ObjectiveResearch(Constants.UPGRADE_R09O_DRINK_THE_BLOOD_OF_MANNOROTH, Constants.UNIT_NBFL_FOUNTAIN_OF_BLOOD_BLOODPACT));
      Global = true;
      _mannoroth = Mannoroth;
      _grom = grom;
    }
    //private bool IsPointValidForGrom(Point whichPoint) =>
    //  !whichPoint.IsPathable(PATHING_TYPE_FLOATABILITY) && whichPoint.IsPathable(PATHING_TYPE_WALKABILITY);

    /// <inheritdoc/>
    protected override string RewardFlavour => "The Warsong has drunk the blood of Mannoroth. It will take Thrall 4 minutes to save Grom and purify the Warsong Clan orcs.";

    /// <inheritdoc />
    protected override string RewardDescription =>
      "You will gain Mannoroth as a temporary unit, all your orcs except your Kor'kron Elites will gain 200 hit points and chaos damage. After 4 min, your units will revert to normal and Mannoroth will become hostile.";

    /// <inheritdoc />
    protected override void OnComplete(Faction completingFaction)
    {
      var timerBloodpact = CreateTimer();
      _mannoroth.ForceCreate(completingFaction.Player, Regions.FountainUnlock.Center, 270);
      _grom.UnitType = Constants.UNIT_OPGH_CORRUPTOR_OF_THE_WARSONG_CLAN_WARSONG_BLOODPACT;
      TimerStart(timerBloodpact, 180, false, () =>
      {
        completingFaction.SetObjectLimit(Constants.UPGRADE_R09O_DRINK_THE_BLOOD_OF_MANNOROTH, -1);
        completingFaction.SetObjectLevel(Constants.UPGRADE_R09O_DRINK_THE_BLOOD_OF_MANNOROTH, -1);
        completingFaction.SetObjectLevel(Constants.UPGRADE_R09P_REVERT_BLOODPACT, 1);

        _mannoroth.ForceCreate(Player(PLAYER_NEUTRAL_AGGRESSIVE), Regions.FountainUnlock.Center, 270);
        _grom.UnitType = Constants.UNIT_OGRH_CHIEFTAIN_OF_THE_WARSONG_CLAN_WARSONG;

      });
      // Grom Auto Revive towards end of bloodpact Uncomment if perma death becomes a thing again -Madsen
      //var TimeRessGrom = CreateTimer();
      //TimerStart(TimeRessGrom, 175, false, () =>
      //{
      //  Point randomgrompoint;
      //  do
      //  {
      //    randomgrompoint = Regions.GromRandomPoint.GetRandomPoint();
      //  } while (!IsPointValidForGrom(randomgrompoint));

      //  ReviveHero(_grom.Unit, randomgrompoint.X, randomgrompoint.Y, true);
    }
  }
}