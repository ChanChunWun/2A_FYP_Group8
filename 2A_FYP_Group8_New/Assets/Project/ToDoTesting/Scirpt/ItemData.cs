using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "FYP/Inventory/ItemData")]
public class ItemData : ScriptableObject
{
    public int ID;

    public int width = 1;
    public int height = 1;
    public int price;

    public Sprite itemIcon;
}
