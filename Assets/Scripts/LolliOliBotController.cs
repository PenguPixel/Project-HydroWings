using System.Collections;
using UnityEngine;

public class LolliOliBotController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private BossAudio bossAudio;
    [SerializeField] private GameObject bossHealthBar;

    [Header("Weakpoints")]
    [SerializeField] private Collider[] weakPoints;

    [Header("Boss Behaviour")]
    [SerializeField] private float bossStartDelay = 13f;
    [SerializeField] private int minimumIdleLoops = 2;
    [SerializeField] private int maximumIdleLoops = 3;

    [Header("Death Explosions")]
    [SerializeField] private int explosionCount = 12;
    [SerializeField] private float timeBetweenExplosions = 0.2f;
    [SerializeField] private float finalExplosionDelay = 1f;

    [SerializeField] private Vector2 explosionRangeX =
        new Vector2(-15f, 8f);

    [SerializeField] private Vector2 explosionRangeY =
        new Vector2(0f, 20f);

    [SerializeField] private Vector2 explosionRangeZ =
        new Vector2(-10f, 0f);

    [SerializeField] private GameObject kamikazeExplosion;
    [SerializeField] private float finalExplosionScale = 10f;

    [SerializeField] private Vector3 finalExplosionOffset =
        new Vector3(0f, 10f, 0f);

    [Header("Final Explosion Sound")]
    [SerializeField] private AudioClip finalExplosionSound;
    [SerializeField] private float finalExplosionVolume = 1f;

    private static readonly int LiquidAttack =
        Animator.StringToHash("LiquidAttack");

    private static readonly int LollipopAttack =
        Animator.StringToHash("LollipopAttack");

    private static readonly int SpawnEnemies =
        Animator.StringToHash("SpawnEnemies");

    private static readonly int Death =
        Animator.StringToHash("Death");

    private bool _isDead;

    private IEnumerator Start()
    {
        SetWeakPointsActive(false);

        if (bossHealthBar != null)
        {
            bossHealthBar.SetActive(false);
        }

        if (bossAudio != null)
        {
            bossAudio.PlayBossMusic();
        }
        else
        {
            Debug.LogWarning(
                "LolliOliBotController: BossAudio wurde nicht zugewiesen."
            );
        }

        yield return null;

        StartCoroutine(BossRoutine());
    }

    private IEnumerator BossRoutine()
    {
        yield return new WaitForSeconds(bossStartDelay);

        if (_isDead)
        {
            yield break;
        }

        SetWeakPointsActive(true);

        if (bossHealthBar != null)
        {
            bossHealthBar.SetActive(true);
        }

        while (!_isDead)
        {
            int idleLoops = Random.Range(
                minimumIdleLoops,
                maximumIdleLoops + 1
            );

            AnimatorStateInfo idleInfo =
                animator.GetCurrentAnimatorStateInfo(0);

            float idleDuration = idleInfo.length;

            yield return new WaitForSeconds(
                idleDuration * idleLoops
            );

            if (_isDead)
            {
                yield break;
            }

            TriggerRandomAttack();

            yield return new WaitUntil(() =>
                _isDead ||
                !animator.GetCurrentAnimatorStateInfo(0)
                    .IsName("Boss_Idle_UpperBody")
            );

            if (_isDead)
            {
                yield break;
            }

            AnimatorStateInfo attackInfo =
                animator.GetCurrentAnimatorStateInfo(0);

            float attackDuration = attackInfo.length;

            yield return new WaitForSeconds(
                attackDuration
            );
        }
    }

    private void SetWeakPointsActive(bool active)
    {
        if (weakPoints == null)
        {
            return;
        }

        foreach (Collider weakPoint in weakPoints)
        {
            if (weakPoint != null)
            {
                weakPoint.enabled = active;
            }
        }
    }

    private void TriggerRandomAttack()
    {
        if (_isDead)
        {
            return;
        }

        int randomAttack = Random.Range(0, 3);

        switch (randomAttack)
        {
            case 0:
                animator.SetTrigger(LiquidAttack);
                break;

            case 1:
                animator.SetTrigger(LollipopAttack);
                break;

            case 2:
                animator.SetTrigger(SpawnEnemies);
                break;
        }
    }

    public void TriggerDeath()
    {
        if (_isDead)
        {
            return;
        }

        _isDead = true;

        StopAllCoroutines();

        SetWeakPointsActive(false);

        if (bossHealthBar != null)
        {
            bossHealthBar.SetActive(false);
        }

        if (bossAudio != null)
        {
            bossAudio.PlayOutro();
        }

        if (animator != null)
        {
            animator.SetTrigger(Death);
        }

        StartCoroutine(BossExplosionRoutine());
    }

    private IEnumerator BossExplosionRoutine()
    {
        for (int i = 0; i < explosionCount; i++)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(explosionRangeX.x, explosionRangeX.y),
                Random.Range(explosionRangeY.x, explosionRangeY.y),
                Random.Range(explosionRangeZ.x, explosionRangeZ.y)
            );

            Vector3 explosionPosition =
                transform.position + randomOffset;

            KamikazeEnemy.OnExplosion?.Invoke(explosionPosition);

            yield return new WaitForSeconds(timeBetweenExplosions);
        }

        yield return new WaitForSeconds(finalExplosionDelay);

        Vector3 finalExplosionPosition =
            transform.position + finalExplosionOffset;

        if (kamikazeExplosion != null)
        {
            GameObject finalExplosion = Instantiate(
                kamikazeExplosion,
                finalExplosionPosition,
                Quaternion.identity
            );

            finalExplosion.transform.localScale =
                Vector3.one * finalExplosionScale;
        }

        if (finalExplosionSound != null)
        {
            AudioSource.PlayClipAtPoint(
                finalExplosionSound,
                finalExplosionPosition,
                finalExplosionVolume
            );
        }

        Destroy(gameObject);
    }
}