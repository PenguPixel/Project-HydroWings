using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;
using UnityEngine.UIElements;
using static UnityEngine.Quaternion;

public class KamikazeEnemy : Enemy
{
        [SerializeField] private float attackSpeedMultiplier = 2f;
        [SerializeField] private float rotationSpeed = 15f;
        [SerializeField] private float explosionDelay = 3f;
        [SerializeField] private Renderer kamikazeRenderer;
        
        private bool _isInRange;
        private Vector3 _baseLookDirection = Vector3.left;
        private bool _isExploding = false;
        private float _explosionTimer = 0f;

        public static UnityEvent<Type, Vector3> OnExplosion;
        public static UnityEvent<KamikazeEnemy> OnCountdownTrigger;
        private void Update()
        {
                if (_isDead) return;
                LifetimeHandling();
                BasicMovementHandling();

                if (_isExploding)
                {
                        _explosionTimer += Time.deltaTime;
                        if (_explosionTimer >= explosionDelay)
                        {
                                
                        }
                }
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
                if (other)
                {
                        splineAnimate.enabled = false;
                        _isInRange = true;
                        MoveTowardsTarget(other);
                        TriggerSelfDestruction(other);
                }
        }

        private void TriggerSelfDestruction(Collider other)
        {
        _explosionTimer = 0f;
        _isExploding = true;
        
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