using System;
using UnityEngine;

namespace _3._Scripts.Stages
{
    public class Stage : MonoBehaviour
    {
        [SerializeField, Min(0)] private int id;
        [SerializeField] private Transform spawnPoint;

        public Transform SpawnPoint => spawnPoint;

        public int ID => id;

        private void OnValidate()
        {
            gameObject.name = $"Stage_{id}";
        }
    }
}