using System;
using UnityEngine;
using UnityEngine.UI;

namespace AgeOfPhoenix.Client.System.Scripts.Components.GUI
{
  [RequireComponent(typeof(CanvasRenderer))]
  public class EmptyGraphic : Graphic, ICanvasRaycastFilter
  {
    protected override void OnPopulateMesh ( VertexHelper vh )
    {
      vh.Clear();
    }
   
    public Boolean IsRaycastLocationValid ( Vector2 sp, Camera eventCamera )
    {
      return true;
    }
  }
}