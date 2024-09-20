using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GBGamesPlugin
{
    public partial class GBGames : MonoBehaviour
    {
        public static GBGames instance { get; private set; }
        public GBGamesSettings settings;
        private static bool _inGame;

        private void Awake()
        {
            StartCoroutine(Initialize());
        }

        private IEnumerator Initialize()
        {
            _inGame = true;
            yield return new WaitForSeconds(1);
            Singleton();
            Storage();
            yield return new WaitForSeconds(2);
            RemoteConfig();
            Advertisement();
            Game();
        }

        private void Singleton()
        {
            transform.SetParent(null);
            gameObject.name = "GBGames";

            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void Advertisement()
        {
            
        }
        
        private void Storage()
        {
            Load();
            if (instance.settings.autoSaveByInterval)
                StartCoroutine(IntervalSave());
        }

        private void Game()
        {
            if (instance.settings.saveOnChangeVisibilityState)
                GameHiddenStateCallback += Save;
        }

        private void RemoteConfig()
        {
            
        }
    }
}
