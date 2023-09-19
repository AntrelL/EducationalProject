using UnityEngine;

namespace Platformer
{
    public class Player : Movement
    {
        protected override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.Space))
                Jump();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            Move(Input.GetAxis("Horizontal"));
        }

        public void TakeFruit(Fruit fruit) { }
    }
}