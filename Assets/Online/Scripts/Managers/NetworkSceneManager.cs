using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class NetworkSceneManager : SimulationBehaviour, IPlayerJoined, IPlayerLeft, ISceneLoadDone, ISceneLoadStart
{
    [SerializeField] protected NetworkRunnerManager runnerManager;
    protected NetworkPlayerContainer networkPlayerContainer = new();
    protected NetworkEnemyContainer networkEnemyContainer = new();
    protected PhaseManager phaseManager;
    protected NetworkPlayerSpawner playerSpawner;
    protected NetworkEnemySpawner enemySpawner;
    
    // UniTask
    CancellationTokenSource cts = new();
    protected CancellationToken token;

    protected bool IsInitialized;

    
    protected async UniTask Init()
    {
        Debug.Log($"Start {SceneManager.GetActiveScene().name} Init");
        await runnerManager.StartScene();

        Debug.Log($"Runner:{Runner}, runnerManager.Runner:{runnerManager.Runner}");
        runnerManager.Runner.AddSimulationBehaviour(this); // Runnerに登録
        Debug.Log($"Runner:{Runner}");

        // networkEnemyContainer = FindObjectOfType<NetworkEnemyContainer>();
        phaseManager = FindObjectOfType<PhaseManager>();

        // Runner.AddSimulationBehaviour(networkEnemyContainer);
        Runner.AddSimulationBehaviour(phaseManager);
        
        // Domain
        playerSpawner = new NetworkPlayerSpawner(Runner);
        enemySpawner = new NetworkEnemySpawner(Runner);

        this.token = cts.Token;

        IsInitialized = true;
        Debug.Log($"Finish {SceneManager.GetActiveScene().name} Init");

    }

    public void PlayerJoined(PlayerRef player)
    {
        if (Runner.IsServer)
        {
            // IsInitializedがtrueになってからスポーンさせる
            // var _= AsyncSpawnPlayer(player, token);
            playerSpawner.SpawnPlayer(player,networkPlayerContainer);

            // Todo: RunnerがSetActiveシーンでシーンの切り替えをする時に対応するシーンマネジャーのUniTaskのキャンセルトークンを呼びたい
        }
    }

    // async UniTask AsyncSpawnPlayer( PlayerRef player,CancellationToken token)
    // {
    //     while (IsInitialized)
    //     {
    //         UniTask.Yield(token);
    //     }
    //     playerSpawner.SpawnPlayer(player,networkPlayerContainer);
    //
    // }

    public void PlayerLeft(PlayerRef player)
    {
        if (Runner.IsServer)
        {
            // IsInitializedがtrueになってからスポーンさせる
            playerSpawner.DeSpawnPlayer(player,networkPlayerContainer);
        }
    }

    public void SceneLoadStart()
    {
        Debug.Log($"SceneLoadStart()");
    }

    public void SceneLoadDone()
    {
        // AddSimulationBehaviour()でRunnerにコールバックが登録されているはずだから呼ばれるはず
        Debug.Log($"SceneLoadDone()");
    }
}

