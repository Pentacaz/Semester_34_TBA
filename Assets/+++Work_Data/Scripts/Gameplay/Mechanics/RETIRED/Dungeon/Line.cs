using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line
{
    private Orientation _orientation;
    private Vector2Int coordinates;

    public Line(Orientation orientation, Vector2Int coordinates)
    {
        this._orientation = orientation;
        this.coordinates = coordinates;
    }

    public Orientation orientation
    {
        get => _orientation;
        set => _orientation = value;
    }
    
    public Vector2Int Coordinates
    {
        get => coordinates;
        set => coordinates = value;
    }
    
}

public enum Orientation
{
    Horizontal = 0,
    Vertical = 1
}
