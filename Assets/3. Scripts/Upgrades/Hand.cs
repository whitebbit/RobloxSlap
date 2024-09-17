using _3._Scripts.UI.Scriptable.Shop;
using Unity.VisualScripting;
using UnityEngine;

namespace _3._Scripts.Upgrades
{
    public class Hand : MonoBehaviour
    {
        private ParticleSystem _particleSystem;

        public void Initialize(UpgradeItem hand)
        {
            if (_particleSystem != null)
                Destroy(_particleSystem.gameObject);

            if (hand.Particle == null) return;

            _particleSystem = Instantiate(hand.Particle, transform);

            var r = _particleSystem.GetComponent<Renderer>();
            var t = _particleSystem.transform;

            r.material.mainTexture = hand.Texture;

            t.localPosition = new Vector3(0, 0, 0);
            t.localScale = Vector3.one;
            t.eulerAngles = Vector3.zero;
        }
    }
}