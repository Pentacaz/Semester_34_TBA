using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorGenerator : MonoBehaviour
{
    public List<Node> CreateCorridor(List<RoomNode> allSpaceNodes, int corridorWidth)
    {
        List<Node> corridorList = new List<Node>();
        Queue<RoomNode> structuresToCheck =
            new Queue<RoomNode>(allSpaceNodes.OrderByDescending(node => node.treeLayerIndex).ToList());
        while (structuresToCheck.Count > 0)
        {
            var node = structuresToCheck.Dequeue();
            if (node.childrenNodeList.Count == 0)
            {
                continue;
            }

            CorridorNode corridor = new CorridorNode(node.childrenNodeList[0], node.childrenNodeList[1], corridorWidth);
            corridorList.Add(corridor);
        }

        return corridorList;
    }
}
