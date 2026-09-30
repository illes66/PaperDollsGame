using System;
using System.Collections.Generic;
using PaperDollsGame.Content;
using PaperDollsGame.Outfit;

namespace PaperDollsGame.Saving
{
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
        public List<OutfitItemData> equippedItems = new List<OutfitItemData>();
        public List<CurrencyBalance> currencies = new List<CurrencyBalance>();
    }
}
