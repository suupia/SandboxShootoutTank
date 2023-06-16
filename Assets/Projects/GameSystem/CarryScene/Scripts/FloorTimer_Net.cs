﻿using Fusion;
using Carry.CarrySystem.CarryScene.Interfaces;
using UnityEngine;
using VContainer;

namespace Carry.CarrySystem.CarryScene.Scripts
{
    public class FloorTimer_Net : NetworkBehaviour
    {
        readonly float _waveTime = 10f;
        bool _isInitialized;
        WaveTimer _waveTimer;
        [Networked] public TickTimer tickTimer { get; set; }
    
        [Inject]
        public void Construct(WaveTimer waveTimer)
        {
            _waveTimer = waveTimer;
            Debug.Log($"_waveTimer : {_waveTimer}");
        }
    
        public void Init()
        {
            _isInitialized = true;
            // tickTimer = TickTimer.CreateFromSeconds(Runner, _waveTime); // ここに書くの良くないかも
        }
    
        public override void FixedUpdateNetwork()
        {
            if (_isInitialized == false) return;
            // if(Object?.IsValid == false) return;
            if(tickTimer.ExpiredOrNotRunning(Runner)) tickTimer = TickTimer.CreateFromSeconds(Runner, _waveTime);
            _waveTimer.tickTimer = tickTimer;
            _waveTimer.NotifyObservers(Runner);
        }
    }
    
    public class WaveTimer : ITimer
    {
        readonly GameContext _gameContext;
    
        [Inject]
        public WaveTimer(GameContext gameContext)
        {
            _gameContext = gameContext;
        }
    
        public TickTimer tickTimer { get; set; }
    
        public void NotifyObservers(NetworkRunner runner)
        {
            _gameContext.Update(runner, this);
        }
    
        public float getRemainingTime(NetworkRunner Runner)
        {
            return tickTimer.RemainingTime(Runner).HasValue ? tickTimer.RemainingTime(Runner).Value : 0f;
        }
    
        public bool isExpired(NetworkRunner Runner)
        {
            return tickTimer.ExpiredOrNotRunning(Runner);
        }
    }
}