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
        [SerializeField] private MeshRenderer _blinkRenderer;
        
        private MaterialPropertyBlock _propBlock;
        private static readonly int IsBlinking = Shader.PropertyToID("_IsBlinking");

        private Collider _targetCollider;
        private bool _isInRange;
        private Vector3 _baseLookDirection = Vector3.left;
        private bool _isExploding = false;
        private float _explosionTimer = 0f;
        private float _scaledKamikazeDamage;
        private float _scaledMovementSpeed;

        public static UnityEvent<Vector3> OnExplosion = new UnityEvent<Vector3>();

        private void Start()
        {
                _propBlock = new MaterialPropertyBlock();
        }

        private void OnEnable()
        {
                _scaledKamikazeDamage = Stats.KamikazeDamage * GameManager.GlobalDifficultiyMultiplier;
                _scaledMovementSpeed = Stats.MovementSpeed * GameManager.GlobalDifficultiyMultiplier;
                _targetCollider = null;
                _isInRange = false;
                _isExploding = false;
                _explosionTimer = 0f;

                if (splineAnimate != null)
                {
                        splineAnimate.enabled = true;
                }

                if (_blinkRenderer != null)
                {
                        if (_propBlock == null)
                        {
                                _propBlock = new MaterialPropertyBlock();
                        }

                        _blinkRenderer.GetPropertyBlock(_propBlock);
                        _propBlock.SetFloat(IsBlinking, 0f);
                        _blinkRenderer.SetPropertyBlock(_propBlock);
                }
        }

        
        
        private void Update()
        {
                if (_isDead) return;
                LifetimeHandling();
                BasicMovementHandling();

                if (_isExploding) 
                {
                        _blinkRenderer.GetPropertyBlock(_propBlock);
                        _propBlock.SetFloat(IsBlinking, _isExploding ? 1f : 0f);
                        _blinkRenderer.SetPropertyBlock(_propBlock);
                        
                        _explosionTimer += Time.deltaTime;
                        if (_explosionTimer >= explosionDelay)
                        {
                                if (_targetCollider != null)
                                {
                                        var target = _targetCollider.GetComponent<Character>();

                                        if (target != null)
                                        {
                                                target.TakeDamage(_scaledKamikazeDamage);
                                        }
                                }

                                TriggerLocalDeath();
                                OnExplosion?.Invoke(transform.position);
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

                        Vector3 moveDirection =
                                transform.forward *
                                (_scaledMovementSpeed * Time.deltaTime);

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
                if (_isExploding) return;

                Debug.Log(
                        "Kamikaze wurde ausgelöst von: " +
                        other.name
                );

                _targetCollider = other;
                _explosionTimer = 0f;
                _isExploding = true;
        }

        private void OnTriggerExit(Collider other)
        {
                _isInRange = false;
                _targetCollider = null;
        }

        private void MoveTowardsTarget(Collider other)
        {
                Vector3 targetDirection =
                        other.transform.position -
                        transform.position;

                Vector3 newDirection = Vector3.RotateTowards(
                        transform.forward,
                        targetDirection,
                        rotationSpeed * Time.deltaTime,
                        0.0f
                );

                transform.rotation = LookRotation(newDirection);

                Vector3 moveDirection =
                        transform.forward *
                        (
                                _scaledMovementSpeed *
                                attackSpeedMultiplier *
                                Time.deltaTime
                        );

                transform.Translate(moveDirection, Space.World);
        }
}