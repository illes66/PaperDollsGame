using System;
using PaperDollsGame.Content;
using PaperDollsGame.Economy;
using PaperDollsGame.Outfit;
using PaperDollsGame.Saving;
using PaperDollsGame.Scoring;

namespace PaperDollsGame.Flow
{
    public enum GamePhase
    {
        SelectEvent,
        Dressing,
        Results
    }

    public sealed class GameFlowController
    {
        private readonly ContentCatalog catalog;
        private readonly OutfitService outfitService;
        private readonly ScoringService scoringService;
        private readonly EconomyService economyService;
        private readonly SaveService saveService;
        private readonly PlayerSaveData saveData;
        private EventDefinition currentEvent;

        public GamePhase Phase { get; private set; }
        public ScoreResult LastResult { get; private set; }

        public GameFlowController(
            ContentCatalog catalog,
            OutfitService outfitService,
            ScoringService scoringService,
            EconomyService economyService,
            SaveService saveService,
            PlayerSaveData saveData)
        {
            this.catalog = catalog != null ? catalog : throw new ArgumentNullException("catalog");
            this.outfitService = outfitService ?? throw new ArgumentNullException("outfitService");
            this.scoringService = scoringService ?? throw new ArgumentNullException("scoringService");
            this.economyService = economyService ?? throw new ArgumentNullException("economyService");
            this.saveService = saveService ?? throw new ArgumentNullException("saveService");
            this.saveData = saveData ?? throw new ArgumentNullException("saveData");
            Phase = GamePhase.SelectEvent;
        }

        public void StartEvent(string eventId)
        {
            currentEvent = catalog.GetEvent(eventId);
            LastResult = null;
            Phase = GamePhase.Dressing;
        }

        public void SubmitOutfit()
        {
            EnsureDressing();
            LastResult = scoringService.Calculate(currentEvent, outfitService.Current);
            economyService.Grant(currentEvent.RewardCurrencyId, currentEvent.RewardAmount);
            saveService.Save(saveData);
            Phase = GamePhase.Results;
        }

        public void ContinueToEventSelection()
        {
            if (Phase != GamePhase.Results)
                throw new InvalidOperationException("The player can continue only from the results phase.");
            currentEvent = null;
            Phase = GamePhase.SelectEvent;
        }

        private void EnsureDressing()
        {
            if (Phase != GamePhase.Dressing || currentEvent == null)
                throw new InvalidOperationException("An event must be started before submitting an outfit.");
        }
    }
}
