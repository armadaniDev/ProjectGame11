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

    //variabel untuk mengaktifkan panel
    [SerializeField] public GameObject SelectedPanel;
    public bool isItemSelected;
    public InventoryManager inventoryManager;

    //Menampilkan detail panel
    public ItemDetails ItemDetails => itemDetails;
    private ItemDetails itemDetails;

    public void Start()
    {
        inventoryManager = FindAnyObjectByType<InventoryManager>();
    }

    public void AddItem(ItemDetails item)
    {
        itemDetails = item;
        itemName = item.ItemName;
        itemIcon = item.ItemIcon;
        AddQuantity(item.Quantity);
    }

    public bool IsEmpty => itemQuantity <= 0;

    public bool CanSatckWith(string otherItemName)
    {
        return !isFull && itemName == otherItemName;
    }

    public void AddQuantity(int amount)
    {
        itemQuantity = Mathf.Min(itemQuantity + amount, MaxStackSize);
        isFull = itemQuantity >= MaxStackSize;

        quantityText.text = itemQuantity.ToString();
        quantityText.enabled = true;
        itemImage.sprite = itemIcon;
        itemImage.enabled = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
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
        // inventoryManager.DeselectAllSlot();
        // SelectedPanel.SetActive(true);
        // isItemSelected = true;
        Debug.Log("Left Click");

        if (IsEmpty || inventoryManager == null)
        {
            return;
        }

        inventoryManager.SelectSlot(this);
    }
}