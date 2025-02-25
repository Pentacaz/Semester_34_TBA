using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    private RoomNode rootNode;
    private List<RoomNode> allSpaceNodes = new List<RoomNode>();
    private int dungeonWidth;
    private int dungeonLength;
  
    
    public DungeonGenerator(int dungeonWidth, int dungeonLength)
    {

        this.dungeonWidth = dungeonWidth;
        this.dungeonLength = dungeonLength;
    }
    

    public List<Node> CalculateRooms(int maxIterations, int roomWidthMin, int roomLenghtMin, float roomBottomCornerModifier, float roomTopCornerModifier, int roomOffset, int corridorWidth)
    {
        BinarySpacePartitioner bsp = new BinarySpacePartitioner(dungeonWidth, dungeonLength);
        allSpaceNodes = bsp.PrepareNodesCollection(maxIterations, roomWidthMin, roomLenghtMin);
        List<Node> roomSpaces = StructureHelper.TraverseGraphTpExtractLowestLeaves(bsp.RoomNode);

        RoomGenerator roomGenerator = new RoomGenerator(maxIterations, roomLenghtMin, roomWidthMin);
        List <RoomNode> roomList = roomGenerator.GenerateRoomsInGivenSpaces(roomSpaces,roomBottomCornerModifier,roomTopCornerModifier,roomOffset);

        CorridorGenerator corridorGenerator = new CorridorGenerator();
        var corridorList = corridorGenerator.CreateCorridor(allSpaceNodes, corridorWidth);
        
        return new List<Node>(roomList).Concat(corridorList).ToList();
    }
    
    
}


