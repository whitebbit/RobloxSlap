using System.Collections.Generic;
using _3._Scripts.Actions.Scriptable;
using _3._Scripts.Enemies.Scriptable;
using _3._Scripts.Pets.Scriptables;
using UnityEngine;

namespace _3._Scripts.Stages.Scriptable
{
    [CreateAssetMenu(fileName = "StageConfig", menuName = "StageConfig", order = 0)]
    public class StageConfig : ScriptableObject
    {
        [SerializeField, Min(0)] private int id;
        [Space] [SerializeField] private PetUnlockerConfig petUnlocker;
        
        [SerializeField] private List<EnemyData> enemies = new();
        [SerializeField] private List<TrainingConfig> trainings = new ();

        public PetUnlockerConfig PetUnlocker => petUnlocker;
        public List<EnemyData> Enemies => enemies;
        public List<TrainingConfig> Trainings => trainings;
        public int ID => id;
        
        
    }
}