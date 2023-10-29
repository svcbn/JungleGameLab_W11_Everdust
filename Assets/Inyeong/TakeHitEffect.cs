using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TakeHitEffect : MonoBehaviour
{
    // 싱글톤 대신 static 쓰기 위해, 이러한 방법 사용.

    // 인스펙터에서 수정할 값.
    [SerializeField] private Image redEffectImage; // 화면 꽉 채운 이미지 컴포넌트. (검은색.)
    [SerializeField] private float time = 1.0f;

    // 실제 Static 메소드에서 사용할 값.
    static private Image RedEffectImage; 
    static private float Time;
    
    static Sequence sequenceFadeInOut;


    private void Awake()
    {
        RedEffectImage = redEffectImage;
        Time = time;

        RedEffectImage.enabled = false;
    }

    private void Start()
    {
        // 구성.
        sequenceFadeInOut = DOTween.Sequence()
            .SetAutoKill(false) // DoTween Sequence는 기본적으로 일회용임. 재사용하려면 써주자.
            .OnRewind(() => // 실행 전. OnStart는 unity Start 함수가 불릴 때 호출됨. 낚이지 말자.
            {
                RedEffectImage.enabled = true;
            })
            .Append(RedEffectImage.DOFade(0.5f, 0f))
            .Append(RedEffectImage.DOFade(0.0f, Time))
            .OnComplete(() => // 실행 후.
            {
                RedEffectImage.enabled = false;
            });
    }

    static public void Play()
    {
        sequenceFadeInOut.Restart(); // Play()로 하면, 한번 밖에 실행 안 됨.
    }

}