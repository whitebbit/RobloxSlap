using UnityEngine;
using UnityEngine.Serialization;

namespace _3._Scripts.Enemies.Scriptable
{
    [CreateAssetMenu(fileName = "Enemy Data", menuName = "EnemyData", order = 0)]
    public class EnemyData : ScriptableObject
    {
        [SerializeField] private string localizationID;
        [SerializeField] private Sprite icon;
        [SerializeField] private float health;
        [SerializeField] private float strength;
        [SerializeField] private ComplexityType complexityType;
        
        public string LocalizationID => localizationID;

        public float Health => health;
        public Sprite Icon => icon;

        public float Strength => strength;

        public ComplexityType ComplexityType => complexityType;
    }
}