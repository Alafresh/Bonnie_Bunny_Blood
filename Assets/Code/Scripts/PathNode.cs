using UnityEngine;

public class PathNode
{
    private GridPosition _gridPosition;
    private int _gCost, _hCost, _fCost;
    private PathNode _beforeNode;
    private bool _isWalkable = true;
    
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

    public void SetGCost(int gCost)
    {
        _gCost = gCost;
    }
    public void SetHCost(int hCost)
    {
        _hCost = hCost;
    }
    public void CalculateFCost()
    {
        _fCost = _gCost + _hCost;
    }

    public void ResetCameFromPathNode()
    {
        _beforeNode = null;
    }

    public GridPosition GetGridPosition()
    {
        return _gridPosition;
    }

    public void SetCameFromPathNode(PathNode beforeNode)
    {
        
        _beforeNode = beforeNode;
    }
    public PathNode GetCameFromPathNode()
    {
        return _beforeNode;
    }

    public bool IsWalkable()
    {
        return _isWalkable;
    }

    public void SetIsWalkable(bool isWalkable)
    {
        _isWalkable = isWalkable;
    }
}
