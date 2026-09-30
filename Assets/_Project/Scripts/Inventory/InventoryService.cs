using System;
using PaperDollsGame.Content;
using PaperDollsGame.Saving;

namespace PaperDollsGame.Inventory
{
    public sealed class InventoryService
    {
        private readonly ContentCatalog catalog;
        private readonly PlayerSaveData saveData;

        public InventoryService(ContentCatalog catalog, PlayerSaveData saveData)
        {
            this.catalog = catalog != null ? catalog : throw new ArgumentNullException("catalog");
            this.saveData = saveData ?? throw new ArgumentNullException("saveData");
        }

        public bool IsOwned(string itemId)
        {
            return saveData.ownedItemIds.Contains(itemId);
        }

        public bool CanEquip(string itemId)
        {
            ItemDefinition definition;
            return IsOwned(itemId) && catalog.TryGetItem(itemId, out definition);
        }

        public void Acquire(string itemId)
        {
            catalog.GetItem(itemId);
            if (!IsOwned(itemId))
                saveData.ownedItemIds.Add(itemId);
        }
    }
}
