using UnityEngine;

namespace Platformer
{
    [RequireComponent(typeof(VampirismSkill))]
    public class Player : Entity
    {
        private const string Horizontal = "Horizontal";

        private VampirismSkill _vampirismSkill;

        protected override void Start()
        {
            base.Start();

            _vampirismSkill = GetComponent<VampirismSkill>();
        }

        protected override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.Space))
                Jump();

            if (Input.GetKeyDown(KeyCode.E))
                _vampirismSkill.TryActivate();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            Move(Input.GetAxis(Horizontal));
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Item item))
                item.Take(this);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Enemy enemy))
            {
                Vector2 enemyKnockback = CalculateKnockback(RecliningForce, enemy.transform.position);
                enemy.ApplyDamage(Damage, enemyKnockback);
            }
        }
    }
}