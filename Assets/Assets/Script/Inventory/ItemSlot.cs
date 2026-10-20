using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    // Item Data
    public string itemName;
    public int itemQuantity;
    public Sprite itemIcon;
    public bool isFull;

    // Item Slot
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image itemImage;
    [SerializeField] private int MaxStackSize = 99;

    // Interactive item slot
    public GameObject Item;
    public bool isItemSelected;
    private InventoryManager inventoryManager;

    public void Awake()
    {
        inventoryManager = FindAnyObjectByType<InventoryManager>();
    }

    public void AddItem(ItemDetails item)
    {
        itemName = item.ItemName;
        itemIcon = item.ItemIcon;
        AddQuantity(item.Quantity);
    }

    public bool CanSatckWith(string otherItemName)
    {
        return !isFull && itemName == otherItemName;
    }

    public void AddQuantity(int amount)
    {
        itemQuantity += amount;
        isFull = itemQuantity >= MaxStackSize;

        quantityText.text = itemQuantity.ToString();
        quantityText.enabled = true;
        itemImage.sprite = itemIcon;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Slot kosong tidak bisa diklik
        if (itemQuantity == 0) return;

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            RightClick();
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            LeftClick();
        }
    }

    public void RightClick()
    {
        Debug.Log("Right Click");
    }

    public void LeftClick()
    {
        Debug.Log("Left Click");
    }
}