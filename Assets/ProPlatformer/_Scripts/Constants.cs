using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myd.Platform
{
    //여기서 좌표에 관한 수치는 시간 유형을 제외하고/10을 필요로 한다
    public static class Constants
    {

        public static bool EnableWallSlide = true;
        public static bool EnableJumpGrace = true;
        public static bool EnableWallBoost = true;

        public static float Gravity = 90f; //중력

        public static float HalfGravThreshold = 4f; //체공 시간 임계값
        public static float MaxFall = -16; //보통최대낙하속도
        public static float FastMaxFall = -24f;  //고속 최대 낙하 속도
        public static float FastMaxAccel = 30f; //급강하 가속도
        //최대 이동 속도
        public static float MaxRun = 9f;
        //Hold상황에서의 최대 이동 속도
        public static float HoldingMaxRun = 7f;
        //공기 저항
        public static float AirMult = 0.65f;
        //이동 가속도
        public static float RunAccel = 100f;
        //이동감속도
        public static float RunReduce = 40f;
        //
        public static float JumpSpeed = 10.5f;  //최대 점프 속도
        public static float VarJumpTime = 0.2f; //점프 지속 시간(점프 시 점프 버튼[VarJumpTime]초간 계속 반응하여 점프 최고 높이에 영향을 미칩니다)
        public static float JumpHBoost = 4f; //벽에서 물러나는 힘
        public static float JumpGraceTime = 0.1f;//코요테 시간

        #region WallJump
        public static float WallJumpCheckDist = 0.3f;
        public static float WallJumpForceTime = .16f; //벽 점프 강제 시간
        public static float WallJumpHSpeed = MaxRun + JumpHBoost;

        #endregion

        #region SuperWallJump
        public static float SuperJumpSpeed = JumpSpeed;
        public static float SuperJumpH = 26f;
        public static float SuperWallJumpSpeed = 16f;
        public static float SuperWallJumpVarTime = .25f;
        public static float SuperWallJumpForceTime = .2f;
        public static float SuperWallJumpH = MaxRun + JumpHBoost* 2;
        #endregion
        #region WallSlide
        public static float WallSpeedRetentionTime = .06f; //벽에 부딪힌 후 허용 가능한 속도 유지 시간
        public static float WallSlideTime = 1.2f; //벽 활주 시간
        public static float WallSlideStartMax = -2f;


        #endregion

        #region Dash相关参数
        public static float DashSpeed = 24f;           //스퍼트 속도
        public static float EndDashSpeed = 16f;        //종료 스퍼트 속도
        public static float EndDashUpMult = .75f;       //위로 스퍼트하면 저항.
        public static float DashTime = .15f;            //스퍼트 타임
        public static float DashCooldown = 2f;         //스퍼트 냉각 시간,
        public static float DashRefillCooldown = .1f;   //스퍼트 재장전 시간
        public static int DashHJumpThruNudge = 6;       //
        public static int DashCornerCorrection = 4;     //수평 Dash에서 가로 막힘을 만나면 픽셀 값을 수정합니다.
        public static int DashVFloorSnapDist = 3;       //DashAttacking에서의 지면 흡착 픽셀 값
        public static float DashAttackTime = .3f;       //
        public static int MaxDashes = 1;
        #endregion

        #region Climb参数
        public static float ClimbMaxStamina = 110;       //최대 지구력
        public static float ClimbUpCost = 100 / 2.2f;   //위로 기어올라 지구력이 소모되다
        public static float ClimbStillCost = 100 / 10f; //기어다니며 근력이 소모되다
        public static float ClimbJumpCost = 110 / 4f;   //기어오르며 지구력을 소모하다
        public static int ClimbCheckDist = 2;           //등반 검사 픽셀 값
        public static int ClimbUpCheckDist = 2;         //픽셀 값을 검사하기 위해 위로 올라가기
        public static float ClimbNoMoveTime = .1f;
        public static float ClimbTiredThreshold = 20f;  //피로를 나타내는 한계치
        public static float ClimbUpSpeed = 4.5f;        //기어오르는 속도
        public static float ClimbDownSpeed = -8f;       //하강 속도
        public static float ClimbSlipSpeed = -3f;       //하강 속도
        public static float ClimbAccel = 90f;           //하강 가속도
        public static float ClimbGrabYMult = .2f;       //등반시 잡음에 의한 Y축 속도 감쇠
        public static float ClimbHopY = 12f;            //Hop의 Y축 속도
        public static float ClimbHopX = 10f;            //Hop의 X축 속도
        public static float ClimbHopForceTime = .2f;    //홉 타임
        public static float ClimbJumpBoostTime = .2f;   //월부스트 시간
        public static float ClimbHopNoWindTime = .3f;   //Wind의 경우, Hop은 0.3초 동안 무풍입니다.
        #endregion

        #region Duck参数
        public static float DuckFriction = 50f;
        public static float DuckSuperJumpXMult = 1.25f;
        public static float DuckSuperJumpYMult = .5f;
        #endregion

        #region Corner Correct
        public static int UpwardCornerCorrection = 4; //위로 이동, X축 상단의 가장자리를 보정할 수 있는 최대 거리
        #endregion

        public static float LaunchedMinSpeedSq = 196;
    }
}
