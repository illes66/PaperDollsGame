using System;
using System.Collections.Generic;
using PaperDollsGame.Content;

namespace PaperDollsGame.Saving
{
    [Serializable]
    public sealed class EquippedItemData
    {
        public ItemSlot slot;
        public string itemId;
    }

    [Serializable]
    public sealed class CurrencyBalance
    {
        public string currencyId;
        public int amount;
    }

    [Serializable]
    public sealed class PlayerSaveData
    {
        public int schemaVersion = SaveService.CurrentSchemaVersion;
        public List<string> ownedItemIds = new List<string>();
        public List<EquippedItemData> equippedItems = new List<EquippedItemData>();
        public List<CurrencyBalance> currencies = new List<CurrencyBalance>();
    }
}
