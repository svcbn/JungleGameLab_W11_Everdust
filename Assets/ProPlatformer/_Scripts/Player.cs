

using Myd.Common;
using Myd.Platform;
using Myd.Platform.Core;
using UnityEngine;

namespace Myd.Platform
{
    /// <요약>
    /// 플레이어 클래스: 포함
    /// 1. 플레이어 디스플레이
    /// 2. 플레이어 컨트롤러(코어 컨트롤러)
    /// 두 사람이 내부적으로 상호 작용하도록 허용합니다.
    /// </summary>
    public class Player
    {
        private PlayerRenderer playerRenderer;
        private PlayerController playerController;

        private IGameContext gameContext;

        public Player(IGameContext gameContext)
        {
            this.gameContext = gameContext;
        }

        //플레이어 엔터티 로드
        public void Reload(Bounds bounds, Vector2 startPosition)
        {
            this.playerRenderer = Object.Instantiate(Resources.Load<PlayerRenderer>("PlayerRenderer"));
            PlayerManager.Instance.player = playerRenderer.transform.gameObject;
            //this.playerRenderer = AssetHelper.Create<PlayerRenderer>("Assets/ProPlatformer/_Prefabs/PlayerRenderer.prefab");
            this.playerRenderer.Reload();
            //초기화
            this.playerController = new PlayerController(playerRenderer, gameContext.EffectControl);
            this.playerController.Init(bounds, startPosition);

            PlayerParams playerParams = Resources.Load<PlayerParams>("PlayerParam");
            //PlayerParams playerParams = AssetHelper.LoadObject<PlayerParams>("Assets/ProPlatformer/PlayerParam.asset");
            playerParams.SetReloadCallback(() => this.playerController.RefreshAbility());
            playerParams.ReloadParams();
        }

        public void Update(float deltaTime)
        {
            playerController.Update(deltaTime);
            Render();
        }

        private void Render()
        {
            playerRenderer.Render(Time.deltaTime);

            Vector2 scale = playerRenderer.transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (int)playerController.Facing;
            playerRenderer.transform.localScale = scale;
            playerRenderer.transform.position = playerController.Position;

            //if (!lastFrameOnGround && this.playerController.OnGround)
            //{
            //    this.playerRenderer.PlayMoveEffect(true, this.playerController.GroundColor);
            //}
            //else if (lastFrameOnGround && !this.playerController.OnGround)
            //{
            //    this.playerRenderer.PlayMoveEffect(false, this.playerController.GroundColor);
            //}
            //this.playerRenderer.UpdateMoveEffect();

            this.lastFrameOnGround = this.playerController.OnGround;
        }

        private bool lastFrameOnGround;

        public Vector2 GetCameraPosition()
        {
            if (this.playerController == null)
            {
                return Vector3.zero;
            }
            return playerController.GetCameraPosition();
        }
    }

}
