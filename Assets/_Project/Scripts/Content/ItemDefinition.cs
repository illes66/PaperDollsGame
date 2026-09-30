using System;
using System.Collections.Generic;
using UnityEngine;

namespace PaperDollsGame.Content
{
    public enum ItemSlot
    {
        Dress,
        Top,
        Bottom,
        Shoes,
        Hair,
        Accessory
    }

    [CreateAssetMenu(menuName = "Paper Dolls/Item Definition")]
    public sealed class ItemDefinition : ScriptableObject
    {
        [SerializeField] private string id;
        [SerializeField] private string displayName;
        [SerializeField] private ItemSlot slot;
        [SerializeField] private List<string> tags = new List<string>();
        [SerializeField] private Sprite icon;
        [SerializeField] private GameObject visualPrefab;

        public string Id { get { return id; } }
        public string DisplayName { get { return displayName; } }
        public ItemSlot Slot { get { return slot; } }
        public IReadOnlyList<string> Tags { get { return tags; } }
        public Sprite Icon { get { return icon; } }
        public GameObject VisualPrefab { get { return visualPrefab; } }
    }
}
