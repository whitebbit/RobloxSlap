using UnityEngine;

namespace _3._Scripts.Actions.Scriptable
{
    [CreateAssetMenu(fileName = "TrainingObjectConfig", menuName = "ScriptableObjects/TrainingObjectConfig", order = 0)]
    public class TrainingObjectConfig : ScriptableObject
    {
        [SerializeField] private TrainingObject prefab;
        [SerializeField] private float reward;
        [SerializeField] private float health;

        public TrainingObject Prefab => prefab;
        public float Reward => reward;

        public float Health => health;
    }
}