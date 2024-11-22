using System;
using UnityEngine;

public class PathfindingUpdater : MonoBehaviour
{
    private void Start()
    {
        DesctructibleCrate.OnAnyDestroyed += DesctructibleCrate_OnAnyDestroyed;
    }

    private void DesctructibleCrate_OnAnyDestroyed(object sender, EventArgs e)
    {
        DesctructibleCrate destructibleCrate = sender as DesctructibleCrate;
        
        Pathfinding.Instance.SetIsWalkableGriPosition(destructibleCrate.GetGridPosition(), true);
    }
}
