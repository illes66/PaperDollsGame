using System;
using PaperDollsGame.Content;
using PaperDollsGame.Inventory;
using PaperDollsGame.Saving;

namespace PaperDollsGame.Outfit
{
    public sealed class OutfitService
    {
        private readonly ContentCatalog catalog;
        private readonly InventoryService inventory;
        private readonly SaveService saveService;
        private readonly PlayerSaveData saveData;
        private readonly OutfitSelection selection = new OutfitSelection();

        public OutfitService(
            ContentCatalog catalog,
            InventoryService inventory,
            SaveService saveService,
            PlayerSaveData saveData)
        {
            this.catalog = catalog != null ? catalog : throw new ArgumentNullException("catalog");
            this.inventory = inventory ?? throw new ArgumentNullException("inventory");
            this.saveService = saveService ?? throw new ArgumentNullException("saveService");
            this.saveData = saveData ?? throw new ArgumentNullException("saveData");
            RestoreSavedOutfit();
        }

        public OutfitSelection Current
        {
            get
            {
                OutfitSelection copy = new OutfitSelection();
                for (int i = 0; i < selection.items.Count; i++)
                {
                    OutfitItemData item = selection.items[i];
                    copy.items.Add(new OutfitItemData { slot = item.slot, itemId = item.itemId });
                }
                return copy;
            }
        }

        public void Equip(string itemId)
        {
            if (!inventory.CanEquip(itemId))
                throw new InvalidOperationException("The item is not owned or is not in the content catalog.");

            ItemDefinition definition = catalog.GetItem(itemId);
            for (int i = selection.items.Count - 1; i >= 0; i--)
            {
                if (selection.items[i].slot == definition.Slot)
                    selection.items.RemoveAt(i);
            }
            selection.items.Add(new OutfitItemData { slot = definition.Slot, itemId = itemId });
            SaveOutfit();
        }

        public void Remove(ItemSlot slot)
        {
            selection.items.RemoveAll(item => item.slot == slot);
            SaveOutfit();
        }

        private void RestoreSavedOutfit()
        {
            for (int i = 0; i < saveData.equippedItems.Count; i++)
            {
                OutfitItemData saved = saveData.equippedItems[i];
                ItemDefinition definition;
                if (saved != null
                    && inventory.CanEquip(saved.itemId)
                    && catalog.TryGetItem(saved.itemId, out definition)
                    && definition.Slot == saved.slot)
                {
                    selection.items.Add(new OutfitItemData { slot = saved.slot, itemId = saved.itemId });
                }
            }
        }

        private void SaveOutfit()
        {
            saveData.equippedItems.Clear();
            for (int i = 0; i < selection.items.Count; i++)
            {
                OutfitItemData item = selection.items[i];
                saveData.equippedItems.Add(new OutfitItemData { slot = item.slot, itemId = item.itemId });
            }
            saveService.Save(saveData);
        }
    }
}
