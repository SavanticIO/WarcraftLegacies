﻿using MacroTools.LegendSystem;
#pragma warning disable CS1591

namespace WarcraftLegacies.Source.Setup.Legends
{
  public sealed class LegendNazjatar
  {
    public LegendaryHero Azshara { get; }
    public LegendaryHero Sivara { get; }
    public LegendaryHero Zakajz { get; }
    public LegendaryHero Nazjar { get; }

    public LegendNazjatar()
    {
      Azshara = new LegendaryHero("Azshara")
      {
        UnitType = Constants.UNIT_H08U_EMPRESS_OF_NAZJATAR_NZOTH,
        StartingXp = 10000,
      };

      Sivara = new LegendaryHero("Sivara")
      {
        UnitType = Constants.UNIT_U02U_ABYSSAL_COMMANDER_NAZJATAR,
        StartingXp = 7000,
      };

      Zakajz = new LegendaryHero("Zakajz")
      {
        UnitType = Constants.UNIT_U00P_C_THRAX_ABERRATION,
        StartingXp = 7000,
      };

      Nazjar = new LegendaryHero("Naz'jar")
      {
        UnitType = Constants.UNIT_H0A5_SEA_WITCH_NZOTH,
        StartingXp = 7000,
      };
    }
    public void RegisterLegends()
    {
      LegendaryHeroManager.Register(Azshara);
      LegendaryHeroManager.Register(Sivara);
      LegendaryHeroManager.Register(Zakajz);
      LegendaryHeroManager.Register(Nazjar);
    }
  }
}