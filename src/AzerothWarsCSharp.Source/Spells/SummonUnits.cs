﻿using AzerothWarsCSharp.Source.Libraries;
using AzerothWarsCSharp.Source.Libraries.SpellSystem;
using static War3Api.Common;

namespace AzerothWarsCSharp.Source.Spells
{
  public class SummonUnits : Spell
  {
    public int SummonUnitTypeId { get; init; } = FourCC("hfoo");
    public int SummonCount { get; init; } = 1;
    public float Duration { get; init; } = 60;
    public float Radius { get; init; } = 150;
    public float AngleOffset { get; init; } = 45;
    public string Effect { get; init; }
    
    public SummonUnits(int id) : base(id)
    {
    }

    public override void OnCast(unit caster, widget target, float targetX, float targetY)
    {
      var casterX = GetUnitX(caster);
      var casterY = GetUnitY(caster);
      var angle = 0f;
      for (var i = 0; i < 4; i++)
      {
        angle += 360f / SummonCount;
        var summonX = MathEx.GetPolarOffsetX(GetUnitX(GetTriggerUnit()), Radius, angle);
        var summonY = MathEx.GetPolarOffsetY(GetUnitY(GetTriggerUnit()), Radius, angle);
        var summonFacing = MathEx.GetAngleBetweenPoints(summonX, summonY, casterX, casterY);
        var summonedUnit = CreateUnit(GetOwningPlayer(caster), SummonUnitTypeId, summonX, summonY, summonFacing);
        UnitApplyTimedLife(summonedUnit, 0, Duration);
        UnitAddType(summonedUnit, UNIT_TYPE_SUMMONED);
        SetUnitAnimation(summonedUnit, "birth");
        QueueUnitAnimation(summonedUnit, "stand");
        DestroyEffect(AddSpecialEffect(Effect, summonX, summonY));
      }
    }
  }
}