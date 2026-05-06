using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{

    public TMP_InputField inputRows;
    public TMP_InputField inputCols;

    public GameObject cellPrefab;
    public int rows = 10;
    public int cols = 10;

    public Cell[,] grid;
    public Node[,] nodes;

    public Transform parent;

    public void GenerateGrid()
    {
        rows = int.Parse(inputRows.text);
        cols = int.Parse(inputCols.text);

        GridLayoutGroup layout = parent.GetComponent<GridLayoutGroup>();
        layout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        layout.constraintCount = cols;

        grid = new Cell[rows, cols];
        nodes = new Node[rows, cols];

        // limpiar grid anterior
        foreach (Transform child in parent)
            Destroy(child.gameObject);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                GameObject obj = Instantiate(cellPrefab, parent);
                Cell cell = obj.GetComponent<Cell>();
                cell.editor = FindObjectOfType<GridEditor>();

                cell.x = i;
                cell.y = j;

                grid[i, j] = cell;
                nodes[i, j] = new Node(i, j);

            }
        }

        Debug.Log("Generando grid...");
    }
    

    public void ClearPathVisuals()
    {
        foreach (var cell in grid)
        {
            if (!cell.isObstacle && !cell.isStart && !cell.isGoal)
            {
                cell.SetColor(Color.white);
            }
        }
    }
    public void ResetNodes()
    {
        foreach (var node in nodes)
        {
            node.parent = null;
            node.gCost = 0;
            node.hCost = 0;
        }
    }

}