using System;
using MacroTools.Extensions;
using static War3Api.Common;

namespace MacroTools.Libraries
{
  /// <summary>
  /// Class for handling operations to do with the game cache 
  /// </summary>
  public static class GameCache
  {
    /// <summary>
    /// 
    /// </summary>
    public static gamecache? WarcraftLegaciesCache { get; private set; }

    /// <summary>
    /// Setup for class
    /// </summary>
    public static void Setup()
    {
      WarcraftLegaciesCache = InitGameCache("WarcraftLegaciesSettings");
    }

    /// <summary>
    /// Loops over all the players and syncs the cached values
    /// </summary>
    public static void SyncCache()
    {
      foreach (var player in WCSharp.Shared.Util.EnumeratePlayers())
      {
        PlayerData.ByHandle(player).UpdatePlayerSetting("CamDistance", Convert.ToInt32(LoadPlayerSettingInteger(player,"CamDistance")));
        PlayerData.ByHandle(player).UpdatePlayerSetting("ShowQuestText", Convert.ToBoolean(LoadPlayerSettingBoolean(player,"ShowQuestText")));
        PlayerData.ByHandle(player).UpdatePlayerSetting("ShowCaptions", Convert.ToBoolean(LoadPlayerSettingBoolean(player,"ShowCaptions")));
        PlayerData.ByHandle(player).UpdatePlayerSetting("PlayDialogue", Convert.ToBoolean(LoadPlayerSettingBoolean(player,"PlayDialogue")));
      }
      FlushGameCache(WarcraftLegaciesCache);
    }
    
    /// <summary>
    /// Saves a boolean value for a player to the game cache
    /// </summary>
    /// <param name="player"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void SavePlayerSetting(player player,string key, bool value)
    {
      StoreBoolean(WarcraftLegaciesCache, I2S(GetPlayerId(player)), key, value);
    }
    
    /// <summary>
    /// Saves an integer value for a player to the game cache
    /// </summary>
    /// <param name="player"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void SavePlayerSetting(player player,string key, int value)
    {
      StoreInteger(WarcraftLegaciesCache, I2S(GetPlayerId(player)), key, value);
    }
    
    /// <summary>
    /// Saves a string value for a player to the game cache
    /// </summary>
    /// <param name="player"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void SavePlayerSetting(player player,string key, string value)
    {
      StoreString(WarcraftLegaciesCache, I2S(GetPlayerId(player)), key, value);
    }
    
    private static int LoadPlayerSettingInteger(player player,string key)
    {
      return GetStoredInteger(WarcraftLegaciesCache, I2S(GetPlayerId(player)), key);
    }
    
    private static bool LoadPlayerSettingBoolean(player player,string key)
    {
      return GetStoredBoolean(WarcraftLegaciesCache, I2S(GetPlayerId(player)), key);
    }
  }
}