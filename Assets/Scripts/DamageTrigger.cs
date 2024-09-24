using System;
using Fusion;
using UnityEngine;

namespace Network
{
    [RequireComponent(typeof(SphereCollider))]
    public class DamageTrigger : NetworkBehaviour
    {
        private SphereCollider _collider;

        public event Action<Enemy> EnemyEnter;
        public event Action<Enemy> EnemyExit;

        public float Radius
        {
            get => _collider.radius;
            set => _collider.radius = value;
        }
        
        public void Awake()
        {
            _collider = GetComponent<SphereCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            var enemy = other.GetComponent<Enemy>();
            if (enemy == null) return;
            Debug.Log("In " + other.name);
            EnemyEnter?.Invoke(enemy);
        }

        private void OnTriggerExit(Collider other)
        {
            var enemy = other.GetComponent<Enemy>();
            if (enemy == null) return;
            Debug.Log("Out " + other.name);
            EnemyExit?.Invoke(enemy);
        }
    }
}