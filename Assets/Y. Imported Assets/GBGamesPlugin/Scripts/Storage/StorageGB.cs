using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;

namespace GBGamesPlugin
{
    public partial class GBGames
    {
        public static SavesGB saves = new();
        public static event Action SaveLoadedCallback;
        private const string SavesID = "saves";


        #region Load

        ///<summary>
        /// Загрузка данных.
        /// </summary>
        private void Load()
        {
            var data = PlayerPrefs.GetString(SavesID);
            saves = string.IsNullOrEmpty(data) ? new SavesGB() : JsonConvert.DeserializeObject<SavesGB>(data);
            SaveLoadedCallback?.Invoke();
            Message($"Save loaded - {data}");
        }

        #endregion

        #region Save

        ///<summary>
        /// Сохранение данных.
        /// </summary>
        public void Save()
        {
            saves.saveID++;
            var data = JsonConvert.SerializeObject(saves);
            PlayerPrefs.SetString(SavesID, data);
            Message($"Save saves - {data}");
        }

        private IEnumerator IntervalSave()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(60 * instance.settings.saveInterval);
                Save();
            }
        }

        #endregion

        #region Delete

        public static void Delete()
        {
            var id = saves.saveID;
            saves = new SavesGB {saveID = id};
            instance.Save();
        }

        #endregion
    }
}