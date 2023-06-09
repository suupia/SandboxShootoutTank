using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;

namespace Nuts.BattleSystem.Enemy.Scripts.Boss_Previous.Attack
{
    /// <summary>
    /// Attackが呼ばれてから遅延してattackの攻撃を呼び出す
    /// 使用しなくても良いが、クライアントコードが複雑にならないことが期待できる。
    /// ここでのAttackはasyncになっている。キャストすればawait句が使用できる
    /// </summary>
    public class DelayAttack : AttackWrapper, IBoss1Attack
    {
        private float _delay;

        public DelayAttack(float delay, [NotNull] IBoss1Attack attack)
        {
            _delay = delay;
            _attack = attack;
        }

        public async void Attack()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_delay));
            _attack.Attack();
        }
    }
}

