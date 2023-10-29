using DG.Tweening;
using Myd.Platform;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Boss : Enemy
{
    [SerializeField] private SpriteRenderer spriteEchoRenderer;
    [SerializeField] private Anticipation[] _3hitAnticipations;
    [SerializeField] private Anticipation[] _3hitVer2;
    [SerializeField] private EnemyMeleeHitBox[] _hitBoxes = new EnemyMeleeHitBox[3];
    [SerializeField] private GameObject _magicCircle;
    [SerializeField] private Transform _magicCirclePreview;
    [SerializeField] private ParticleSystem _magicCircleExplosion;
    [SerializeField] private GameObject _stunStar;

    private SpriteRenderer _ownSpriteRenderer;
    private Animator _animator;
    private UnityEngine.Coroutine _teleportCR;
    private bool _canFlip = true;
    private Transform _player;
    private ProjectileManager _projManager;
    private UnityEngine.Coroutine _magicCircleCR;
    private HandleLaserPattern _handleLaserPattern;

    public Transform Player
    {
        get
        {
            if (_player is not null) return _player;
            if (FindObjectOfType<PlayerRenderer>() is PlayerRenderer p) _player = p.transform;
            return _player;
        }
    }

    private const float EchoBrightness = .6f;
    private const float EchoAlpha = .5f;
    private const float SpriteMaxScale = 1.5f;
    private const float AnticipationCutoffTime = .15f; //스프라이트 에코가 끝나기 몇 초 전에 애니메이션이 재생될 것인지.
    private const float MagicCircleTime = 7f;

    protected override void Awake()
    {
        base.Awake();
        _ownSpriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _projManager = FindObjectOfType<ProjectileManager>();
        _handleLaserPattern = FindObjectOfType<HandleLaserPattern>();

        spriteEchoRenderer.enabled = false;
        spriteEchoRenderer.color = new Color(EchoBrightness, EchoBrightness, EchoBrightness, EchoAlpha);
        _magicCircle.SetActive(false);
        _magicCirclePreview.localScale = Vector3.zero;
    }

    protected override void Update()
    {
        base.Update();

        if (!_canFlip) return;
        if (Player is null) return;
        
        _ownSpriteRenderer.flipX = Player.position.x < transform.position.x;
    }

    private void AE_StartRandomPattern()
    {
        _stunStar.SetActive(false);
        const int numOfPatterns = 5;
        var index = Random.Range(0, numOfPatterns);
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
            case 3:
                _animator.Play("Boss_Cast 2");
                break;
            case 4:
                _animator.Play("Boss_Laser");
                break;
        }
    }
    
    public IEnumerator CR_3HitAnticipation(int index)
    {
        //
        spriteEchoRenderer.sprite = _3hitAnticipations[index].sprite;
        spriteEchoRenderer.flipX = _ownSpriteRenderer.flipX;
        spriteEchoRenderer.enabled = true;

        //
        spriteEchoRenderer.transform.DOScale(SpriteMaxScale, _3hitAnticipations[index].time + AnticipationCutoffTime).OnComplete(SpriteEchoEnded);

        //
        TeleportInfo tInfo = _3hitAnticipations[index].teleportInfo;
        if (tInfo.duration > 0)
        {
            if (_teleportCR != null) StopCoroutine(_teleportCR);
            _teleportCR = StartCoroutine(CR_Teleport(tInfo.startDelay, tInfo.duration, tInfo.relativePositionFromPlayer, tInfo.shouldFlip));
        }

        //
        _hitBoxes[index].ActivateHitBox(_3hitAnticipations[index].time + _3hitAnticipations[index].hitboxDelayOffset);

        //
        float originalSpeed = _animator.speed;
        _animator.speed = 0;
        yield return new WaitForSeconds(_3hitAnticipations[index].time);
        _animator.speed = originalSpeed;
        spriteEchoRenderer.transform.DOPunchScale(Vector3.one * .5f, .08f);
    }

    public IEnumerator CR_3HitVer2(int index)
    {
        //
        spriteEchoRenderer.sprite = _3hitVer2[index].sprite;
        spriteEchoRenderer.flipX = _ownSpriteRenderer.flipX;
        spriteEchoRenderer.enabled = true;

        //
        spriteEchoRenderer.transform.DOScale(SpriteMaxScale, _3hitVer2[index].time + AnticipationCutoffTime).OnComplete(SpriteEchoEnded);

        //
        TeleportInfo tInfo = _3hitVer2[index].teleportInfo;
        if (tInfo.duration > 0)
        {
            if (_teleportCR != null) StopCoroutine(_teleportCR);
            _teleportCR = StartCoroutine(CR_Teleport(tInfo.startDelay, tInfo.duration, tInfo.relativePositionFromPlayer, tInfo.shouldFlip));
        }

        //
        int hitBoxIndex = index == 2 ? 0 : index;
        _hitBoxes[hitBoxIndex].ActivateHitBox(_3hitVer2[index].time + _3hitVer2[index].hitboxDelayOffset);

        //
        float originalSpeed = _animator.speed;
        _animator.speed = 0;
        yield return new WaitForSeconds(_3hitVer2[index].time);
        _animator.speed = originalSpeed;
        spriteEchoRenderer.transform.DOPunchScale(Vector3.one * .5f, .08f);
    }

    public IEnumerator CR_Teleport(float startDelay, float duration, Vector2 targetPosition, bool shouldFlip, bool isRelativePosition = true)
    {
        yield return new WaitForSeconds(startDelay);

        //순간이동 사라지는 효과.
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
        Vector2 teleportOffset = _ownSpriteRenderer.flipX ? targetPosition : new Vector2(-1 * targetPosition.x, targetPosition.y);
        teleportOffset = isRelativePosition ? (Vector2)Player.position + teleportOffset : targetPosition;
        transform.position = teleportOffset;
        if (shouldFlip) _ownSpriteRenderer.flipX = !_ownSpriteRenderer.flipX;
        spriteEchoRenderer.flipX = _ownSpriteRenderer.flipX;

        //순간이동 나타나는 효과.
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
        //
        _projManager.Start4ProjAttack();
        //

        float originalAnimSpd = _animator.speed;
        _animator.speed = 0f;
        //플레이어 속박시킴
        PlayerManager.Instance.ShowText("속박됨!");
        PlayerManager.Instance.SetPlayerBond(3f);
        yield return new WaitForSeconds(4f);
        _animator.speed = originalAnimSpd;
    }

    public void StartMagicCirclePattern()
    {
        if (_magicCircleCR != null) StopCoroutine(_magicCircleCR);
        _magicCircleCR = StartCoroutine(CR_Cast2_NotAE());
    }
    
    public void AE_StartLaserPattern()
    {
        var facingLeft = Player.position.x < _handleLaserPattern.transform.position.x;
        _handleLaserPattern.StartLaserPattern(facingLeft);
    }

    public IEnumerator CR_Cast2_NotAE()
    {
        float _timer = 0f;
        //애니메이션 멈추기
        float originalSpeed = _animator.speed;
        _animator.speed = 0;
        
        GetComponent<HandleWeaknessCircle>().weaknessCircle.SetActive(true);
        
        //최종 범위 표시
        _magicCircle.SetActive(true);
        //진행도 표시
        while (_timer < MagicCircleTime)
        {
            _magicCirclePreview.localScale = Vector3.one * (_timer / MagicCircleTime) * 3.18f;
            _timer += Time.deltaTime;
            yield return null;
        }
        //공격 이펙트
        _magicCircleExplosion.Clear();
        _magicCircleExplosion.Play();
        //실제 데미지 적용
        if (_magicCircle.GetComponent<Collider2D>().IsTouching(Player.GetComponent<Collider2D>()))
        {
            Player.GetComponent<PlayerStats>().Hit(40);
        }
        //매직서클 효과 전부 비활성화
        _magicCircle.SetActive(false);
        _magicCirclePreview.localScale = Vector3.zero;

        //TODO: 약점 비활성화 및 초기화
        GetComponent<HandleWeaknessCircle>().weaknessCircle.SetActive(false);
        GetComponent<HandleWeaknessCircle>().ResetWeaknessPosition();

        //애니메이션 재생
        _animator.speed = originalSpeed;
        spriteEchoRenderer.transform.DOPunchScale(Vector3.one * .5f, .08f);
    }

    private void AE_TeleportToCenter()
    {
        Vector2 center = new(-24.8f, 0f);
        StartCoroutine(CR_Teleport(0, .2f, center, false, false));
    }

    public void CancelMagicCircle()
    {
        GetComponent<HandleWeaknessCircle>().weaknessCircle.SetActive(false);
        GetComponent<HandleWeaknessCircle>().ResetWeaknessPosition();
        //코루틴 정지
        if (_magicCircleCR != null) StopCoroutine(_magicCircleCR);
        //표시 효과들 끄기
        _magicCircle.SetActive(false);
        _magicCirclePreview.localScale = Vector3.zero;
        _stunStar.SetActive(true);
        _animator.speed = 1f;
    }

    private void SpriteEchoEnded()
    {
        spriteEchoRenderer.enabled = false;
        spriteEchoRenderer.transform.DOScale(1, 0.01f);
    }

    private void AE_CanFlip()
    {
        _canFlip = true;
    }

    private void AE_CanNotFlip()
    {
        _canFlip = false;
    }
    
    public bool GetCurrentFlipXIsTurnedOn()
    {
        return spriteEchoRenderer.flipX;
    }
}

[System.Serializable]
public struct Anticipation
{
    public Sprite sprite;
    public float time;
    public float hitboxDelayOffset;
    public int damage;
    public TeleportInfo teleportInfo;
}

[System.Serializable]
public struct TeleportInfo
{
    public float startDelay;
    public float duration;
    [Tooltip("플레이어 위치 기준 순간이동 대상 로컬 포지션.")] public Vector2 relativePositionFromPlayer;
    public bool shouldFlip;
}