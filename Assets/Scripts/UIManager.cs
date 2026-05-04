using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PathfindingManager pathfindingManager;
    public TMP_Dropdown dropdown;
    public TMP_Text infoText;

    public void Solve()
    {
        bool useAStar = dropdown.value == 1;

        string method = useAStar ? "A*" : "Voraz";

        infoText.text = "Resolviendo con: " + method;

        pathfindingManager.Solve(useAStar);
    }

    public void SetObstacleMode()
    {
        infoText.text = "Modo: Colocar obstáculos";
    }

    public void SetStartMode()
    {
        infoText.text = "Modo: Seleccionar inicio";
    }

    public void SetGoalMode()
    {
        infoText.text = "Modo: Seleccionar destino";
    }

}