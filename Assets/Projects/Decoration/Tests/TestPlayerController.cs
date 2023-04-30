
using System;
using Decoration;
using Fusion;
using UnityEngine;
using Main;

namespace Animations.Tests
{
    public class TestPlayerController : NetworkBehaviour
    {
        [SerializeField] private GameObject planePrefab;
        [Networked] private NetworkButtons PreButtons { get; set; }

        [Networked] private ref PlayerDecorationContainer.Data DecorationDataRef => ref MakeRef<PlayerDecorationContainer.Data>();

        //以下はテスト用のプロパティ
        [Networked] private byte Hp { get; set; } = 2;

        private GameObject _playerUnitObject;
        private PlayerDecorationContainer _playerDecorationContainer;

        public override void Spawned()
        {
            Setup();
            _playerDecorationContainer.OnSpawned();
        }

        private void Setup()
        {
            _playerUnitObject = Instantiate(planePrefab, transform);
            _playerDecorationContainer = new PlayerDecorationContainer(
                new PlaneAnimatorSetter(_playerUnitObject));
        }
        
        public override void FixedUpdateNetwork()
        {
            if (GetInput(out NetworkInputData input))
            {
                if (input.Buttons.WasPressed(PreButtons, PlayerOperation.MainAction))
                {
                    _playerDecorationContainer.OnMainAction(ref DecorationDataRef);
                }

                //Assuming changed transform.forward
                //現状のテストだとNetworkTransform系がついていないので完全に同期はしない可能性がある。あくまでテスト用のコード
                var direction = new Vector3(input.Horizontal, 0, input.Vertical).normalized;

                _playerDecorationContainer.OnChangeDirection(ref DecorationDataRef, direction);

                //Assuming Attacked
                if (input.Buttons.WasPressed(PreButtons, PlayerOperation.Debug1))
                {
                    _playerDecorationContainer.OnAttacked(ref DecorationDataRef);
                }
                
                //Assuming Dead
                if (input.Buttons.WasPressed(PreButtons, PlayerOperation.Debug2))
                {
                    Hp--;
                }

                PreButtons = input.Buttons;
            }
        }

        private void Update()
        {
 
        }

        public override void Render()
        {

            
            //Decoration側の理想としては、OnRenderedの中でOnMovedが呼ばれる
            //そのため、動き系の処理と同じループ頻度でOnRenderedは呼んでほしい
            //ただ、トルク系は動きが残るので大丈夫かも。要検討
            _playerDecorationContainer.OnRendered(ref DecorationDataRef, Hp);
        }
    }
}