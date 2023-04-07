using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows;

public class Tank : NetworkPlayerUnit
{
    NetworkRunner runner;
    [Networked] TickTimer ReloadTimer { get; set; }
    [Networked] NetworkObject Target { get; set; }

    NetworkCharacterControllerPrototype cc;
    RangeDetector rangeDetector;
    
    
    public Tank(NetworkPlayerInfo info) : base(info)
    {
        this.info = info;
        runner = info.runner;
        
        cc = info.playerObj.GetComponent<NetworkCharacterControllerPrototype>(); // playerObjは汎用的なプレハブからインスタンス化されていることを前提としている
        rangeDetector = info.playerObj.GetComponent<RangeDetector>();
    }

    public override void Move(Vector3 direction)
    {
        cc.Move(direction);
    }

    public override void Action(NetworkButtons buttons, NetworkButtons preButtons)
    {
        if (ReloadTimer.ExpiredOrNotRunning(runner))
        {
            //Auto Aim
            //一旦タグで識別、外部のスクリプトでトリガー管理。依存関係を減らしたいならPhysics系を使っても良さそう
            //その場合はLayer分けもしっかりしていきたい
            //いずれこの処理は書き換えるべき
            var enemies =  rangeDetector.GameObjects.Where(o => o != null && o.CompareTag("Enemy")).ToArray();
            if (enemies.Length > 0)
            {
                Target = enemies.First().GetComponent<NetworkObject>();

                //一時的に弾の発射位置のためにオフセットを適用
                //将来的には、戦車を上部と下部で別にして、上部が発射位置を管理するようにしようかな
                var offset = new Vector3(0, 1.2f, 0);
                runner.Spawn(info.bulletPrefab, info.playerObj.transform.position + offset, info.playerObj.transform.rotation, PlayerRef.None, OnBeforeSpawnBullet);
                ReloadTimer = TickTimer.CreateFromSeconds(runner, 2f);
            }
        }

        if (buttons.GetPressed(preButtons).IsSet(PlayerOperation.MainAction))
        {
            runner.Spawn(info.pickerPrefab, info.playerObj.transform.position,  info.playerObj.transform.rotation, PlayerRef.None);
        }
    }
    

    void OnBeforeSpawnBullet(NetworkRunner runner, NetworkObject obj)
    {
        var bullet = obj.GetComponent<Bullet>();
        bullet.AddForce(Target.transform.position - info.playerObj. transform.position);
    }
}
