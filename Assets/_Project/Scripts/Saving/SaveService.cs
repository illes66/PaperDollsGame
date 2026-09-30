using System;
using System.Collections.Generic;
using System.IO;
using PaperDollsGame.Content;
using PaperDollsGame.Outfit;
using UnityEngine;

namespace PaperDollsGame.Saving
{
    public sealed class SaveService
    {
        public const int CurrentSchemaVersion = 1;

        private readonly string savePath;

        public SaveService(string savePath)
        {
            if (string.IsNullOrWhiteSpace(savePath))
                throw new ArgumentException("A save path is required.", "savePath");
            this.savePath = savePath;
        }

        public PlayerSaveData LoadOrCreate(
            IEnumerable<string> starterItemIds,
            string startingCurrencyId,
            int startingCurrencyAmount)
        {
            RecoverInterruptedSave();
            if (File.Exists(savePath))
            {
                PlayerSaveData loaded = JsonUtility.FromJson<PlayerSaveData>(File.ReadAllText(savePath));
                if (loaded == null)
                    throw new InvalidDataException("The player save file is empty or invalid.");
                if (loaded.schemaVersion != CurrentSchemaVersion)
                    throw new InvalidDataException("Unsupported player save schema version " + loaded.schemaVersion + ".");
                Normalize(loaded);
                return loaded;
            }

            if (startingCurrencyAmount < 0)
                throw new ArgumentOutOfRangeException("startingCurrencyAmount");
            if (string.IsNullOrWhiteSpace(startingCurrencyId))
                throw new ArgumentException("A starting currency ID is required.", "startingCurrencyId");

            PlayerSaveData newSave = new PlayerSaveData();
            if (starterItemIds != null)
            {
                foreach (string itemId in starterItemIds)
                {
                    if (!string.IsNullOrWhiteSpace(itemId) && !newSave.ownedItemIds.Contains(itemId))
                        newSave.ownedItemIds.Add(itemId);
                }
            }
            newSave.currencies.Add(new CurrencyBalance
            {
                currencyId = startingCurrencyId,
                amount = startingCurrencyAmount
            });
            Save(newSave);
            return newSave;
        }

        public void Save(PlayerSaveData data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            Normalize(data);
            data.schemaVersion = CurrentSchemaVersion;

            string directory = Path.GetDirectoryName(savePath);
            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);

            string temporaryPath = savePath + ".tmp";
            string backupPath = savePath + ".bak";
            try
            {
                File.WriteAllText(temporaryPath, JsonUtility.ToJson(data, true));
                if (File.Exists(savePath))
                {
                    if (File.Exists(backupPath))
                        File.Delete(backupPath);
                    File.Move(savePath, backupPath);
                    try
                    {
                        File.Move(temporaryPath, savePath);
                        File.Delete(backupPath);
                    }
                    catch
                    {
                        if (!File.Exists(savePath) && File.Exists(backupPath))
                            File.Move(backupPath, savePath);
                        throw;
                    }
                }
                else
                {
                    File.Move(temporaryPath, savePath);
                }
            }
            finally
            {
                if (File.Exists(temporaryPath))
                    File.Delete(temporaryPath);
            }
        }

        private void RecoverInterruptedSave()
        {
            string backupPath = savePath + ".bak";
            if (!File.Exists(savePath) && File.Exists(backupPath))
                File.Move(backupPath, savePath);
            else if (File.Exists(savePath) && File.Exists(backupPath))
                File.Delete(backupPath);

            string temporaryPath = savePath + ".tmp";
            if (File.Exists(temporaryPath))
                File.Delete(temporaryPath);
        }

        private static void Normalize(PlayerSaveData data)
        {
            if (data.ownedItemIds == null)
                data.ownedItemIds = new List<string>();
            if (data.equippedItems == null)
                data.equippedItems = new List<OutfitItemData>();
            if (data.currencies == null)
                data.currencies = new List<CurrencyBalance>();
        }
    }
}
