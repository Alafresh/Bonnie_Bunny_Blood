using System;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private Unit _unit;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetMouseWorldPosition());
            GridPosition startGridPosition = new GridPosition(0, 0);
            
            List<GridPosition> gridPositionList = Pathfinding.Instance.FindPath(startGridPosition, mouseGridPosition);

            Debug.Log("hola" + gridPositionList.Count);
            for (int i = 0; i < gridPositionList.Count - 1; i++)
            {
                Debug.Log("hola" + gridPositionList[i]);
                Debug.DrawLine(
                        LevelGrid.Instance.GetWorldPosition(gridPositionList[i]),
                        LevelGrid.Instance.GetWorldPosition(gridPositionList[i+1]),
                        Color.white,
                        10f
                    );
            }
        }
    }
}
