using System;
using MacroTools.Extensions;
using static War3Api.Common;

namespace MacroTools.Libraries
{
  /// <summary>
  /// Class to read and write player settings data from a file
  /// </summary>
  public static class FileIo
  {
    private sealed class File
    {
      public readonly string FileName;
      public readonly string Data;

      public File(player player)
      {
        FileName = GetPlayerName(player)+"_WarcraftLegaciesData";
        Data = PlayerData.ByHandle(player).CamDistance + "-" + PlayerData.ByHandle(player).ShowQuestText + "-" + PlayerData.ByHandle(player).ShowCaptions + "-" + PlayerData.ByHandle(player).PlayDialogue;
      }
    }

    /// <summary>
    /// Reads player settings data from a file
    /// </summary>
    /// <param name="player"></param>
    public static void Read(player player)
    {
      var currentPlayerName = GetPlayerName(player);
      Preloader($"WarcraftLegacies\\{currentPlayerName}_WarcraftLegaciesData.txt");
      var data = GetPlayerName(player).Split('-');
      SetPlayerName(player, currentPlayerName);
      if (data.Length != 4) return;
      PlayerData.ByHandle(player).CamDistance = Convert.ToInt32(data[0]);
      GameCache.SavePlayerSetting(player,"CamDistance",Convert.ToInt32(data[0]));
      
      PlayerData.ByHandle(player).ShowQuestText = Convert.ToBoolean(data[1]);
      GameCache.SavePlayerSetting(player,"ShowQuestText",Convert.ToBoolean(data[1]));
      
      PlayerData.ByHandle(player).ShowCaptions = Convert.ToBoolean(data[2]);
      GameCache.SavePlayerSetting(player,"ShowCaptions",Convert.ToBoolean(data[2]));
      
      PlayerData.ByHandle(player).PlayDialogue = Convert.ToBoolean(data[3]);
      GameCache.SavePlayerSetting(player,"PlayDialogue",Convert.ToBoolean(data[3]));
      SaveGameCache(GameCache.WarcraftLegaciesCache);
    }

    /// <summary>
    /// Writes player settings data to a file
    /// </summary>
    /// <param name="player"></param>
    public static void Write(player player)
    {
      var file = new File(player);
      PreloadGenClear();
      PreloadGenStart();
      Preload($"\" )\ncall SetPlayerName(GetLocalPlayer(),\"{file.Data}\") //");
      PreloadGenEnd($"WarcraftLegacies\\{file.FileName}.txt");
    }
  }
}