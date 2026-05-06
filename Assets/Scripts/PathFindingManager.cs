using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathfindingManager : MonoBehaviour
{
    public GridManager gridManager;

    private Cell startCell;
    private Cell goalCell;

    // -------------------------
    // HEURÍSTICA (Manhattan)
    // -------------------------
    float Heuristic(Node a, Node b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    // -------------------------
    // VECINOS VÁLIDOS
    // -------------------------
    List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        int[][] directions = new int[][]
        {
        new int[] {1, 0},
        new int[] {-1, 0},
        new int[] {0, 1},
        new int[] {0, -1}
        };

        foreach (var dir in directions)
        {
            int newX = node.x + dir[0];
            int newY = node.y + dir[1];

            // 🔥 CAMBIO IMPORTANTE
            if (newX >= 0 && newX < gridManager.grid.GetLength(0) &&
                newY >= 0 && newY < gridManager.grid.GetLength(1))
            {
                Cell cell = gridManager.grid[newX, newY];

                if (!cell.isObstacle)
                    neighbors.Add(gridManager.nodes[newX, newY]);
            }
        }

        return neighbors;
    }

    // -------------------------
    // BÚSQUEDA VORAZ
    // -------------------------
    Node Greedy(Node start, Node goal)
    {
        List<Node> open = new List<Node>();
        List<Node> closed = new List<Node>();

        open.Add(start);

        start.hCost = Heuristic(start, goal);
        while (open.Count > 0)
        {
            Node current = open.OrderBy(n => n.hCost).First();

            if (current.x == goal.x && current.y == goal.y)
                return current;

            open.Remove(current);
            closed.Add(current);

            foreach (Node neighbor in GetNeighbors(current))
            {
                if (closed.Any(n => n.x == neighbor.x && n.y == neighbor.y) ||
                    open.Any(n => n.x == neighbor.x && n.y == neighbor.y))
                    continue;
                gridManager.grid[neighbor.x, neighbor.y].SetColor(Color.yellow);
                neighbor.hCost = Heuristic(neighbor, goal);
                neighbor.parent = current;

                open.Add(neighbor);
            }
        }

        return null;
    }

    // -------------------------
    // BÚSQUEDA A*
    // -------------------------
    Node AStar(Node start, Node goal)
    {
        List<Node> open = new List<Node>();
        List<Node> closed = new List<Node>();

        open.Add(start);

        while (open.Count > 0)
        {
            Node current = open.OrderBy(n => n.fCost).First();

            if (current.x == goal.x && current.y == goal.y)
                return current;

            open.Remove(current);
            closed.Add(current);

            foreach (Node neighbor in GetNeighbors(current))
            {
                if (closed.Any(n => n.x == neighbor.x && n.y == neighbor.y))
                    continue;

                float newCost = current.gCost + 1;
                gridManager.grid[neighbor.x, neighbor.y].SetColor(Color.yellow);

                Node existing = open.FirstOrDefault(n => n.x == neighbor.x && n.y == neighbor.y);

                if (existing == null)
                {
                    neighbor.gCost = newCost;
                    neighbor.hCost = Heuristic(neighbor, goal);
                    neighbor.parent = current;

                    open.Add(neighbor);
                }
                else if (newCost < existing.gCost)
                {
                    existing.gCost = newCost;
                    existing.parent = current;
                }
            }
        }

        return null;
    }

    // -------------------------
    // RECONSTRUIR CAMINO
    // -------------------------
    List<Node> ReconstructPath(Node end)
    {
        List<Node> path = new List<Node>();

        Node current = end;

        while (current != null)
        {
            path.Add(current);
            current = current.parent;
        }

        path.Reverse();
        return path;
    }

    // -------------------------
    // DIBUJAR CAMINO
    // -------------------------
    public void DrawPathAnimated(List<Node> path)
    {
        StartCoroutine(AnimatePath(path));
    }

    IEnumerator AnimatePath(List<Node> path)
    {
        foreach (Node n in path)
        {
            gridManager.grid[n.x, n.y].SetColor(Color.blue);
            yield return new WaitForSeconds(0.05f); // velocidad
        }
    }

    // -------------------------
    // EJECUTAR SOLUCIÓN
    // -------------------------
    public void Solve(bool useAStar)
    {
        // 👇 LIMPIA ANTES DE TODO
     
        gridManager.ClearPathVisuals();
        gridManager.ResetNodes();

        if (startCell == null || goalCell == null)
        {
            Debug.LogError("Falta definir inicio o destino");
            return;
        }

        Node start = gridManager.nodes[startCell.x, startCell.y];
        Node goal = gridManager.nodes[goalCell.x, goalCell.y];

        Node result;

        if (useAStar)
            result = AStar(start, goal);
        else
            result = Greedy(start, goal);

        if (result != null)
        {
            List<Node> path = ReconstructPath(result);
            DrawPathAnimated(path);
        }
        else
        {
            Debug.Log("No se encontró camino");
        }
    }

    // -------------------------
    // SETTERS DESDE UI
    // -------------------------
    public void SetStart(Cell cell)
    {
        if (startCell != null)
        {
            startCell.isStart = false;
            startCell.SetColor(Color.white);
        }

        startCell = cell;
        startCell.isStart = true;
        startCell.SetColor(Color.green);
    }

    public void SetGoal(Cell cell)
    {
        if (goalCell != null)
        {
            goalCell.isGoal = false;
            goalCell.SetColor(Color.white);
        }

        goalCell = cell;
        goalCell.isGoal = true;
        goalCell.SetColor(Color.red);
    }

    public void ResetGrid()
    {
        foreach (var cell in gridManager.grid)
        {
            cell.isObstacle = false;
            cell.SetColor(Color.white);
        }
    }
    
}