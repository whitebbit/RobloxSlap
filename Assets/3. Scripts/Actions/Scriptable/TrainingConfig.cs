using System.Collections.Generic;
using UnityEngine;

namespace _3._Scripts.Actions.Scriptable
{
    [CreateAssetMenu(fileName = "TrainingConfig", menuName = "ScriptableObjects/TrainingConfig", order = 0)]
    public class TrainingConfig : ScriptableObject
    {
        [SerializeField] private List<TrainingObjectConfig> trainingObjects = new();

        public IEnumerable<TrainingObjectConfig> TrainingObjects => trainingObjects;
    }
}