
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

        [Networked] private ref NetworkDecorationPlayer DecorationPlayerRef => ref MakeRef<NetworkDecorationPlayer>();

        //以下はテスト用のプロパティ
        [Networked] private byte Hp { get; set; } = 2;
        [Networked] private float Horizontal { get; set; }

        private GameObject _playerUnitObject;
        private DecorationPlayerContainer _decorationContainer;

        public override void Spawned()
        {
            Setup();
            _decorationContainer.OnSpawned();
        }

        private void Setup()
        {
            _playerUnitObject = Instantiate(planePrefab, transform);
            _decorationContainer = new DecorationPlayerContainer(
                new PlaneAnimatorSetter(_playerUnitObject));
        }
        
        public override void FixedUpdateNetwork()
        {
            if (GetInput(out NetworkInputData input))
            {
                if (input.Buttons.WasPressed(PreButtons, PlayerOperation.MainAction))
                {
                    _decorationContainer.OnMainAction(ref DecorationPlayerRef);
                }

                Horizontal = input.Horizontal;
                
                //Assuming Attacked
                if (input.Buttons.WasPressed(PreButtons, PlayerOperation.Debug1))
                {
                    _decorationContainer.OnAttacked(ref DecorationPlayerRef);
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
            //Assuming changed transform.forward
            //現状のテストだとNetworkTransform系がついていないので完全に同期はしない可能性がある。あくまでテスト用のコード
            var preRotation = _playerUnitObject.transform.rotation.eulerAngles;
            var deltaY = Horizontal * Time.deltaTime * 100;
            var rotation = Quaternion.Euler(0, preRotation.y + deltaY, 0);
            _playerUnitObject.transform.rotation = rotation;
        }

        public override void Render()
        {
            //Decoration側の理想としては、OnRenderedの中でOnMovedが呼ばれる
            //そのため、動き系の処理と同じループ頻度でOnRenderedは呼んでほしい
            //ただ、トルク系は動きが残るので大丈夫かも。要検討
            _decorationContainer.OnRendered(ref DecorationPlayerRef, Hp);
        }
    }
}