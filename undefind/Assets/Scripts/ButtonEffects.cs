using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private Button button;
    private Color originalColor;
    public Color hoverColor = Color.yellow;
    public Color pressedColor = Color.red;

    void Start()
    {
        button = GetComponent<Button>();
        originalColor = button.image.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        button.image.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        button.image.color = originalColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        button.image.color = pressedColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        button.image.color = hoverColor;
    }
}
