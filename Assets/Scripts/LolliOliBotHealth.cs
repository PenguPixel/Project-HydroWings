using UnityEngine;
using UnityEngine.Events;

public class LolliOliBotHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 500f;

    private float _currentHealth;
    private bool _isDead;

    public UnityEvent<float, float> OnHealthChanged;
    public static UnityEvent OnBossDeath = new UnityEvent();

    private void Start()
    {
        _currentHealth = maxHealth;
        OnHealthChanged?.Invoke(_currentHealth, maxHealth);
    }

    public void DealDamage(float damage)
    {
        if (_isDead)
            return;

        _currentHealth = Mathf.Max(_currentHealth - damage, 0f);

        OnHealthChanged?.Invoke(_currentHealth, maxHealth);

        Debug.Log(
            $"Lolli-Oli-Bot erhält {damage} Schaden. Leben: {_currentHealth}/{maxHealth}"
        );

        if (_currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        if (_isDead)
            return;

        _isDead = true;
        OnBossDeath?.Invoke();  // hier Win Condition einfügen

        LolliOliBotController controller =
            GetComponent<LolliOliBotController>();

        controller?.TriggerDeath();
    }
}