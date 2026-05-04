using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IPointerClickHandler
{
    public GridEditor editor;

    public int x, y;

    public bool isObstacle;
    public bool isStart;
    public bool isGoal;

    public Image img;

    void Awake()
    {
        img = GetComponent<Image>();
    }

    public void SetColor(Color color)
    {
        img.color = color;
    }

    // 👇 ESTO ES LO QUE FALTABA
    public void OnPointerClick(PointerEventData eventData)
    {
        if (editor != null)
        {
            editor.OnCellClicked(this);
        }
        else
        {
            Debug.LogError("Editor no asignado en Cell");
        }
    }


}