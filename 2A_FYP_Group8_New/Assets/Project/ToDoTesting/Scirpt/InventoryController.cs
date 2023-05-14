using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryController : Singleton<InventoryController>
{
    [SerializeField]
    private ItemGrid itemGrid;

    public InventoryItem SelectedItem { get; private set; }
    RectTransform selectedRect;
    InventoryItem overlapItem;
    public CanvasGroup cg;
    [SerializeField] List<ItemData> items;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Transform canvasTransform;

    InventoryHighlight inventoryHighlight;

    protected override void Awake()
    {
        base.Awake();
        ScenceManager.DontDestroyOnLoad(gameObject);
        inventoryHighlight = GetComponent<InventoryHighlight>();
        inventoryHighlight.SetParent(itemGrid);
        cg = transform.GetChild(0).GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (cg.alpha == 0)
            return;

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

        var eventSystem = EventSystem.current;
        if (eventSystem == null || !eventSystem.IsPointerOverGameObject())
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

    
    private bool InsertItem(InventoryItem itemToInsert)
    {
        Vector2Int? posOnGrid = itemGrid.FindSpaceForObject(itemToInsert);
        if (posOnGrid == null)
            return false;

        itemGrid.PlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
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
            itemToHighlight = itemGrid.GetItem(positionOnGrid.x, positionOnGrid.y);

            if(itemToHighlight != null)
            {
                inventoryHighlight.Show(true);
                inventoryHighlight.SetSize(itemToHighlight);
                inventoryHighlight.SetPosition(itemGrid, itemToHighlight);
            }
            else
            {
                inventoryHighlight.Show(false);
            }
        }
        else
        {
            inventoryHighlight.Show(itemGrid.BoundryCheck(
                positionOnGrid.x, 
                positionOnGrid.y,
                SelectedItem.WIDTH,
                SelectedItem.HEIGHT)
                );
            inventoryHighlight.SetSize(SelectedItem);
            inventoryHighlight.SetPosition(itemGrid, SelectedItem, positionOnGrid.x, positionOnGrid.y);
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
    private void InsertRandomItem()
    {
        if (itemGrid == null) { return; }

        CreateRandomItem();
        InventoryItem itemToInsert = SelectedItem;
        SelectedItem = null;
        if (!InsertItem(itemToInsert))
            Destroy(itemToInsert.gameObject);
    }

    public void AddItems(List<ItemData> InItems)
    {
        foreach (var item in InItems)
        {
            InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();
            inventoryItem.Set(item);
            if (!InsertItem(inventoryItem))
                Destroy(inventoryItem.gameObject);
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

        return itemGrid.GetTileGridPosition(position); 
    }

    private void PlaceItem(Vector2Int tileGridPosition)
    {
        bool complete = itemGrid.PlaceItem(SelectedItem, tileGridPosition.x, tileGridPosition.y, ref overlapItem);
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
        SelectedItem = itemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);

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
