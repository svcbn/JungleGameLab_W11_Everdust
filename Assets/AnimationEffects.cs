using AllIn1SpriteShader;
using DG.Tweening;
using Myd.Platform;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEffects : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteEchoRenderer;
    [SerializeField] private Anticipation[] _anticipations;

    private SpriteRenderer _ownSpriteRenderer;
    private Animator _animator;
    private AllIn1Shader _allIn1Shader;

    private const float _echoBrightness = .6f;
    private const float _echoAlpha = .5f;
    private const float _spriteMaxScale = 1.5f;
    private const float _anticipationCutoffTime = .15f; //��������Ʈ ���ڰ� ������ �� �� ���� �ִϸ��̼��� ����� ������.
    private const float _teleportDuration = .2f;

    private void Awake()
    {
        _ownSpriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _allIn1Shader = GetComponent<AllIn1Shader>();

        _spriteEchoRenderer.enabled = false;
        _spriteEchoRenderer.color = new Color(_echoBrightness, _echoBrightness, _echoBrightness, _echoAlpha);
    }

    public IEnumerator CR_Anticipation(int index)
    {
        //��������Ʈ ���߱�
        _spriteEchoRenderer.sprite = _anticipations[index].sprite;
        _spriteEchoRenderer.flipX = _ownSpriteRenderer.flipX;
        _spriteEchoRenderer.enabled = true;

        //ũ�� �ִϸ��̼�
        _spriteEchoRenderer.transform.DOScale(_spriteMaxScale, _anticipations[index].time + _anticipationCutoffTime).OnComplete(SpriteEchoEnded);

        //�����̵� �ؾ��ϸ� ����
        TeleportInfo tInfo = _anticipations[index].teleportInfo;
        if (tInfo.duration > 0)
        {
            StartCoroutine(CR_Teleport(tInfo.startDelay, tInfo.duration, tInfo.relativePositionFromPlayer));
        }

        //�ִϸ��̼� ����� ���
        float originalSpeed = _animator.speed;
        _animator.speed = 0;
        yield return new WaitForSeconds(_anticipations[index].time);
        _animator.speed = originalSpeed;
        _spriteEchoRenderer.transform.DOPunchScale(Vector3.one * .5f, .08f);
    }

    public IEnumerator CR_Teleport(float startDelay, float duration, Vector2 targetPosition)
    {
        yield return new WaitForSeconds(startDelay);

        //�����̵� ������� ����Ʈ
        float timer = 0;
        transform.DOScaleY(1.3f, _teleportDuration);
        while (timer < _teleportDuration)
        {
            timer += Time.deltaTime;
            float fadeAmount = timer / _teleportDuration;
            _ownSpriteRenderer.sharedMaterial.SetFloat("_FadeAmount", fadeAmount);
            yield return null;
        }

        //���� ������ �̵�
        transform.position = (Vector2) FindObjectOfType<PlayerRenderer>().transform.position + targetPosition;

        //�����̵� ����� ����Ʈ
        transform.DOScaleY(1f, _teleportDuration);
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            float fadeAmount = timer / _teleportDuration;
            _ownSpriteRenderer.sharedMaterial.SetFloat("_FadeAmount", fadeAmount);
            yield return null;
        }
    }

    private void SizeRevert()
    {
        transform.localScale = Vector3.one;
    }

    private void SpriteEchoEnded()
    {
        _spriteEchoRenderer.enabled = false;
        _spriteEchoRenderer.transform.DOScale(1, 0.01f);
        //_spriteEchoRenderer.transform.localScale = Vector3.one;
        
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
    public Vector2 relativePositionFromPlayer;
}