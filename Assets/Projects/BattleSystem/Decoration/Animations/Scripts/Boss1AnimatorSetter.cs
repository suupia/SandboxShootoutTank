using Nuts.Projects.BattleSystem.Decoration.Scripts;
using UnityEngine;
using UnityEngine.Assertions;

namespace Nuts.BattleSystem.Decoration.Scripts
{
    public class Boss1AnimatorSetter: IBoss1Decoration
    {
        private readonly GameObject _gameObject;
        private readonly Animator _animator;
        private readonly Rigidbody _rd;
        private static readonly int Dead = Animator.StringToHash("Dead");
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Tackling = Animator.StringToHash("Tackling");
        private static readonly int Jumping = Animator.StringToHash("Jumping");
        private static readonly int SpitOut = Animator.StringToHash("SpitOut");
        private static readonly int Vacuuming = Animator.StringToHash("Vacuuming");

        public Boss1AnimatorSetter(GameObject gameObject)
        {
            Assert.IsNotNull(gameObject);

            _gameObject = gameObject;
            _animator = gameObject.GetComponentInChildren<Animator>();
            _rd = gameObject.GetComponentInParent<Rigidbody>();
            
            Assert.IsNotNull(_animator);
            Assert.IsNotNull(_rd);
        }

        public void OnTackle(bool onStart)
        {
            _animator.SetBool(Tackling, onStart);

        }

        public void OnJump(bool onStart)
        {
            _animator.SetBool(Jumping, onStart);
        }

        public void OnSpitOut()
        {
            _animator.SetTrigger(SpitOut);
        }

        public void OnVacuum(bool onStart)
        {
            _animator.SetBool(Vacuuming, onStart);
        }

        public void OnDamaged()
        {
            
        }

        public void OnDead()
        {
            _animator.SetTrigger(Dead);
        }

        public void OnMoved()
        {
            _animator.SetFloat(Speed, _rd.velocity.magnitude);
        }

        public void OnSpawned()
        {
        }
    }
}