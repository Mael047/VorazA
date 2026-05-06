using UnityEngine;

public class GridEditor : MonoBehaviour
{
    public EditMode currentMode;
    public PathfindingManager pathfindingManager;

    public Color obstacleColor;
    public Color ColorStart;
    public Color ColorGoal;

    public void OnCellClicked(Cell cell)
    {
        switch (currentMode)
        {
            case EditMode.Obstacle:
                cell.isObstacle = true;
                cell.SetColor(obstacleColor);
                break;

            case EditMode.Start:
                pathfindingManager.SetStart(cell);
                cell.SetColor(ColorStart); 
                break;

            case EditMode.Goal:
                pathfindingManager.SetGoal(cell);
                cell.SetColor(ColorGoal); 
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