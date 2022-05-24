// using AzerothWarsCSharp.MacroTools.FactionSystem;
//
// using static War3Api.Common;  using static AzerothWarsCSharp.MacroTools.GeneralHelpers;
// {
//   public static class TitanForgeArtifact
//   {
//     private static readonly int RESEARCH_ID = FourCC("R08K");
//
//     private static void Research()
//     {
//       var heldItem = UnitItemInSlot(GetTriggerUnit(), 0);
//       Artifact heldArtifact = Artifact.GetFromTypeId(GetItemTypeId(heldItem));
//       if (heldItem != null && heldArtifact != null && heldArtifact.Titanforged == false)
//       {
//         heldArtifact.Titanforge();
//       }
//       else
//       {
//         GetTriggerPlayer().AdjustPlayerState(PLAYER_STATE_RESOURCE_GOLD, 1000);
//         GetTriggerPlayer().AdjustPlayerState(PLAYER_STATE_RESOURCE_LUMBER, 750);
//       }
//
//       SetPlayerTechResearched(GetTriggerPlayer(), RESEARCH_ID, 1);
//     }
//
//     public static void Setup()
//     {
//       RegisterResearchFinishedAction(RESEARCH_ID, Research);
//     }
//   }
// }