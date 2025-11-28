using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlotUI : MonoBehaviour
{
    public Image iconImage;
    public TMP_Text amountText;
    public Image background;

    [HideInInspector] public int index; // индекс слота в InventoryManager

    public void Set(InventorySlot slot, ItemData itemData)
    {
        if (slot == null || slot.IsEmpty || itemData == null)
        {
            iconImage.enabled = false;
            amountText.text = "";
            background.color = new Color(1, 1, 1, 0.2f); // полу-прозрачный
            return;
        }

        iconImage.enabled = true;
        iconImage.sprite = itemData.icon;
        amountText.text = slot.amount > 1 ? slot.amount.ToString() : "";
        background.color = Color.white;
    }
}
