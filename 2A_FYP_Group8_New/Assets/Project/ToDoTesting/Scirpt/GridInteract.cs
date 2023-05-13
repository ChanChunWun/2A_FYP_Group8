using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemGrid))]
public class GridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private InventoryController inventoryController;

    private ItemGrid itemGrid;

    private void Awake()
        => itemGrid = GetComponent<ItemGrid>();

    public void OnPointerEnter(PointerEventData eventData)
        => inventoryController.SelectedItemGrid = itemGrid;

    public void OnPointerExit(PointerEventData eventData)
        => inventoryController.SelectedItemGrid = null;
}
