
using DG.Tweening;
using Myd.Common;
using Myd.Platform.Core;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace Myd.Platform
{
    enum EGameState
    {
        Load,
        Play,
        Pause,
        Fail,
    }
    public class Game : MonoBehaviour, IGameContext
    {
        public static Game Instance;

        [SerializeField]
        public Level level;
        //장면 효과 관리자
        [SerializeField]
        private SceneEffectManager sceneEffectManager;
        [SerializeField]
        private SceneCamera gameCamera;
        //플레이어
        public Player player { get; private set;}

        Texture2D cursorTexture;

        EGameState gameState;

        ProjectileManager projectileManager;

        void Awake()
        {
            Instance = this;

            gameState = EGameState.Load;

            player = new Player(this);

            cursorTexture = Resources.Load<Texture2D>("Sprites/Crosshair");

            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);

        }

        //void Start()
        //{

        //}

        IEnumerator Start()
        {
            yield return null;

            //플레이어 로드
            player.Reload(level.Bounds, level.StartPosition);
            this.gameState = EGameState.Play;

            projectileManager = GetComponentInChildren<ProjectileManager>();
            projectileManager.Init(player);


            yield return null;
        }

        public void Update()
        {
            float deltaTime = Time.unscaledDeltaTime;
            if (UpdateTime(deltaTime))
            {
                if (this.gameState == EGameState.Play)
                {
                    GameInput.Update(deltaTime);
                    //플레이어 로직 데이터 업데이트
                    player.Update(deltaTime);
                    //카메라 업데이트
                    gameCamera.SetCameraPosition(player.GetCameraPosition());
                }
            }

        }



        #region 冻帧
        private float freezeTime;

        // 프레임 데이터를 업데이트하고, 프레임이 없으면 true를 반환합니다.
        public bool UpdateTime(float deltaTime)
        {
            if (freezeTime > 0f)
            {
                freezeTime = Mathf.Max(freezeTime - deltaTime, 0f);
                return false;
            }
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
            }
            return true;
        }

        //정지화면
        public void Freeze(float freezeTime)
        {
            this.freezeTime = Mathf.Max(this.freezeTime, freezeTime);
            if (this.freezeTime > 0)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
        #endregion
        public void CameraShake(Vector2 dir, float duration)
        {
            this.gameCamera.Shake(dir, duration);
        }

        public IEffectControl EffectControl { get=>this.sceneEffectManager; }

        public ISoundControl SoundControl { get; }

        
    }

}
