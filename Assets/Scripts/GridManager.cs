using UnityEngine;
using TMPro;

public class GridManager : MonoBehaviour
{

    public TMP_InputField inputRows;
    public TMP_InputField inputCols;

    public GameObject cellPrefab;
    public int rows = 10;
    public int cols = 10;

    public Cell[,] grid;

    public Transform parent;

    public void GenerateGrid()
    {
        rows = int.Parse(inputRows.text);
        cols = int.Parse(inputCols.text);

        grid = new Cell[rows, cols];

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
            }
        }

        Debug.Log("Generando grid...");
    }
}