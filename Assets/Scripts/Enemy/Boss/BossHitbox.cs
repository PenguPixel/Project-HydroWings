using UnityEngine;

public class BossHitbox : MonoBehaviour
{
    [SerializeField] private LolliOliBotHealth bossHealth;
    [SerializeField] private float damageMultiplier = 1f;

    private WeakpointHitFlash _hitFlash;

    private void Awake()
    {
        _hitFlash = GetComponent<WeakpointHitFlash>();
    }

    public void DealDamage(float damage)
    {
        if (!bossHealth)
            return;

        // Weakpoint aufleuchten lassen
        _hitFlash?.Flash();

        // Schaden verursachen
        bossHealth.DealDamage(damage * damageMultiplier);
    }
}