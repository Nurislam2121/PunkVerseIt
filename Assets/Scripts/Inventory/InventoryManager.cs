using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Tooltip("Количество слотов в инвентаре (UI должен соответствовать).")]
    public int capacity = 20;
    public List<InventorySlot> slots;

    // Справочник ItemData по itemId
    public List<ItemData> itemDatabase; // заполни через инспектор
    private Dictionary<string, ItemData> itemLookup;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        DontDestroyOnLoad(gameObject);
        slots = new List<InventorySlot>(capacity);
        for (int i = 0; i < capacity; i++) slots.Add(new InventorySlot());

        itemLookup = itemDatabase.ToDictionary(i => i.itemId, i => i);

        LoadInventory();
    }

    public ItemData GetItemData(string id)
    {
        if (itemLookup.TryGetValue(id, out var d)) return d;
        return null;
    }

    // Добавить предмет, вернуть true если удалось
    public bool AddItem(string itemId, int amount = 1)
    {
        var item = GetItemData(itemId);
        if (item == null) { Debug.LogWarning("Item not found: " + itemId); return false; }

        if (item.stackable)
        {
            // сначала попытаемся добавить в существующие стеки
            for (int i = 0; i < slots.Count; i++)
            {
                var s = slots[i];
                if (!s.IsEmpty && s.itemId == itemId && s.amount < item.maxStack)
                {
                    int space = item.maxStack - s.amount;
                    int toAdd = Mathf.Min(space, amount);
                    s.amount += toAdd;
                    amount -= toAdd;
                    if (amount <= 0) { OnInventoryChanged(); return true; }
                }
            }
        }

        // затем помещаем в пустые слоты
        for (int i = 0; i < slots.Count; i++)
        {
            var s = slots[i];
            if (s.IsEmpty)
            {
                int toPut = item.stackable ? Mathf.Min(item.maxStack, amount) : 1;
                s.itemId = itemId;
                s.amount = toPut;
                amount -= toPut;
                if (amount <= 0) { OnInventoryChanged(); return true; }
            }
        }

        // если остались не помещённые предметы — не хватает места
        OnInventoryChanged();
        return amount <= 0;
    }

    public bool RemoveItem(string itemId, int amount = 1)
    {
        for (int i = 0; i < slots.Count && amount > 0; i++)
        {
            var s = slots[i];
            if (!s.IsEmpty && s.itemId == itemId)
            {
                int take = Mathf.Min(s.amount, amount);
                s.amount -= take;
                amount -= take;
                if (s.amount <= 0) { s.itemId = ""; s.amount = 0; }
            }
        }
        OnInventoryChanged();
        return amount <= 0;
    }

    public void ClearInventory()
    {
        for (int i = 0; i < slots.Count; i++) slots[i] = new InventorySlot();
        OnInventoryChanged();
    }

    // Сохранение/загрузка (простая JSON в PlayerPrefs)
    [System.Serializable]
    private class SaveData
    {
        public List<InventorySlot> slots;
    }

    public void SaveInventory()
    {
        var sd = new SaveData { slots = slots };
        string json = JsonUtility.ToJson(sd);
        PlayerPrefs.SetString("inventory_save", json);
        PlayerPrefs.Save();
        Debug.Log("Inventory saved.");
    }

    public void LoadInventory()
    {
        if (!PlayerPrefs.HasKey("inventory_save")) return;
        string json = PlayerPrefs.GetString("inventory_save");
        var sd = JsonUtility.FromJson<SaveData>(json);
        if (sd != null && sd.slots != null)
        {
            // убедимся, что размер совпадает
            slots = new List<InventorySlot>(capacity);
            for (int i = 0; i < capacity; i++)
            {
                if (i < sd.slots.Count) slots.Add(sd.slots[i]);
                else slots.Add(new InventorySlot());
            }
        }
        OnInventoryChanged();
        Debug.Log("Inventory loaded.");
    }

    public bool HasItem(string itemId)
    {
        foreach (var s in slots)
        {
            if (!s.IsEmpty && s.itemId == itemId)
                return true;
        }
        return false;
    }


    public int GetItemCount(string itemId)
    {
        int count = 0;
        foreach (var s in slots)
        {
            if (!s.IsEmpty && s.itemId == itemId)
                count += s.amount;
        }
        return count;
    }


    // Событие об изменении (UI подписывается)
    public System.Action onInventoryChanged;
    private void OnInventoryChanged()
    {
        onInventoryChanged?.Invoke();
    }
}
