public class Node
{
    public int x, y;

    public float gCost = 0;
    public float hCost = 0;

    public float fCost => gCost + hCost;

    public Node parent;

    public Node(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}