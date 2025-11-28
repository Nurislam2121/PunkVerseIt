[System.Serializable]
public class InventorySlot
{
    public string itemId;
    public int amount;

    public InventorySlot() { itemId = ""; amount = 0; }
    public InventorySlot(string id, int qty) { itemId = id; amount = qty; }
    public bool IsEmpty => string.IsNullOrEmpty(itemId) || amount <= 0;
}
