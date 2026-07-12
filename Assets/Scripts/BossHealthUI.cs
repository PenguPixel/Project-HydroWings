using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private LolliOliBotHealth bossHealth;

    private void Start()
    {
        bossHealth.OnHealthChanged.AddListener(UpdateHealthBar);
    }

    private void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    private void OnDestroy()
    {
        if (bossHealth != null)
        {
            bossHealth.OnHealthChanged.RemoveListener(UpdateHealthBar);
        }
    }
}