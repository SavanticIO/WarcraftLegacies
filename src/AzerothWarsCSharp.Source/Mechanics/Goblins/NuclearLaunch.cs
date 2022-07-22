﻿using AzerothWarsCSharp.MacroTools;
using AzerothWarsCSharp.MacroTools.SpellSystem;
using AzerothWarsCSharp.MacroTools.Wrappers;
using WCSharp.Shared.Data;
using static War3Api.Common;

namespace AzerothWarsCSharp.Source.Mechanics.Goblins
{
  public sealed class NuclearLaunch : Spell
  {
    private readonly float _castTime;
    private readonly int _dummyNukeLeftOverId;
    private readonly int _nuclearWarningUnitTypeId;
    private readonly string _warningSoundPath;

    public NuclearLaunch(int id, string warningSoundPath, int nuclearWarningUnitTypeId, int dummyNukeLeftOverId,
      float castTime) : base(id)
    {
      _warningSoundPath = warningSoundPath;
      _nuclearWarningUnitTypeId = nuclearWarningUnitTypeId;
      _dummyNukeLeftOverId = dummyNukeLeftOverId;
      _castTime = castTime;
    }

    public override void OnStartCast(unit caster, unit target, Point targetPoint)
    {
      var sound = new SoundWrapper(_warningSoundPath);
      sound.Play(true);
      var dummyNukeWarning =
        CreateUnit(caster.OwningPlayer(), _nuclearWarningUnitTypeId, targetPoint.X, targetPoint.Y, 0);
      UnitApplyTimedLife(dummyNukeWarning, 0, _castTime);
    }

    public override void OnCast(unit caster, unit target, float targetX, float targetY)
    {
      var dummyNukeLeftover = CreateUnit(caster.OwningPlayer(), _dummyNukeLeftOverId, targetX, targetY, 0);
      UnitApplyTimedLife(dummyNukeLeftover, 0, 3);
      dummyNukeLeftover.IssueOrder("flamestrike", new Point(targetX, targetY));
    }
  }
}