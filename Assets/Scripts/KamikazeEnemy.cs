using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using static UnityEngine.Quaternion;

public class KamikazeEnemy : Enemy
{
        [SerializeField] private float attackSpeedMultiplier = 2f;
        [SerializeField] private float rotationSpeed = 15f;
        
        private bool _isInRange;
        private Vector3 _baseLookDirection = Vector3.left;
        private float _explosionDelay = 3f;
        private float _remainingDelay = 0f;
        public static UnityEvent<Type, Vector3> OnExplosion;
        public static UnityEvent<KamikazeEnemy> OnCountdownTrigger;
        private void Update()
        {
                if (_isDead) return;
                LifetimeHandling();
                BasicMovementHandling();
        }

        private void BasicMovementHandling()
        {
                if (!_isInRange)
                {
                       Vector3 newDirection = Vector3.RotateTowards(
                                transform.forward, 
                                _baseLookDirection,
                                rotationSpeed * Time.deltaTime, 
                                0.0f
                                );
                        transform.rotation = LookRotation(newDirection);
                        Vector3 moveDirection = transform.forward * (Stats.MovementSpeed * Time.deltaTime);
                        transform.Translate(moveDirection, Space.World);
                }
        }

        private void OnTriggerStay(Collider other)
        {
                _remainingDelay = _explosionDelay;
                if (other)
                {
                        splineAnimate.enabled = false;
                        _isInRange = true;
                        MoveTowardsTarget(other);
                        TriggerExplosionTimer();
                }
        }

        private void TriggerExplosionTimer()
        {
                OnCountdownTrigger?.Invoke(this);
                if (_remainingDelay > 0)
                {
                        _remainingDelay -= Time.deltaTime;
                }
                else
                {
                        OnExplosion?.Invoke(GetType(), transform.position);
                }
        }

        private void OnTriggerExit(Collider other)
        {
                _isInRange = false;
        }

        private void MoveTowardsTarget(Collider other)
        {
                Vector3 targetDirection = other.transform.position - transform.position;
                Vector3 newDirection = Vector3.RotateTowards(
                        transform.forward,
                        targetDirection,
                        rotationSpeed * Time.deltaTime,
                        0.0f
                );
                transform.rotation = LookRotation(newDirection);
                Vector3 moveDirection = transform.forward * (Stats.MovementSpeed * attackSpeedMultiplier * Time.deltaTime);
                transform.Translate(moveDirection, Space.World);
        }
}