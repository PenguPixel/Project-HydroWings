using UnityEngine;

public class BossHitbox : MonoBehaviour
{
    [SerializeField] private LolliOliBotHealth bossHealth;
    [SerializeField] private float damageMultiplier = 1f;

    public void DealDamage(float damage)
    {
        if (bossHealth == null)
            return;

        bossHealth.DealDamage(damage * damageMultiplier);
    }
}