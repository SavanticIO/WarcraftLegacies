﻿using MacroTools.LegendSystem;
using static War3Api.Common;
#pragma warning disable CS1591

namespace WarcraftLegacies.Source.Setup.Legends
{
  public sealed class LegendCthun
  {
    public LegendaryHero Cthun { get; }
    public LegendaryHero Ouro { get; }
    public LegendaryHero Skeram { get; }
    public LegendaryHero Moam { get; }

    public LegendCthun()
    {
      Cthun = new LegendaryHero("C'thun")
      {
        UnitType = Constants.UNIT_U00R_OLD_GOD,
        PermaDies = true,
        StartingXp = 10000,
      };

      Moam = new LegendaryHero("Moam")
      {
        UnitType = Constants.UNIT_U00Z_OBSIDIAN_DESTROYER,
        StartingXp = 7000,
      };

      Skeram = new LegendaryHero("Prophet Skeram")
      {
        UnitType = Constants.UNIT_E005_THE_PROPHET,
        StartingXp = 7000,
        PlayerColor = PLAYER_COLOR_RED
      };

      Ouro = new LegendaryHero("Ouro")
      {
        UnitType = Constants.UNIT_U02S_ANCIENT_SAND_WORM,
        StartingXp = 7000,
      };
    }
    public void RegisterLegends()
    {
      LegendaryHeroManager.Register(Cthun);
      LegendaryHeroManager.Register(Moam);
      LegendaryHeroManager.Register(Ouro);
      LegendaryHeroManager.Register(Skeram);
    }
  }
}