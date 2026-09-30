using System;
using System.Collections.Generic;
using PaperDollsGame.Content;

namespace PaperDollsGame.Outfit
{
    [Serializable]
    public sealed class OutfitItemData
    {
        public ItemSlot slot;
        public string itemId;
    }

    [Serializable]
    public sealed class OutfitSelection
    {
        public List<OutfitItemData> items = new List<OutfitItemData>();
    }
}
