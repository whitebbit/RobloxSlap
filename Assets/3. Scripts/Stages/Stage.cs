using System;
using System.Collections.Generic;
using System.Linq;
using _3._Scripts.Actions;
using _3._Scripts.Pets;
using _3._Scripts.Stages.Enums;
using _3._Scripts.Stages.Scriptable;
using UnityEngine;
using VInspector;

namespace _3._Scripts.Stages
{
    public class Stage : MonoBehaviour
    {
        [Header("Main")] [SerializeField] private StageConfig config;
        [SerializeField] private Transform spawnPoint;

        public Transform SpawnPoint => spawnPoint;
        public float GiftBooster => config.GiftBooster;
        public int ID => config.ID;

        public void Initialize()
        {
            InitializeEnemy();
            InitializePetUnlocker();
            InitializeTraining();
            InitializeTeleport();
        }

        private void InitializeTeleport()
        {
            var obj = GetComponentsInChildren<StagePortal>().FirstOrDefault(s => s.Type == TeleportType.Next);
            if(obj != null)
                obj.SetPrice(config.TeleportPrice);
        }
        
        private void InitializeEnemy()
        {
            var obj = GetComponentsInChildren<Interactive.MiniGame>();
            var enemyIndex = 0;
            foreach (var miniGame in obj)
            {
                miniGame.Initialize(config.Enemies[enemyIndex]);
                enemyIndex++;
                if (enemyIndex >= config.Enemies.Count) break;
            }
        }

        private void InitializeTraining()
        {
            var obj = GetComponentsInChildren<Training>();
            var trainIndex = 0;
            foreach (var training in obj)
            {
                training.Initialize(config.Trainings[trainIndex]);
                trainIndex++;
                if (trainIndex >= config.Trainings.Count) break;
            }
        }

        private void InitializePetUnlocker()
        {
            var obj = GetComponentInChildren<PetUnlocker>();
            obj.Initialize(config.PetUnlocker);
        }

        private void OnValidate()
        {
            gameObject.name = $"Stage_{ID}";
        }
    }
}