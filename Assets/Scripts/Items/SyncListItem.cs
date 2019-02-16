using UnityEngine.Networking;

public class SyncListItem : SyncList<Item> {

    public int value;

    protected override void SerializeItem(NetworkWriter writer, Item item) {
        writer.Write(ItemBase.GetItemId(item));
    }

    protected override Item DeserializeItem(NetworkReader reader) {
        return ItemBase.GetItem(reader.ReadInt32());
    }
}
