﻿using MacroTools.BookSystem.Core;
using MacroTools.Frames;
using static War3Api.Common;

namespace MacroTools.BookSystem.ArtifactSystem
{
  public sealed class ArtifactPageFactory : IPageFactory<ArtifactPage>
  {
    /// <inheritdoc />
    public ArtifactPage Create(float width, float height, int pageNumber, Frame parent)
    {
      var newPage = new ArtifactPage(width, height)
      {
        PageNumber = pageNumber,
        Parent = parent,
        Visible = false
      };
      newPage.SetPoint(FRAMEPOINT_CENTER, parent, FRAMEPOINT_CENTER, 0, 0);
      
      return newPage;
    }
  }
}