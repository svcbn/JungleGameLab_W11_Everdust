using DG.Tweening;
using Myd.Platform;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [SerializeField] private SpriteRenderer _spriteEchoRenderer;
    [SerializeField] private Anticipation[] _3hitAnticipations;
    [SerializeField] private Anticipation[] _3hitVer2;

    private SpriteRenderer _ownSpriteRenderer;
    private Animator _animator;
    private UnityEngine.Coroutine _teleportCR;
    private BossState _state = BossState.Peaceful;


    private const float _echoBrightness = .6f;
    private const float _echoAlpha = .5f;
    private const float _spriteMaxScale = 1.5f;
    private const float _anticipationCutoffTime = .15f; //스프라이트 에코가 끝나기 몇 초 전에 애니메이션이 재생될 것인지.

    protected override void Awake()
    {
        base.Awake();
        _ownSpriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();

        _spriteEchoRenderer.enabled = false;
        _spriteEchoRenderer.color = new Color(_echoBrightness, _echoBrightness, _echoBrightness, _echoAlpha);
    }

    protected override void Update()
    {
        base.Update();
        switch (_state)
        {
            case BossState.Peaceful:
                break;
            case BossState.BattleIdle:
                //TODO: 랜덤으로 패턴 실행
                break;
            case BossState.InPattern:
                break;
            default:
                break;
        }
    }

    private void AE_StartRandomPattern()
    {
        int numOfPatterns = 3;
        int index = Random.Range(0, numOfPatterns);
        switch (index)
        {
            case 0:
                _animator.Play("Boss_3Hit");
                break;
            case 1:
                _animator.Play("Boss_3HitVer2");
                break;
            case 2:
                _animator.Play("Boss_Cast");
                break;
        }
    }

    public IEnumerator CR_3HitAnticipation(int index)
    {
        //스프라이트 맞추기
        _spriteEchoRenderer.sprite = _3hitAnticipations[index].sprite;
        _spriteEchoRenderer.flipX = _ownSpriteRenderer.flipX;
        _spriteEchoRenderer.enabled = true;

        //크기 애니메이션
        _spriteEchoRenderer.transform.DOScale(_spriteMaxScale, _3hitAnticipations[index].time + _anticipationCutoffTime).OnComplete(SpriteEchoEnded);

        //순간이동 해야하면 실행
        TeleportInfo tInfo = _3hitAnticipations[index].teleportInfo;
        if (tInfo.duration > 0)
        {
            if (_teleportCR != null) StopCoroutine(_teleportCR);
            _teleportCR = StartCoroutine(CR_Teleport(tInfo.startDelay, tInfo.duration, tInfo.relativePositionFromPlayer, tInfo.shouldFlip));
        }

        //애니메이션 멈췄다 재생
        float originalSpeed = _animator.speed;
        _animator.speed = 0;
        yield return new WaitForSeconds(_3hitAnticipations[index].time);
        _animator.speed = originalSpeed;
        _spriteEchoRenderer.transform.DOPunchScale(Vector3.one * .5f, .08f);
    }

    public IEnumerator CR_3HitVer2(int index)
    {
        //스프라이트 맞추기
        _spriteEchoRenderer.sprite = _3hitVer2[index].sprite;
        _spriteEchoRenderer.flipX = _ownSpriteRenderer.flipX;
        _spriteEchoRenderer.enabled = true;

        //크기 애니메이션
        _spriteEchoRenderer.transform.DOScale(_spriteMaxScale, _3hitVer2[index].time + _anticipationCutoffTime).OnComplete(SpriteEchoEnded);

        //순간이동 해야하면 실행
        TeleportInfo tInfo = _3hitVer2[index].teleportInfo;
        if (tInfo.duration > 0)
        {
            if (_teleportCR != null) StopCoroutine(_teleportCR);
            _teleportCR = StartCoroutine(CR_Teleport(tInfo.startDelay, tInfo.duration, tInfo.relativePositionFromPlayer, tInfo.shouldFlip));
        }

        //애니메이션 멈췄다 재생
        float originalSpeed = _animator.speed;
        _animator.speed = 0;
        yield return new WaitForSeconds(_3hitVer2[index].time);
        _animator.speed = originalSpeed;
        _spriteEchoRenderer.transform.DOPunchScale(Vector3.one * .5f, .08f);
    }

    public IEnumerator CR_Teleport(float startDelay, float duration, Vector2 targetPosition, bool shouldFlip)
    {
        yield return new WaitForSeconds(startDelay);

        //순간이동 사라지는 이펙트
        float timer = 0;
        transform.DOScaleY(1.3f, duration);
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float fadeAmount = timer / duration;
            _ownSpriteRenderer.sharedMaterial.SetFloat("_FadeAmount", fadeAmount);
            yield return null;
        }

        //실제 포지션 이동
        Vector2 teleportOffset = _ownSpriteRenderer.flipX ? targetPosition : new Vector2 (-1 * targetPosition.x, targetPosition.y);
        transform.position = (Vector2) FindObjectOfType<PlayerRenderer>().transform.position + teleportOffset;
        if (shouldFlip) _ownSpriteRenderer.flipX = !_ownSpriteRenderer.flipX;
        _spriteEchoRenderer.flipX = _ownSpriteRenderer.flipX;

        //순간이동 생기는 이펙트
        transform.DOScaleY(1f, duration);
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            float fadeAmount = timer / duration;
            _ownSpriteRenderer.sharedMaterial.SetFloat("_FadeAmount", fadeAmount);
            yield return null;
        }
        _ownSpriteRenderer.sharedMaterial.SetFloat("_FadeAmount", -1);
    }

    public IEnumerator CR_Cast1()
    {
        //투사체 4개 소환
        //애니메이션 멈추기
        float originalAnimSpd = _animator.speed;
        _animator.speed = 0f;
        yield return new WaitForSeconds(4f);
        _animator.speed = originalAnimSpd;
    }

    private void SpriteEchoEnded()
    {
        _spriteEchoRenderer.enabled = false;
        _spriteEchoRenderer.transform.DOScale(1, 0.01f);
    }
}

[System.Serializable]
public struct Anticipation
{
    public Sprite sprite;
    public float time;
    public TeleportInfo teleportInfo;
}

[System.Serializable]
public struct TeleportInfo
{
    public float startDelay;
    public float duration;
    [Tooltip("보스가 왼쪽을 보고 있을 때 기준, 플레이어 대비 어느 위치로 순간이동 해야하는가.")] public Vector2 relativePositionFromPlayer;
    public bool shouldFlip;
}

public enum BossState
{
    Peaceful, BattleIdle, InPattern
}