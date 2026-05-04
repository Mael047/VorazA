using UnityEngine;

public class GridEditor : MonoBehaviour
{
    public EditMode currentMode;
    public PathfindingManager pathfindingManager;

    public void OnCellClicked(Cell cell)
    {
        switch (currentMode)
        {
            case EditMode.Obstacle:
                cell.isObstacle = true;
                cell.SetColor(Color.black);
                break;

            case EditMode.Start:
                pathfindingManager.SetStart(cell);
                cell.SetColor(Color.green); 
                break;

            case EditMode.Goal:
                pathfindingManager.SetGoal(cell);
                cell.SetColor(Color.red); 
                break;
        }
    }

    public void SetObstacleMode()
    {
        currentMode = EditMode.Obstacle;
    }

    public void SetStartMode()
    {
        currentMode = EditMode.Start;
    }

    public void SetGoalMode()
    {
        currentMode = EditMode.Goal;
    }



}