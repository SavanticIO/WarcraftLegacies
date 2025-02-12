﻿using System.Collections.Generic;
using MacroTools.PassiveAbilities;
using MacroTools.PassiveAbilitySystem;
using MacroTools.Spells;
using MacroTools.SpellSystem;

namespace WarcraftLegacies.Source.Setup.Spells
{
  public static class ScarletSpellSetup
  {
    public static void Setup()
    { 
      PassiveAbilityManager.Register(new NoTargetSpellOnCast(UNIT_H08H_HIGH_INQUISITOR_SCARLET, ABILITY_Z3X2_HEALING_FRENZY_ICON_SALLY)
      {
        DummyAbilityId = ABILITY_Z3X9_HEALING_FRENZY_SALLY_DUMMY,
        DummyOrderId = OrderId("fanofknives"),
        ProcChance = 1.0f,
        AbilityWhitelist = new List<int>
        {
          ABILITY_ANHW_HEALING_WAVE_PINK_VOL_JIN,
          ABILITY_Z9X3_SWIFT_HOLY_LIGHT_SALLY,
          ABILITY_A078_SPIRITUAL_GUIDANCE_SALLY,
          ABILITY_A0DK_DISPEL_MAGIC_SALLY,
        }
      });

      var crusaderShout = new Stomp(ABILITY_A0KB_CRUSADER_S_SHOUT_SAIDEN)
      {
        Radius = 600,
        DamageBase = 00,
        DamageLevel = 00,
        DurationBase = 2,
        DurationLevel = 1,
        StunAbilityId = ABILITY_A0KD_SOUL_BURN_SAIDEN_DUMMY,
        StunOrderId = OrderId("soulburn"),
        SpecialEffect = @"war3mapImported\RoarCasterScarlet.mdx"
      };
      SpellSystem.Register(crusaderShout);

      PassiveAbilityManager.Register(new SummonUnitOnDeath(UNIT_N09N_BISHOP_OF_THE_LIGHT_SCARLET)
      {
        Duration = 30,
        SummonUnitTypeId = UNIT_ST6W_UNHOLY_ARCHON_SCARLET_QUEST,
        SummonCount = 1,
        SpecialEffectPath = @"Abilities\Spells\Undead\DarkRitual\DarkRitualTarget.mdl",
        RequiredResearch = UPGRADE_R040_QUEST_COMPLETED_ONSLAUGHT
      });

      var recklessOnslaught = new CooldownReset(ABILITY_A0TC_RECKLESS_ONSLAUGHT_SCARLET);
      SpellSystem.Register(recklessOnslaught);

    }
  }
}