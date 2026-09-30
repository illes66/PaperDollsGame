using System;
using System.Collections.Generic;
using UnityEngine;

namespace PaperDollsGame.Content
{
    [Serializable]
    public sealed class TagScoreRule
    {
        [SerializeField] private string tag;
        [SerializeField] private int points;

        public string Tag { get { return tag; } }
        public int Points { get { return points; } }
    }

    [Serializable]
    public sealed class SlotScoreRule
    {
        [SerializeField] private ItemSlot slot;
        [SerializeField] private int points;

        public ItemSlot Slot { get { return slot; } }
        public int Points { get { return points; } }
    }

    [CreateAssetMenu(menuName = "Paper Dolls/Event Definition")]
    public sealed class EventDefinition : ScriptableObject
    {
        [SerializeField] private string id;
        [SerializeField] private string displayName;
        [TextArea, SerializeField] private string prompt;
        [SerializeField] private List<TagScoreRule> tagScoring = new List<TagScoreRule>();
        [SerializeField] private List<SlotScoreRule> slotScoring = new List<SlotScoreRule>();
        [SerializeField] private string rewardCurrencyId = "GEMS";
        [SerializeField, Min(0)] private int rewardAmount;

        public string Id { get { return id; } }
        public string DisplayName { get { return displayName; } }
        public string Prompt { get { return prompt; } }
        public IReadOnlyList<TagScoreRule> TagScoring { get { return tagScoring; } }
        public IReadOnlyList<SlotScoreRule> SlotScoring { get { return slotScoring; } }
        public string RewardCurrencyId { get { return rewardCurrencyId; } }
        public int RewardAmount { get { return rewardAmount; } }
    }
}
