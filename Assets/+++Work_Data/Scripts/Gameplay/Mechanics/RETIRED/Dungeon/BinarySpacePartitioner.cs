using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinarySpacePartitioner
{
   private RoomNode _rootNode;
   
   public RoomNode RoomNode
   {
      get => _rootNode;
   }
   
   public BinarySpacePartitioner(int dungeonWidth, int dungeonLength)
   {
      this._rootNode = new RoomNode(new Vector2Int(0, 0), new Vector2Int(dungeonWidth, dungeonLength), null, 0);
   }


   public List<RoomNode> PrepareNodesCollection(int maxIterations, int roomWidthMin, int roomLenghtMin)
   {
      Queue<RoomNode> graph = new Queue<RoomNode>();
      List<RoomNode> listToReturn = new List<RoomNode>();
      graph.Enqueue(this._rootNode);
      listToReturn.Add(this._rootNode);
      int iterations = 0;
      while (iterations < maxIterations && graph.Count > 0)
      {
         iterations++;
         RoomNode currentNode = graph.Dequeue();
         if (currentNode.Width >= roomWidthMin * 2 || currentNode.Length >= roomLenghtMin * 2)
         {
            SplitTheSpace(currentNode, listToReturn, roomLenghtMin, roomWidthMin, graph);
         }
      }
      return listToReturn;
   }

   private void SplitTheSpace(RoomNode currentNode, List<RoomNode> listToReturn, int roomLenghtMin, int roomWidthMin, Queue<RoomNode> graph)
   {
      Line line = GetLineDividingSpace(currentNode.bottomLeftAreaCorner, currentNode.topRightAreaCorner, roomWidthMin,
         roomLenghtMin);

      RoomNode node1, node2;
      if (line.orientation == Orientation.Horizontal)
      {
         node1 = new RoomNode(currentNode.bottomLeftAreaCorner,
            new Vector2Int(currentNode.topRightAreaCorner.x, line.Coordinates.y),
            currentNode, currentNode.treeLayerIndex + 1);
         
         node2 = new RoomNode(new Vector2Int(currentNode.bottomLeftAreaCorner.x,
         line.Coordinates.y),currentNode.topRightAreaCorner,
            currentNode, currentNode.treeLayerIndex + 1);

      }
      else
      {
         node1 = new RoomNode(currentNode.bottomLeftAreaCorner,new Vector2Int(line.Coordinates.x,currentNode.topRightAreaCorner.y),
            currentNode,currentNode.treeLayerIndex+ 1);
         
         node2 = new RoomNode(new Vector2Int(line.Coordinates.x,currentNode.bottomLeftAreaCorner.y),
            currentNode.topRightAreaCorner,currentNode,currentNode.treeLayerIndex +1);

      }

      AddNewNodeToCollections(listToReturn, graph, node1);
      AddNewNodeToCollections(listToReturn, graph, node2);
   }

   private void AddNewNodeToCollections(List<RoomNode> listToReturn, Queue<RoomNode> graph, RoomNode node)
   {
      listToReturn.Add(node);
      graph.Enqueue(node);
   }

   private Line GetLineDividingSpace(Vector2Int bottomLeftAreaCorner, Vector2Int topRightAreaCorner, int roomWidthMin, int roomLenghtMin)
   {
      Orientation orientation;
      bool lengthStatus = (topRightAreaCorner.y - bottomLeftAreaCorner.y) >= 2 * roomLenghtMin;
      bool widthStatus = (topRightAreaCorner.x - bottomLeftAreaCorner.x) >= 2 * roomWidthMin;
      if (lengthStatus && widthStatus)
      {
         orientation = (Orientation)(Random.Range(0,2));
      }else if (widthStatus)
      {
         orientation = Orientation.Vertical;
      }
      else
      {
         orientation = Orientation.Horizontal;
      }

      return new Line(orientation, GetCoordinatesForOrientation(orientation, bottomLeftAreaCorner,topRightAreaCorner,roomWidthMin,roomLenghtMin));
   }

   private Vector2Int GetCoordinatesForOrientation(Orientation orientation, Vector2Int bottomLeftAreaCorner, Vector2Int topRightAreaCorner, int roomWidthMin, int roomLenghtMin)
   {
      Vector2Int coordiates = Vector2Int.zero;
      if (orientation == Orientation.Horizontal)
      {
         coordiates = new Vector2Int(0,Random.Range((bottomLeftAreaCorner.y + roomLenghtMin),(topRightAreaCorner.y-roomLenghtMin)));
      }
      else
      {
         coordiates = new Vector2Int(Random.Range((bottomLeftAreaCorner.x + roomWidthMin),(topRightAreaCorner.x-roomWidthMin)),0);
      }

      return coordiates;
   }
}
