using MacroTools.Commands;
using MacroTools.CommandSystem;

namespace TestMap.Source.Setup
{
  public static class CommandSetup
  {
    public static void Setup(CommandManager commandManager)
    {
      commandManager.Register(new Limited());
      commandManager.Register(new Clear("clear"));
      commandManager.Register(new Clear("c"));
      commandManager.Register(new Cam());
      commandManager.Register(new QuestText());
      commandManager.Register(new Captions());
      commandManager.Register(new Dialouge());
    }
  }
}