using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
   private int maxIterations;
   private int roomLenghtMin;
   private int roomWidthMin;
   public RoomGenerator(int maxIterations, int roomLenghtMin, int roomWidthMin)
   {
      this.maxIterations = maxIterations;
      this.roomLenghtMin = roomLenghtMin;
      this.roomWidthMin = roomWidthMin;
   }

   public List<RoomNode> GenerateRoomsInGivenSpaces(List<Node> roomSpaces,float roomBottomCornerModifier, float roomTopCornerModifier, int roomOffset)
   {
      List<RoomNode> listToReturn = new List<RoomNode>();
      foreach (var space in roomSpaces)
      {
         Vector2Int newBottomLeftPoint = StructureHelper.GenerateBottomLeftCornerBetween(
            space.bottomLeftAreaCorner, space.topRightAreaCorner, roomBottomCornerModifier, roomOffset);
         
         Vector2Int newTopRightPoint = StructureHelper.GenerateTopRightCornerBetween(
            space.bottomLeftAreaCorner, space.topRightAreaCorner, roomTopCornerModifier, roomOffset);

         space.bottomLeftAreaCorner = newBottomLeftPoint;
         space.topRightAreaCorner = newTopRightPoint;
         space.bottomRightAreaCorner = new Vector2Int(newTopRightPoint.x, newBottomLeftPoint.y);
         space.topLeftAreaCorner = new Vector2Int(newBottomLeftPoint.x, newTopRightPoint.y);
         listToReturn.Add((RoomNode)space);
      }

      return listToReturn;
   }
}
