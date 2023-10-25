using System;
using System.Collections;
using UnityEngine;

namespace Myd.Platform
{
    [CreateAssetMenu(fileName = "PlayerParams", menuName = "Pro Platformer/Player Param", order = 1 )]
    public class PlayerParams : ScriptableObject
    {
        [Header("벽타기 활성화")]
        public bool EnableWallSlide;
        [Header("코요테 타임 활성화")]
        public bool EnableJumpGrace;
        [Header("벽 점프 활성화")]
        [Tooltip("지구력을 소모하지 않아도 되는 테크닉")]
        public bool EnableWallBoost;

        [Header("수평방향 파라미터")]
        [Tooltip("최대 수평 속도")]
        public int MaxRun;
        [Tooltip("수평방향가속도")]
        public int RunAccel;
        [Tooltip("수평방향감속도")]
        public int RunReduce;
        [Space]
        [Header("수직 방향 매개 변수")]
        [Tooltip("중력 가속도")]
        public float Gravity; //중력
        [Tooltip("속도가 이 임계값보다 작으면 중력이 절반으로 감소합니다.값이 작을수록 체공 시간이 길어지고, 0은 닫힘을 나타냅니다.")]
        [Range(0, 9)]
        public float HalfGravThreshold;
        [Tooltip("최대 낙하 속도(대역 방향, 위는 양)")]
        public float MaxFall; //보통최대낙하속도
        [Tooltip("급강하의 최대속도(대역방향, 위로는 정)")]
        public float FastMaxFall;  //고속 최대 낙하 속도
        [Tooltip("낙하->급속하강 가속도")]
        public float FastMaxAccel; //급강하 가속도

        [Space]
        [Header("점프 매개변수")]
        [Tooltip("최대 점프 속도")]
        public float JumpSpeed;
        [Tooltip("점프 지속시간(점프시 점프버튼[VarJumpTime]초간 계속 반응하여 점프 최고높이에 영향을 미칩니다)")]
        public float VarJumpTime;
        [Tooltip("점프 수평 방향, 수평 방향 감속도")]
        public float JumpHBoost;
        [Tooltip("코요테 시간(플랫폼을 떠날 때 점프에 응답할 수 있는 시간)")]
        public float JumpGraceTime;
        [Tooltip("위로 이동하는데 장애가 있는 좌우 보정 픽셀")]
        public int UpwardCornerCorrection;

        [Header("Dash 스퍼트 인자")]
        [Tooltip("초스피드 스퍼트를 시작하다")]
        public float DashSpeed;          //스퍼트 속도
        [Tooltip("스퍼트 종료 후 속도")]
        public float EndDashSpeed;        //종료 스퍼트 속도
        [Tooltip("Y축방향 스퍼트의 감쇠계수")]
        public float EndDashUpMult;       //위로 스퍼트하면 저항.
        [Tooltip("스퍼트 타임")]
        public float DashTime;            //스퍼트 타임
        [Tooltip("스퍼트 냉각 시간")]
        public float DashCooldown = 2;         //스퍼트 냉각 시간,
        [Tooltip("스퍼트 재장전 시간\n")]
        public float DashRefillCooldown;   //스퍼트 재장전 시간
        [Tooltip("Dashs 수평 또는 수직 위치 보정 픽셀 값")]
        public int DashCornerCorrection;     //Dash시 차단물과의 시정가능거리 단위 0.1m
        [Tooltip("최대 Dash 수")]
        public int MaxDashes;    // 최대 Dash 수


        [Header("등반 매개변수")]
        [Tooltip("등반 수평선 검출 화소")]
        public int ClimbCheckDist;           //등반 검사 픽셀 값
        [Tooltip("등반 수직 방향 방사선 검출 화소")]
        public int ClimbUpCheckDist;         //픽셀 값을 검사하기 위해 위로 올라가기
        [Tooltip("등반 시간 이동 불가")]
        public float ClimbNoMoveTime;
        [Tooltip("상향등반속도")]
        public float ClimbUpSpeed;        //기어오르는 속도
        [Tooltip("하향등반속도")]
        public float ClimbDownSpeed;       //하강 속도
        [Tooltip("등반하락속도")]
        public float ClimbSlipSpeed;       //하강 속도
        [Tooltip("등반하락가속도")]
        public float ClimbAccel;          //하강 가속도
        [Tooltip("등반 시작 시 원래 Y축 속도에 대한 감쇠")]
        public float ClimbGrabYMult;       //등반시 잡음에 의한 Y축 속도 감쇠

        [Header("Hop参数（边缘登陆）")]
        [Tooltip("Hop의 Y축 속도")]
        public float ClimbHopY;          //Hop의 Y축 속도
        [Tooltip("Hop의 X축 속도")]
        public float ClimbHopX;           //Hop의 X축 속도
        [Tooltip("홉 타임")]
        public float ClimbHopForceTime;    //홉 타임
        [Tooltip("월부스트 시간")]
        public float ClimbJumpBoostTime;   //월부스트 시간
        [Tooltip("Wind의 경우, Hop은 0.3초 동안 무풍입니다.")]
        public float ClimbHopNoWindTime;   //Wind의 경우, Hop은 0.3초 동안 무풍입니다.

        public float DuckFriction;
        public float DuckSuperJumpXMult;
        public float DuckSuperJumpYMult;

        private Action reloadCallback;
        public void SetReloadCallback(Action onReload)
        {
            this.reloadCallback = onReload;
        }

        public void OnValidate()
        {
            ReloadParams();
        }

        public void ReloadParams()
        {
            Debug.Log("=======모든 Player 설정 매개 변수 업데이트");
            Constants.MaxRun = MaxRun;
            Constants.RunAccel = RunAccel;
            Constants.RunReduce = RunReduce;
            Constants.Gravity = Gravity; //重力
            Constants.HalfGravThreshold = HalfGravThreshold;
            Constants.MaxFall = MaxFall; //普通最大下落速度
            Constants.FastMaxFall = FastMaxFall;  //快速最大下落速度
            Constants.FastMaxAccel = FastMaxAccel; //快速下落加速度

            Constants.UpwardCornerCorrection = UpwardCornerCorrection;

            Constants.JumpSpeed = JumpSpeed;
            Constants.VarJumpTime = VarJumpTime;
            Constants.JumpHBoost = JumpHBoost;
            Constants.JumpGraceTime = JumpGraceTime;

            Constants.DashSpeed = DashSpeed;          //冲刺速度
            Constants.EndDashSpeed = EndDashSpeed;        //结束冲刺速度
            Constants.EndDashUpMult = EndDashUpMult;       //如果向上冲刺，阻力。
            Constants.DashTime = DashTime;            //冲刺时间
            Constants.DashCooldown = DashCooldown;         //冲刺冷却时间，
            Constants.DashRefillCooldown = DashRefillCooldown;   //冲刺重新装填时间
            Constants.DashCornerCorrection = DashCornerCorrection;     //水平Dash时，遇到阻挡物的可纠正像素值
            Constants.MaxDashes = MaxDashes;    // 最大Dash次数

            Constants.ClimbCheckDist = ClimbCheckDist;           //攀爬检查像素值
            Constants.ClimbUpCheckDist = ClimbUpCheckDist;         //向上攀爬检查像素值
            Constants.ClimbNoMoveTime = ClimbNoMoveTime;
            Constants.ClimbUpSpeed = ClimbUpSpeed;        //上爬速度
            Constants.ClimbDownSpeed = ClimbDownSpeed;       //下爬速度
            Constants.ClimbSlipSpeed = ClimbSlipSpeed;       //下滑速度
            Constants.ClimbAccel = ClimbAccel;          //下滑加速度
            Constants.ClimbGrabYMult = ClimbGrabYMult;       //攀爬时抓取导致的Y轴速度衰减
            Constants.ClimbHopY = ClimbHopY;          //Hop的Y轴速度 
            Constants.ClimbHopX = ClimbHopX;           //Hop的X轴速度
            Constants.ClimbHopForceTime = ClimbHopForceTime;    //Hop时间
            Constants.ClimbJumpBoostTime = ClimbJumpBoostTime;   //WallBoost时间
            Constants.ClimbHopNoWindTime = ClimbHopNoWindTime;   //Wind情况下,Hop会无风0.3秒

            Constants.WallJumpHSpeed = MaxRun + JumpHBoost;
            Constants.SuperJumpSpeed = JumpSpeed;
            Constants.SuperWallJumpH = MaxRun + JumpHBoost * 2;

            Constants.DashCornerCorrection = this.DashCornerCorrection;

            Constants.DuckFriction = DuckFriction;
            Constants.DuckSuperJumpXMult = DuckSuperJumpXMult;
            Constants.DuckSuperJumpYMult = DuckSuperJumpYMult;

            Constants.EnableWallSlide = this.EnableWallSlide; //启用墙壁下滑功能
            Constants.EnableJumpGrace = this.EnableJumpGrace; //土狼时间
            Constants.EnableWallBoost = this.EnableWallBoost; //WallBoost

            reloadCallback?.Invoke();
        }
    }
}