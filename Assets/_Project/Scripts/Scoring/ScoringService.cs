using System;
using System.Collections.Generic;
using PaperDollsGame.Content;
using PaperDollsGame.Outfit;

namespace PaperDollsGame.Scoring
{
    [Serializable]
    public sealed class ScoreBreakdownEntry
    {
        public string description;
        public string itemId;
        public int points;
    }

    [Serializable]
    public sealed class ScoreResult
    {
        public int totalScore;
        public List<ScoreBreakdownEntry> breakdown = new List<ScoreBreakdownEntry>();
    }

    public sealed class ScoringService
    {
        private readonly ContentCatalog catalog;

        public ScoringService(ContentCatalog catalog)
        {
            this.catalog = catalog != null ? catalog : throw new ArgumentNullException("catalog");
        }

        public ScoreResult Calculate(EventDefinition eventDefinition, OutfitSelection outfit)
        {
            if (eventDefinition == null)
                throw new ArgumentNullException("eventDefinition");
            if (outfit == null)
                throw new ArgumentNullException("outfit");

            ScoreResult result = new ScoreResult();
            for (int itemIndex = 0; itemIndex < outfit.items.Count; itemIndex++)
            {
                OutfitItemData selected = outfit.items[itemIndex];
                ItemDefinition item = catalog.GetItem(selected.itemId);
                if (item.Slot != selected.slot)
                    throw new InvalidOperationException("Outfit item slot does not match its definition.");

                for (int ruleIndex = 0; ruleIndex < eventDefinition.TagScoring.Count; ruleIndex++)
                {
                    TagScoreRule rule = eventDefinition.TagScoring[ruleIndex];
                    if (HasTag(item, rule.Tag))
                        AddPoints(result, "Tag: " + rule.Tag, item.Id, rule.Points);
                }

                for (int ruleIndex = 0; ruleIndex < eventDefinition.SlotScoring.Count; ruleIndex++)
                {
                    SlotScoreRule rule = eventDefinition.SlotScoring[ruleIndex];
                    if (selected.slot == rule.Slot)
                        AddPoints(result, "Slot: " + rule.Slot, item.Id, rule.Points);
                }
            }
            return result;
        }

        private static bool HasTag(ItemDefinition item, string tag)
        {
            if (string.IsNullOrEmpty(tag))
                return false;
            for (int i = 0; i < item.Tags.Count; i++)
            {
                if (string.Equals(item.Tags[i], tag, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        private static void AddPoints(ScoreResult result, string description, string itemId, int points)
        {
            result.breakdown.Add(new ScoreBreakdownEntry
            {
                description = description,
                itemId = itemId,
                points = points
            });
            result.totalScore += points;
        }
    }
}
