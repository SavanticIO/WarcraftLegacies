﻿using System;
using System.Collections.Generic;
using MacroTools.ControlPointSystem;
using MacroTools.FactionSystem;
using MacroTools.Libraries;
using static War3Api.Common;

namespace MacroTools.Extensions
{
  /// <summary>
  /// Provides extra information about players that is not already tracked by the Warcraft 3 engine.
  /// </summary>
  internal sealed class PlayerData
  {
    /// <summary>
    /// Fired when the player leaves a team.
    /// </summary>
    public event EventHandler<PlayerChangeTeamEventArgs>? PlayerLeftTeam;
    
    /// <summary>
    /// Fired when the player joins a team.
    /// </summary>
    public event EventHandler<PlayerChangeTeamEventArgs>? PlayerJoinedTeam;

    /// <summary>
    /// Fired when the player changes their <see cref="Faction"/>.
    /// </summary>
    public event EventHandler<PlayerFactionChangeEventArgs>? ChangedFaction;
    
    private static readonly Dictionary<int, PlayerData> ById = new();
    private readonly Dictionary<int, int> _objectLevels = new();

    private readonly Dictionary<int, int> _objectLimits = new();
    private float _baseIncome; //Gold per minute
    private float _bonusIncome;

    private Team? _team;
    private Faction? _faction;

    private float _partialGold; //Just used for income calculations
    private float _partialLumber;

    private int? _camDistance;
    private bool? _showCaptions;
    private bool? _playDialoge;
    private bool? _showQuestText;

    private PlayerData(player player)
    {
      Player = player;
      EliminationTurns = 0;
      
    }

    private player Player { get; }

    /// <summary>
    /// Controls who the player is allied to.
    /// </summary>
    public Team? Team
    {
      get => _team;
      set
      {
        if (_team != null)
        {
          _team?.RemovePlayer(Player);
          PlayerLeftTeam?.Invoke(this, new PlayerChangeTeamEventArgs(Player, _team));
        }

        if (value == null) return;
        var prevTeam = _team;
        _team = value;
        value.AddPlayer(Player);
        PlayerJoinedTeam?.Invoke(this, new PlayerChangeTeamEventArgs(Player, prevTeam));
      }
    }
    
    /// <summary>
    ///   Controls name, available objects, color, and icon.
    /// </summary>
    public Faction? Faction
    {
      get => _faction;
      set
      {
        var prevFaction = Faction;

        //Unapply old faction
        if (_faction != null)
        {
          _faction = null;
          if (prevFaction != null)
            prevFaction.Player = null; //Referential integrity
        }

        //Apply new faction
        if (value != null)
        {
          if (value.Player == null)
          {
            Player.SetColor(value.PlayerColor, true);
            _faction = value;
            //Enforce referential integrity
            if (value.Player != Player)
              value.Player = Player;
          }
          else
          {
            throw new Exception("Attempted to Person " + GetPlayerName(Player) +
                                " to already occupied faction with name " + value.Name);
          }
        }

        FactionChange?.Invoke(this, new PlayerFactionChangeEventArgs(Player, prevFaction));
        ChangedFaction?.Invoke(this, new PlayerFactionChangeEventArgs(Player, prevFaction));
      }
    }

    public int EliminationTurns { get; set; }
    
    public float LumberIncome { get; set; }

    /// <summary>
    ///   Gold per second gained from all sources.
    /// </summary>
    public float TotalIncome => BaseIncome + BonusIncome;

    /// <summary>
    ///   Gold per second gained from secondary sources like Forsaken's plagued buildings.
    /// </summary>
    public float BonusIncome
    {
      get => _bonusIncome;
      set
      {
        _bonusIncome = value;
        IncomeChanged?.Invoke(this, this);
      }
    }
    
    public int CamDistance
    {
      get => _camDistance ?? 700;
      set
      {
        _camDistance = Math.Clamp(value, 700, 2701);
        DisplayTextToPlayer(GetLocalPlayer(), 0, 0, $"Set CamDistance for {GetPlayerName(GetLocalPlayer())} to {_camDistance}");
        Player.ApplyCameraField(CAMERA_FIELD_TARGET_DISTANCE, CamDistance, 1);
        if (Player == GetLocalPlayer())
        {
          FileIo.Write(Player);
        }
      }
    }
    
    public bool ShowCaptions
    {
      get => _showCaptions ?? true;
      set
      {
        _showCaptions = value;
        DisplayTextToPlayer(GetLocalPlayer(), 0, 0, $"Set ShowCaptions for {GetPlayerName(GetLocalPlayer())} to {_showCaptions}");
        if (Player == GetLocalPlayer())
          FileIo.Write(Player);
      }
    }
    
    public bool ShowQuestText
    {
      get => _showQuestText ?? true;
      set
      {
        _showQuestText = value;
        DisplayTextToPlayer(GetLocalPlayer(), 0, 0, $"Set ShowQuestText for {GetPlayerName(GetLocalPlayer())} to {_showQuestText}");
        if (Player == GetLocalPlayer())
          FileIo.Write(Player);
      }
    }

    public bool PlayDialogue
    {
      get => _playDialoge ?? true;
      set
      {
        _playDialoge = value;
        DisplayTextToPlayer(GetLocalPlayer(), 0, 0, $"Set PlayDialogue for {GetPlayerName(GetLocalPlayer())} to {_playDialoge}");
        if (Player == GetLocalPlayer())
          FileIo.Write(Player);
      }
    }

    /// <summary>
    ///   Gold per second gained from primary sources like Control Points.
    /// </summary>
    public float BaseIncome
    {
      get => _baseIncome;
      set
      {
        if (value < 0)
          throw new ArgumentOutOfRangeException(
            $"Tried to assign a negative {nameof(BaseIncome)} value to {GetPlayerName(Player)}.");

        _baseIncome = value;
        IncomeChanged?.Invoke(this, this);
      }
    }

    /// <summary>
    /// Control points the player owns
    /// </summary>
    public List<ControlPoint> ControlPoints { get; set; } = new();

    /// <summary>
    /// Adds <see cref="ControlPoint" /> to list of this <see cref="player" />'s controlpoints, updates the <see cref="Team" /> total and fires any events subscribed to ControlPointsChanged
    /// </summary>
    /// <param name="controlPoint"></param>
    public void AddControlPoint(ControlPoint controlPoint)
    {
      ControlPoints.Add(controlPoint);
      ControlPointsChanged?.Invoke(this, this);
    }
    
    /// <summary>
    /// Removes <see cref="ControlPoint" /> from list of this <see cref="player" />'s controlpoints, updates the <see cref="Team" /> total and fires any events subscribed to ControlPointsChanged
    /// </summary>
    /// <param name="controlPoint"></param>
    public void RemoveControlPoint(ControlPoint controlPoint)
    {
      ControlPoints.Remove(controlPoint);
      ControlPointsChanged?.Invoke(this, this);
    }

    /// <summary>
    /// The number of extra <see cref="ControlPoint.ControlLevel"/>s the player gets each turn.
    /// </summary>
    public float ControlLevelPerTurnBonus { get; set; }

    /// <summary>
    /// Fired when the player's income changes.
    /// </summary>
    public event EventHandler<PlayerData>? IncomeChanged;
    
    /// <summary>
    /// Fired when the <see cref="player" />'s <see cref="ControlPoint" />s change
    /// </summary>
    public event EventHandler<PlayerData>? ControlPointsChanged;

    /// <summary>
    /// Fired when any player changes <see cref="Faction"/>.
    /// Todo: remove this and use the instance version instead.
    /// </summary>
    public static event EventHandler<PlayerFactionChangeEventArgs>? FactionChange;

    public int GetObjectLevel(int obj) => _objectLevels.ContainsKey(obj) ? _objectLevels[obj] : 0;

    public void SetObjectLevel(int obj, int level)
    {
      var objectLimit = Player.GetObjectLimit(obj);
      
      if (level > objectLimit)
        throw new ArgumentException(
          $"{nameof(level)} ({level}) cannot be higher than the object limit for {GetObjectName(obj)} ({objectLimit}).",
          $"{nameof(level)}");
      
      //Object levels cannot be changed for objects with a limit of 0.
      //This works around it by increasing the limit to 1 before making the change, then reverting it back.
      var revertAfter = false;

      if (objectLimit < 1)
      {
        SetPlayerTechMaxAllowed(Player, obj, 100);
        revertAfter = true;
      }

      SetPlayerTechResearched(Player, obj, Math.Max(level, 0));

      _objectLevels[obj] = level;
      if (revertAfter)
        SetPlayerTechMaxAllowed(Player, obj, 0);
      
      if (GetPlayerTechCount(Player, obj, false) != Math.Max(level, 0))
        throw new InvalidOperationException(
          $"Failed to set the object level of {GetObjectName(obj)} to {level}; it is {GetPlayerTechCount(Player, obj, false)} instead.");
    }

    public int GetObjectLimit(int id) => _objectLimits.TryGetValue(id, out var limit) ? limit : 0;

    public void SetObjectLimit(int id, int limit)
    {
      _objectLimits[id] = limit;

      if (limit >= Faction.UNLIMITED)
        SetPlayerTechMaxAllowed(Player, id, -1);
      else if (limit <= 0)
        SetPlayerTechMaxAllowed(Player, id, 0);
      else
        SetPlayerTechMaxAllowed(Player, id, limit);
    }

    public void ModObjectLimit(int id, int limit) =>
      SetObjectLimit(id, GetObjectLimit(id) + limit);

    public void AddGold(float x)
    {
      var fullGold = (float) Math.Floor(x);
      var remainderGold = x - fullGold;

      SetPlayerState(Player, PLAYER_STATE_RESOURCE_GOLD,
        GetPlayerState(Player, PLAYER_STATE_RESOURCE_GOLD) + R2I(fullGold));
      _partialGold += remainderGold;

      while (true)
      {
        if (_partialGold < 1) break;

        _partialGold -= 1;
        SetPlayerState(Player, PLAYER_STATE_RESOURCE_GOLD, GetPlayerState(Player, PLAYER_STATE_RESOURCE_GOLD) + 1);
      }
    }

    public void AddLumber(float x)
    {
      var fullLumber = (float) Math.Floor(x);
      var remainderLumber = x - fullLumber;

      SetPlayerState(Player, PLAYER_STATE_RESOURCE_LUMBER,
        GetPlayerState(Player, PLAYER_STATE_RESOURCE_LUMBER) + R2I(fullLumber));
      _partialLumber += remainderLumber;

      while (true)
      {
        if (_partialLumber < 1) break;

        _partialLumber -= 1;
        SetPlayerState(Player, PLAYER_STATE_RESOURCE_LUMBER, GetPlayerState(Player, PLAYER_STATE_RESOURCE_LUMBER) + 1);
      }
    }

    /// <summary>
    ///   Retrieves the <see cref="PlayerData" /> object which contains information about the given <see cref="player" />.
    /// </summary>
    public static PlayerData ByHandle(player whichPlayer)
    {
      if (ById.TryGetValue(GetPlayerId(whichPlayer), out var person)) return person;

      var newPerson = new PlayerData(whichPlayer);
      Register(newPerson);
      return newPerson;
    }

    /// <summary>
    ///   Register a <see cref="PlayerData" /> to the Person system.
    /// </summary>
    private static void Register(PlayerData playerData)
    {
      ById.Add(GetPlayerId(playerData.Player), playerData);
    }
  }
}