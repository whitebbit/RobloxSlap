using System.Collections.Generic;
using UnityEngine;

namespace _3._Scripts.Actions.Scriptable
{
    [CreateAssetMenu(fileName = "TrainingConfig", menuName = "ScriptableObjects/TrainingConfig", order = 0)]
    public class TrainingConfig : ScriptableObject
    {
        [SerializeField] private List<TrainingObject> trainingObjects = new();
        [SerializeField] private float count;
        [SerializeField] private float requiredCount;

        public IEnumerable<TrainingObject> TrainingObjects => trainingObjects;
        public float Count => count;
        public float RequiredCount => requiredCount;
    }
}