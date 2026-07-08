public class KamikazeEnemy : Enemy
{
        private void Update()
        {
                LifetimeHandling();
                if (_isDead) return;
        }
}