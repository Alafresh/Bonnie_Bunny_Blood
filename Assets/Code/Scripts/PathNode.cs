using UnityEngine;

public class PathNode
{
    private GridPosition _gridPosition;
    private int _gCost, _hCost, _fCost;
    private PathNode _beforeNode;
    
    public PathNode(GridPosition gridPosition)
    {
        _gridPosition = gridPosition;
    }

    public override string ToString()
    {
        return _gridPosition.ToString();
    }

    public int GetGCost()
    {
        return _gCost;
    }
    public int GetHCost()
    {
        return _hCost;
    }
    public int GetFCost()
    {
        return _fCost;
    }
}
