using System;
using System.Collections.Generic;
using PaperDollsGame.Content;
using PaperDollsGame.Economy;
using PaperDollsGame.Flow;
using PaperDollsGame.Inventory;
using PaperDollsGame.Outfit;
using PaperDollsGame.Saving;
using PaperDollsGame.Scoring;
using UnityEngine;

namespace PaperDollsGame.Bootstrap
{
    public sealed class GameBootstrap : MonoBehaviour
    {
        [SerializeField] private ContentCatalog contentCatalog;
        [SerializeField] private List<string> starterItemIds = new List<string>();
        [SerializeField] private string startingCurrencyId = "GEMS";
        [SerializeField, Min(0)] private int startingCurrencyAmount;

        public GameFlowController Flow { get; private set; }
        public InventoryService Inventory { get; private set; }
        public OutfitService Outfit { get; private set; }
        public EconomyService Economy { get; private set; }

        private void Awake()
        {
            if (contentCatalog == null)
                throw new InvalidOperationException("Assign a ContentCatalog to GameBootstrap.");

            SaveService saveService = new SaveService(
                System.IO.Path.Combine(Application.persistentDataPath, "player-save.json"));
            PlayerSaveData saveData = saveService.LoadOrCreate(
                starterItemIds,
                startingCurrencyId,
                startingCurrencyAmount);

            Inventory = new InventoryService(contentCatalog, saveData);
            Outfit = new OutfitService(contentCatalog, Inventory, saveService, saveData);
            Economy = new EconomyService(saveData);
            ScoringService scoring = new ScoringService(contentCatalog);
            Flow = new GameFlowController(contentCatalog, Outfit, scoring, Economy, saveService, saveData);
        }
    }
}
