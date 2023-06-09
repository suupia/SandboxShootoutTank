﻿using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using Carry.CarrySystem.Player.Scripts;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Fusion;
#nullable enable
namespace Carry.CarrySystem.Map.Scripts
{
    /// <summary>
    /// フロアごとに別のマップを生成し、返すクラス
    /// </summary>
    public class EntityGridMapSwitcher
    {
        readonly EntityGridMapLoader _gridMapLoader;
        int _currentIndex;
        EntityGridMap _currentMap;
        TilePresenterAttacher _tilePresenterAttacher;
        
        [Inject]
        public EntityGridMapSwitcher(EntityGridMapLoader gridMapGridMapLoader)
        {
            _gridMapLoader = gridMapGridMapLoader;
            _currentIndex = 1; //Floor1から始まる
            _currentMap = _gridMapLoader.LoadEntityGridMap(_currentIndex); // indexはとりあえず0にしておく
        }
        public void RegisterTilePresenterContainer( TilePresenterAttacher tilePresenterAttacher)
        {
            _tilePresenterAttacher = tilePresenterAttacher;
            _tilePresenterAttacher.AttachTilePresenter(_currentMap);
        }
        public EntityGridMap GetMap()
        {
            return _currentMap;
        }
        
        public void NextFloor()
        {
            Debug.Log($"次のフロアに変更します nextIndex: {_currentIndex + 1}");
            _currentIndex++;
            var nextMap = _gridMapLoader.LoadEntityGridMap(_currentIndex);
            _currentMap = nextMap;
            // _tilePresenterRegister?.RegisterTilePresenter(runner, _currentMap);
            _tilePresenterAttacher?.AttachTilePresenter(_currentMap);
            
            // 以下リセット処理
            var players = GameObject.FindObjectsByType<CarryPlayerController_Net>(FindObjectsSortMode.None);
            foreach (var player in players)
            {
                player.Reset();
            }

        }
    }
}