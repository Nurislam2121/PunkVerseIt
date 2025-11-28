using UnityEngine;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    [Tooltip("Ручной список слотов из сцены")]
    public List<ItemSlotUI> uiSlots;

    private void Start()
    {
        InventoryManager.Instance.onInventoryChanged += RefreshUI;
        RefreshUI();
    }

    public void RefreshUI()
    {
        var slots = InventoryManager.Instance.slots;

        for (int i = 0; i < uiSlots.Count; i++)
        {
            if (i >= slots.Count) return; // если UI слотов больше чем данных

            var s = slots[i];
            var itemData = string.IsNullOrEmpty(s.itemId) ? null :
                InventoryManager.Instance.GetItemData(s.itemId);

            uiSlots[i].Set(s, itemData);
        }
    }

    private void OnDestroy()
    {
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.onInventoryChanged -= RefreshUI;
    }
}
