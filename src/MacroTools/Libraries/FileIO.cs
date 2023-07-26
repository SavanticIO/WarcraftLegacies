using System;
using MacroTools.Extensions;
using static War3Api.Common;

namespace MacroTools.Libraries
{
  public class FileIO
  {
    public class File
    {
      public readonly string fileName;
      public readonly string data;

      public File(player player)
      {
        fileName = GetPlayerName(player)+"_WarcraftLegaciesData";
        data = PlayerData.ByHandle(player).CamDistance + "-" + PlayerData.ByHandle(player).ShowQuestText + "-" + PlayerData.ByHandle(player).ShowCaptions + "-" + PlayerData.ByHandle(player).PlayDialogue;
      }
    }

    public static void Read(player player)
    {
      var currentPlayerName = GetPlayerName(GetLocalPlayer());
      Preloader($"WarcraftLegacies\\{GetPlayerName(player)+"_WarcraftLegaciesData"}.txt");
      var data = GetPlayerName(GetLocalPlayer()).Split('-');
      SetPlayerName(GetLocalPlayer(), currentPlayerName);
      if (data.Length != 4) return;
      PlayerData.ByHandle(player).CamDistance = Convert.ToInt32(data[0]);
      PlayerData.ByHandle(player).ShowQuestText = Convert.ToBoolean(data[1]);
      PlayerData.ByHandle(player).ShowCaptions = Convert.ToBoolean(data[2]);
      PlayerData.ByHandle(player).PlayDialogue = Convert.ToBoolean(data[3]);
    }

    public static void Write(player player)
    {
      var file = new File(player);
      PreloadGenClear();
      PreloadGenStart();
      Preload("\" )\ncall SetPlayerName(GetLocalPlayer(),\"" + file.data + "\") //");
      PreloadGenEnd($"WarcraftLegacies\\{file.fileName}.txt");
    }
  }
}