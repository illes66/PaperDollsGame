using System;
using PaperDollsGame.Saving;

namespace PaperDollsGame.Economy
{
    public sealed class EconomyService
    {
        private readonly PlayerSaveData saveData;

        public EconomyService(PlayerSaveData saveData)
        {
            this.saveData = saveData ?? throw new ArgumentNullException("saveData");
        }

        public int GetBalance(string currencyId)
        {
            CurrencyBalance balance = FindBalance(currencyId);
            return balance == null ? 0 : balance.amount;
        }

        public void Grant(string currencyId, int amount)
        {
            if (string.IsNullOrWhiteSpace(currencyId))
                throw new ArgumentException("A currency ID is required.", "currencyId");
            if (amount < 0)
                throw new ArgumentOutOfRangeException("amount");

            CurrencyBalance balance = FindBalance(currencyId);
            if (balance == null)
            {
                balance = new CurrencyBalance { currencyId = currencyId };
                saveData.currencies.Add(balance);
            }
            checked { balance.amount += amount; }
        }

        public bool TrySpend(string currencyId, int amount)
        {
            if (string.IsNullOrWhiteSpace(currencyId))
                throw new ArgumentException("A currency ID is required.", "currencyId");
            if (amount < 0)
                throw new ArgumentOutOfRangeException("amount");

            CurrencyBalance balance = FindBalance(currencyId);
            if (balance == null || balance.amount < amount)
                return false;
            balance.amount -= amount;
            return true;
        }

        private CurrencyBalance FindBalance(string currencyId)
        {
            for (int i = 0; i < saveData.currencies.Count; i++)
            {
                CurrencyBalance balance = saveData.currencies[i];
                if (balance != null && string.Equals(balance.currencyId, currencyId, StringComparison.Ordinal))
                    return balance;
            }
            return null;
        }
    }
}
