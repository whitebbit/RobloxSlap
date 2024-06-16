using _3._Scripts.Characters;
using UnityEngine;
using VInspector;

namespace _3._Scripts.UI.Scriptable.Shop
{
    [CreateAssetMenu(fileName = "UpgradeItem", menuName = "Shop Items/Upgrade Item", order = 0)]
    public class UpgradeItem : ShopItem
    {
        [SerializeField] private float booster;
        [SerializeField] private Color color;
        
        [Tab("Prefab")] [SerializeField]
        private Texture texture;
        [SerializeField] private ParticleSystem particle;

        public float Booster => booster;
        public Color Color => color;
        public Texture Texture => texture;
        public ParticleSystem Particle => particle;
        public override string Title()
        {
            return $"x{booster}<sprite index=1>";
        }
    }
}