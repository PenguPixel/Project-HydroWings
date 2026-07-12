using System.Collections;
using UnityEngine;

public class LolliOliBotController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private int minimumIdleLoops = 1;
    [SerializeField] private int maximumIdleLoops = 3;

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
        // Einen Frame warten, bis der Animator gestartet ist
        yield return null;

        StartCoroutine(BossRoutine());
    }

    private IEnumerator BossRoutine()
    {
        while (!_isDead)
        {
            // Zufällig 1 bis 3 Idle-Durchläufe
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

            // Zufällige Attacke starten
            TriggerRandomAttack();

            // Warten, bis der Animator wirklich
            // von Idle in die Attacke gewechselt hat
            yield return new WaitUntil(() =>
                !animator.GetCurrentAnimatorStateInfo(0)
                    .IsName("Boss_Idle_UpperBody")
            );

            AnimatorStateInfo attackInfo =
                animator.GetCurrentAnimatorStateInfo(0);

            float attackDuration = attackInfo.length;

            // Warten, bis die Attacke fertig ist
            yield return new WaitForSeconds(
                attackDuration
            );

            // Danach beginnt die while-Schleife erneut
        }
    }

    private void TriggerRandomAttack()
    {
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
            return;

        _isDead = true;

        StopAllCoroutines();

        animator.SetTrigger(Death);
    }
}