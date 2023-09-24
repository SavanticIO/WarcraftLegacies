using System;
using System.Collections.Generic;
using War3Api;
using WCSharp.SaveLoad;
using WCSharp.Shared;

namespace MacroTools.Save
{
  /// <summary>
  /// Manager class for saving and loading player settings.
  /// </summary>
  public static class SaveManager
  {
    public static Dictionary<Common.player, PlayerSettings> SavesByPlayer { get; } = new();
    private static SaveSystem<PlayerSettings> saveSystem;
	
    public static void Initialize()
    {
      saveSystem = new SaveSystem<PlayerSettings>(new SaveSystemOptions
      {
        Hash1 = 36653,
        Hash2 = 612319,
        Salt = "zCi5fkypenPpgukyoEW8H6YC",
        BindSavesToPlayerName = true,
        SaveFolder = "WarcraftLegacies"
      });

      saveSystem.OnSaveLoaded += SaveManager_OnSaveLoaded;

      foreach (var player in Util.EnumeratePlayers())
      {
        saveSystem.Load(player);
      }
    }
	
    public static void SaveManager_OnSaveLoaded(PlayerSettings save, LoadResult loadResult)
    {
      SavesByPlayer[save.GetPlayer()] = save;
		
      // If the load result is anything except success, the save will be a newly created object
      if (loadResult == LoadResult.FailedHash)
      {
        Console.WriteLine(
          $"Validating save file for {Common.GetPlayerName(save.GetPlayer())} failed! The game should probably be restarted.");
      }
      // Extension method for determining whether the load result is any of the failed states
      if (loadResult.Failed())
      {
        Console.WriteLine("An existing save failed to load correctly!");
      }
    }
	
    /// <summary>
    /// Saves the player settings for the given player.
    /// </summary>
    /// <param name="save"></param>
    public static void Save(PlayerSettings save)
    {
      saveSystem.Save(save);
    }
  }
}