﻿using Fusion;

namespace Nuts.BattleSystem.GameScene.Interfaces
{
    public interface ITimerObserver
    {
        void Update(NetworkRunner runner, ITimer timer);
    }

    public interface ITimer
    {
        void NotifyObservers(NetworkRunner runner);
        float getRemainingTime(NetworkRunner runner);
        bool isExpired(NetworkRunner runner);
    }
}