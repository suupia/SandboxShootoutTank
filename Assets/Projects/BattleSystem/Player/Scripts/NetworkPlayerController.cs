using Nuts.Utility.Scripts;
using UnityEngine;
using VContainer;
using VContainer.Unity;
#nullable enable
namespace Nuts.BattleSystem.Player.Scripts
{
    /// <summary>
    ///     The only NetworkBehaviour to control the character.
    ///     Note: Objects to which this class is attached do not move themselves.
    ///     Attachment on the inspector is done to the Info class.
    /// </summary>
    public class NetworkPlayerController : AbstractNetworkPlayerController
    {
        ReturnToMainBaseGauge? _returnToMainBaseGauge;

        protected override void Update()
        {
            base.Update();

            if (Object.HasInputAuthority)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift)) _returnToMainBaseGauge?.FillGauge();

                if (Input.GetKeyUp(KeyCode.LeftShift)) _returnToMainBaseGauge?.ResetGauge();

                // ToDo : デバッグ用なので後で消す
                if (Input.GetKeyDown(KeyCode.H)) Debug.Log($"hp = {PlayerStruct.Hp}");
            }
        }

        public override void Spawned()
        {
            base.Spawned();

            if (Object.HasInputAuthority)
            {
                // setup ReturnToMainBase
                _returnToMainBaseGauge = FindObjectOfType<LifetimeScope>().Container.Resolve<ReturnToMainBaseGauge>();
                if (_returnToMainBaseGauge == null)
                {
                    Debug.LogError($"_returnToMainBaseGauge is null.");
                    return;
                }
                _returnToMainBaseGauge.SetOnReturnToMainBase(RPC_SetToOrigin);
            }
        }

    }
}