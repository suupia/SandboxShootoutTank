using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

namespace Nuts.Projects.BattleSystem.Decoration.Scripts
{
    /// <summary>
    /// デコレーション系のすべてを管理する
    /// ローカル変数の管理をすべてこっちに持ってこれる
    /// 範囲が広い？個々で見れば小さいと思う
    /// アニメーション、サウンド、エフェクトなどのデコレーションを
    /// 任意に選択でき、かつ責任をすべてこのクラスにまとめる。
    /// これらはIDecoration系のインタフェースを継承していないといけない
    /// 
    /// 呼び出し側の制限は
    ///     正しく関数を呼ぶ（TestPlayerControllerを参考にしてください）
    ///     Networkedプロパティとして対応したNetworkDecoration系の変数を持つ
    /// </summary>
    public class PlayerDecorationDetector
    {
        public struct Data : INetworkStruct
        {
            public int MainActionCount;
            public int AttackCount;
            [Networked] public Vector3 Direction { get; set; }
        }

        private readonly List<IPlayerDecoration> _decorations;

        private int _preAttackCount;
        private int _preMainActionCount;
        private int _preHp;

        public PlayerDecorationDetector(params IPlayerDecoration[] decorations)
        {
            _decorations = decorations.ToList();
        }

        public void OnSpawned()
        {
            _decorations.ForEach(d => d.OnSpawned());
        }

        public void OnMainAction(ref Data data)
        {
            data.MainActionCount++;
        }

        public void OnChangeDirection(ref Data data, Vector3 direction)
        {
            data.Direction = direction;
        }


        public void OnAttacked(ref Data data)
        {
            data.AttackCount++;
        }

        /// <summary>
        /// AttackとMainAttackとMoveとDamageとDeadの管理
        /// </summary>
        /// <param name="data"></param>
        /// <param name="hp"></param>
        public void OnRendered(Data data, int hp)
        {
            var direction = data.Direction;
            _decorations.ForEach(d => d.OnChangeDirection(direction));
            _decorations.ForEach(d => d.OnMoved());

            if (hp != _preHp) OnChangedHp(hp);

            if (data.MainActionCount > _preMainActionCount)
            {
                _decorations.ForEach(d => d.OnMainAction());
                _preMainActionCount = data.MainActionCount;
            }

            if (data.AttackCount > _preAttackCount)
            {
                _decorations.ForEach(d => d.OnAttacked());
                _preAttackCount = data.AttackCount;
            }
        }

        private void OnChangedHp(int hp)
        {
            if (hp <= 0)
            {
                Debug.Log("OnDead!");
                _decorations.ForEach(d => d.OnDead());
            }
            else if (hp < _preHp)
            {
                Debug.Log("OnDamaged!");

                _decorations.ForEach(d => d.OnDamaged());
            }

            _preHp = hp;
        }
    }
}