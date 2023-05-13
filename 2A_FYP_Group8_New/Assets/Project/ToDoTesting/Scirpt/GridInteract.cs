using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemGrid))]
public class GridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private ItemGrid itemGrid;

    private void Awake()
        => itemGrid = GetComponent<ItemGrid>();

    public void OnPointerEnter(PointerEventData eventData) { }

    public void OnPointerExit(PointerEventData eventData) { }
}
