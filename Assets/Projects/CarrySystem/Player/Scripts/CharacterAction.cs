﻿using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;
using VContainer;
using VContainer.Unity;
#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class CharacterAction : ICharacterAction
    {
        readonly Transform _transform;
        readonly EntityGridMap _map;
        public CharacterAction(Transform transform)
        {
            _transform = transform;
            var resolver = Object.FindObjectOfType<LifetimeScope>().Container; // このコンストラクタはNetworkBehaviour内で実行されるため、ここで取得してよい
            _map = resolver.Resolve<EntityGridMapSwitcher>().GetMap();
        }
        public void Action()
        {
            Debug.Log($"ものを拾ったり、置いたりします");

            // 自身のGridPosを表示
            var gridPos = GridConverter.WorldPositionToGridPosition(_transform.position);
            Debug.Log($"Player GridPos: {gridPos}");

            // 前方のGridPosを表示
            var forward = _transform.forward;
            var direction = new Vector2(forward.x, forward.z);
            var gridDirection = GridConverter.WorldDirectionToGridDirection(direction);
            var forwardGridPos = gridPos + gridDirection;
            Debug.Log($"Player Forward GridPos: {forwardGridPos}");
            
            // そのGridPosにRockがあるかどうかを確認
            var index = _map.GetIndexFromVector(forwardGridPos);
            Debug.Log($"index : {index}のRockは{_map.GetSingleEntity<Rock>(index)}です");
            if (_map.GetSingleEntity<Rock>(forwardGridPos) != null)
            {
                Debug.Log($"Rockがあります！！！");
            }
            else
            {
                Debug.Log($"Rockがありません");
            }

            // アイテムがある場合は、アイテムを拾う

            // アイテムがない、かつ、アイテムを持っているときは、アイテムを置く
        }
    }
}