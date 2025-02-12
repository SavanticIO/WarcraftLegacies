﻿using MacroTools.PassiveAbilities;
using MacroTools.PassiveAbilitySystem;
using MacroTools.SpellSystem;
using WarcraftLegacies.Source.Spells;

namespace WarcraftLegacies.Source.Setup.Spells
{
  public static class IronforgeSpellSetup
  {
    /// <summary>
    /// Sets up <see cref="IronforgeSpellSetup"/>.
    /// </summary>
    public static void Setup()
    {
      SpellSystem.Register(new TitanForgeArtifact(ABILITY_A0T3_TITANFORGE_ARTIFACT_IRONFORGE, 0));

      var lightningAttack = new SpellOnAttack(UNIT_H03Z_STORMRIDER_IRONFORGE,
        ABILITY_A10J_MASTER_OF_LIGHTNING_STORMRIDERS)
      {
        DummyAbilityId = ABILITY_ACFL_FORKED_LIGHTNING_LIGHT_BLUE_HIGHBORNE,
        DummyOrderId = OrderId("forkedlightning"),
        ProcChance = 0.15f
      };
      PassiveAbilityManager.Register(lightningAttack);
    }
  }
}