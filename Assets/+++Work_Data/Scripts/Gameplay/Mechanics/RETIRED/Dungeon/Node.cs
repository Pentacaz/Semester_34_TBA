using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;

public abstract class Node
{
    private List<Node> _childrenNodeList;

    public List<Node> childrenNodeList
    {
        get => _childrenNodeList;
    }

    public bool hasVisited { get; set; }
    public Vector2Int bottomLeftAreaCorner;
    public Vector2Int bottomRightAreaCorner;
    public Vector2Int topLeftAreaCorner;
    public Vector2Int topRightAreaCorner;

    public int treeLayerIndex { get; set; }
    public Node parent { get; set; }


    public Node(Node parentNode)
    {
        _childrenNodeList = new List<Node>();
        this.parent = parentNode;
        if (parentNode != null)
        {
            parentNode.AddChild(this);
        }

    }

   public void AddChild(Node node)
    {
        _childrenNodeList.Add(node);
    }

    public void RemoveChild(Node node)
    {
        _childrenNodeList.Remove(node);
    }
}
