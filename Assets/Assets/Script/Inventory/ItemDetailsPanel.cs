using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDetailsPanel : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemDescriptionText;
    [SerializeField] private TMP_Text itemQtyText;

    public void Show(ItemDetails item, int Quantity)
    {
        if (item == null)
        {
            Clear();
            return;
        }
        
        itemImage.sprite = item.ItemIcon;
        itemImage.enabled = true;
        itemNameText.text = item.ItemName;
        itemDescriptionText.text = item.Description;
        itemQtyText.text = "Quantity: " + Quantity;
    }

    public void Clear()
    {
        itemImage.sprite = null;
        itemImage.enabled = false;
        itemNameText.text = "";
        itemDescriptionText.text = "";
        itemQtyText.text = "";
    }
}
