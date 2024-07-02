using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Singleton;
using GBGamesPlugin;
using UnityEngine;
using VInspector;

namespace _3._Scripts.Stages
{
    [ExecuteInEditMode]
    public class StageController : Singleton<StageController>
    {
        [SerializeField] private List<Stage> stages = new();
        
        public Stage CurrentStage { get; private set; }
        private int _activeStageID;

        public int CurrentID { get; private set; }
        private void Start()
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
                TeleportToStage(GBGames.saves.stageID);
#else
             TeleportToStage(GBGames.saves.stageID);
#endif
        }

        public void TeleportToNextStage()
        {
            CurrentID += 1;
            if (CurrentID > GBGames.saves.stageID)
                GBGames.saves.stageID = CurrentID;
            TeleportToStage(CurrentID);
            GBGames.instance.Save();
        }

        public void TeleportToPreviousStage()
        {
            CurrentID -= 1;
            TeleportToStage(CurrentID);
        }

        private void TeleportToStage(int id)
        {
            var stage = stages.FirstOrDefault(s => s.ID == id);
            
            if (stage == null) return;
            
            CurrentStage = stage;
            CurrentID = id;

            foreach (var s in stages)
            {
                s.gameObject.SetActive(false);
            }
            
            var spawnPoint = stage.SpawnPoint.position;

            stage.gameObject.SetActive(true);
            stage.Initialize();
            
            Player.Player.instance.Teleport(spawnPoint);
        }

#if UNITY_EDITOR

        [Button]
        private void NextStage()
        {
            _activeStageID = Math.Clamp(_activeStageID + 1, 0, stages.Count - 1);

            foreach (var stage in stages)
            {
                var active = stage.ID == _activeStageID;
                stage.gameObject.SetActive(active);
            }
        }

        [Button]
        private void PreviousStage()
        {
            _activeStageID = Math.Clamp(_activeStageID - 1, 0, stages.Count - 1);
            foreach (var stage in stages)
            {
                var active = stage.ID == _activeStageID;
                stage.gameObject.SetActive(active);
            }
        }
#endif
    }
}