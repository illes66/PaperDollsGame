using System;
using System.Collections.Generic;
using UnityEngine;

namespace PaperDollsGame.Content
{
    [CreateAssetMenu(menuName = "Paper Dolls/Content Catalog")]
    public sealed class ContentCatalog : ScriptableObject
    {
        [SerializeField] private List<ItemDefinition> items = new List<ItemDefinition>();
        [SerializeField] private List<EventDefinition> events = new List<EventDefinition>();

        private Dictionary<string, ItemDefinition> itemsById;
        private Dictionary<string, EventDefinition> eventsById;

        public IReadOnlyList<ItemDefinition> Items { get { return items; } }
        public IReadOnlyList<EventDefinition> Events { get { return events; } }

        public ItemDefinition GetItem(string id)
        {
            EnsureLookups();
            ItemDefinition definition;
            if (string.IsNullOrEmpty(id) || !itemsById.TryGetValue(id, out definition))
                throw new KeyNotFoundException("No item definition exists for ID '" + id + "'.");
            return definition;
        }

        public bool TryGetItem(string id, out ItemDefinition definition)
        {
            EnsureLookups();
            return !string.IsNullOrEmpty(id) && itemsById.TryGetValue(id, out definition);
        }

        public EventDefinition GetEvent(string id)
        {
            EnsureLookups();
            EventDefinition definition;
            if (string.IsNullOrEmpty(id) || !eventsById.TryGetValue(id, out definition))
                throw new KeyNotFoundException("No event definition exists for ID '" + id + "'.");
            return definition;
        }

        private void EnsureLookups()
        {
            if (itemsById != null && eventsById != null)
                return;

            itemsById = new Dictionary<string, ItemDefinition>(StringComparer.Ordinal);
            eventsById = new Dictionary<string, EventDefinition>(StringComparer.Ordinal);
            AddDefinitions(items, itemsById, "item");
            AddDefinitions(events, eventsById, "event");
        }

        private static void AddDefinitions<T>(IList<T> definitions, IDictionary<string, T> lookup, string kind)
            where T : ScriptableObject
        {
            for (int i = 0; i < definitions.Count; i++)
            {
                T definition = definitions[i];
                if (definition == null)
                    throw new InvalidOperationException("The content catalog contains a null " + kind + " definition.");

                string id = definition is ItemDefinition
                    ? ((ItemDefinition)(ScriptableObject)definition).Id
                    : ((EventDefinition)(ScriptableObject)definition).Id;
                if (string.IsNullOrWhiteSpace(id))
                    throw new InvalidOperationException("A " + kind + " definition has an empty ID.");
                if (lookup.ContainsKey(id))
                    throw new InvalidOperationException("Duplicate " + kind + " ID '" + id + "'.");
                lookup.Add(id, definition);
            }
        }

        private void OnEnable()
        {
            itemsById = null;
            eventsById = null;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            itemsById = null;
            eventsById = null;
        }
#endif
    }
}
