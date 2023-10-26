using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEffects : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteEchoRenderer;
    [SerializeField] private Anticipation[] _anticipations;

    private SpriteRenderer _ownSpriteRenderer;
    private Animator _animator;

    private const float _echoBrightness = .6f;
    private const float _echoAlpha = .5f;
    private const float _spriteMaxScale = 1.5f;
    private const float _anticipationCutoffTime = .15f; //스프라이트 에코가 끝나기 몇 초 전에 애니메이션이 재생될 것인지.

    private void Awake()
    {
        _ownSpriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _spriteEchoRenderer.enabled = false;
        _spriteEchoRenderer.color = new Color(_echoBrightness, _echoBrightness, _echoBrightness, _echoAlpha);
    }


    public IEnumerator CR_Anticipation(int index)
    {
        //스프라이트 맞추기
        _spriteEchoRenderer.sprite = _anticipations[index].sprite;
        _spriteEchoRenderer.flipX = _ownSpriteRenderer.flipX;
        _spriteEchoRenderer.enabled = true;

        //크기 애니메이션
        _spriteEchoRenderer.transform.DOScale(_spriteMaxScale, _anticipations[index].time + _anticipationCutoffTime).OnComplete(SpriteEchoEnded);

        //애니메이션 멈췄다 재생
        float originalSpeed = _animator.speed;
        _animator.speed = 0;
        yield return new WaitForSeconds(_anticipations[index].time);
        _animator.speed = originalSpeed;
        _spriteEchoRenderer.transform.DOPunchScale(Vector3.one * .5f, .08f);
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
}
