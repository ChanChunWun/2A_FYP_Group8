using System.Collections.Generic;
using UnityEngine;

public class InventoryController : Singleton<InventoryController>
{
    [HideInInspector]
    private ItemGrid selectedItemGrid;
    public ItemGrid SelectedItemGrid
    {
        get => selectedItemGrid;
        set
        {
            selectedItemGrid = value;
            inventoryHighlight.SetParent(value);
        }
    }

    public InventoryItem SelectedItem { get; private set; }
    RectTransform selectedRect;
    InventoryItem overlapItem;

    [SerializeField] List<ItemData> items;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Transform canvasTransform;

    InventoryHighlight inventoryHighlight;

    protected override void Awake()
    {
        base.Awake();
        inventoryHighlight = GetComponent<InventoryHighlight>();
    }

    private void Update()
    {
        ItemIconDrag();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            CreateRandomItem();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            InsertRandomItem();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateItem();
        }

        if (selectedItemGrid == null) 
        {
            inventoryHighlight.Show(false);
            return; 
        }

        HandleHighlight();

        if (Input.GetMouseButtonDown(0))
        {
            LeftMouseButtonPress();
        }
    }

    private void RotateItem()
        => SelectedItem?.Rotate();

    private void InsertRandomItem()
    {
        if (selectedItemGrid == null) { return; }

        CreateRandomItem();
        InventoryItem itemToInsert = SelectedItem;
        SelectedItem = null;
        if (!InsertItem(itemToInsert))
            Destroy(itemToInsert.gameObject);
    }

    private bool InsertItem(InventoryItem itemToInsert)
    {
        Vector2Int? posOnGrid = selectedItemGrid.FindSpaceForObject(itemToInsert);
        if (posOnGrid == null)
            return false;

        selectedItemGrid.PlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
        return true;
    }

    Vector2Int oldPosition;
    public InventoryItem itemToHighlight { get; private set; }

    private void HandleHighlight()
    {
        Vector2Int positionOnGrid = GetTileGridPosition();
        if (oldPosition == positionOnGrid) { return; }

        oldPosition = positionOnGrid;
        if (SelectedItem == null)
        {
            itemToHighlight = selectedItemGrid.GetItem(positionOnGrid.x, positionOnGrid.y);

            if(itemToHighlight != null)
            {
                inventoryHighlight.Show(true);
                inventoryHighlight.SetSize(itemToHighlight);
                inventoryHighlight.SetPosition(selectedItemGrid, itemToHighlight);
            }
            else
            {
                inventoryHighlight.Show(false);
            }
        }
        else
        {
            inventoryHighlight.Show(selectedItemGrid.BoundryCheck(
                positionOnGrid.x, 
                positionOnGrid.y,
                SelectedItem.WIDTH,
                SelectedItem.HEIGHT)
                );
            inventoryHighlight.SetSize(SelectedItem);
            inventoryHighlight.SetPosition(selectedItemGrid, SelectedItem, positionOnGrid.x, positionOnGrid.y);
        }
    }

    private void CreateRandomItem()
    {
        InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        SelectedItem = inventoryItem;

        selectedRect = inventoryItem.GetComponent<RectTransform>();
        selectedRect.SetParent(canvasTransform);
        selectedRect.SetAsLastSibling();

        int selectedItemID = UnityEngine.Random.Range(0, items.Count);
        inventoryItem.Set(items[selectedItemID]);

    }

    public void AddItems(List<InventoryItem> InItems)
    {
        for (int i = 0; i < InItems.Count; i++)
        {
            selectedRect = InItems[i].GetComponent<RectTransform>();
            selectedRect.SetParent(canvasTransform);
            selectedRect.SetAsLastSibling();
            int selectedItemID = UnityEngine.Random.Range(0, items.Count);
            InItems[i].Set(items[selectedItemID]);

        }

    }

    private void LeftMouseButtonPress()
    {
        Vector2Int tileGridPosition = GetTileGridPosition();

        if (SelectedItem == null)
        {
            PickUpItem(tileGridPosition);
        }
        else
        {
            PlaceItem(tileGridPosition);
        }
    }

    private Vector2Int GetTileGridPosition()
    {
        Vector2 position = Input.mousePosition;

        if (SelectedItem != null)
        {
            position.x -= (SelectedItem.WIDTH - 1) * ItemGrid.tileSizeWidth / 2;
            position.y += (SelectedItem.HEIGHT - 1) * ItemGrid.tileSizeHeight / 2;
        }

        return selectedItemGrid.GetTileGridPosition(position); 
    }

    private void PlaceItem(Vector2Int tileGridPosition)
    {
        bool complete = selectedItemGrid.PlaceItem(SelectedItem, tileGridPosition.x, tileGridPosition.y, ref overlapItem);
        if (complete)
        {
            SelectedItem = null;

            if(overlapItem != null)
            {
                SelectedItem = overlapItem;
                overlapItem = null;
                selectedRect = SelectedItem.GetComponent<RectTransform>();
                selectedRect.SetAsLastSibling();
            }
        } 
    }

    private void PickUpItem(Vector2Int tileGridPosition)
    {
        SelectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);

        if (SelectedItem != null)
        {
            selectedRect = SelectedItem.GetComponent<RectTransform>();
        }
    }

    private void ItemIconDrag()
    {
        if (SelectedItem != null)
        {
            selectedRect.position = Input.mousePosition;
        }
    }
}
