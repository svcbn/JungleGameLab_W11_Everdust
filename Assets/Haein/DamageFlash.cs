using AllIn1SpriteShader;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Color _flashColor = Color.white;
    [SerializeField] private float _flashTime = .25f;

    private SpriteRenderer[] _spriteRenderers;
    private Material[] _materials;
    private bool _useAll1Shader = false;

    private Coroutine _all1ShaderFlashCR;
    private Coroutine _damageFlashCoroutine;
    
    private void Awake()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        Init();
    }

    private void Init()
    {
        _materials = new Material[_spriteRenderers.Length];
        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            _materials[i] = _spriteRenderers[i].material;
        }

        if (GetComponentInChildren<AllIn1Shader>() != null) _useAll1Shader = true;
    }

    public void CallDamageFlash()
    {
        if (_useAll1Shader)
        {
            if (_all1ShaderFlashCR != null) { StopCoroutine(_all1ShaderFlashCR); }
            _all1ShaderFlashCR = StartCoroutine(CR_All1ShaderFlasher());
        }
        else
        {
            if (_damageFlashCoroutine != null) { StopCoroutine(_damageFlashCoroutine); }
            _damageFlashCoroutine = StartCoroutine(DamageFlasher());
        }
    }

    private IEnumerator DamageFlasher()
    {
        SetFlashColor();
        float currentFlashAmount = 0f;
        float elapsedTime = 0f;
        while (elapsedTime < _flashTime)
        {
            elapsedTime += Time.deltaTime;
            currentFlashAmount = Mathf.Lerp(1f, 0f, elapsedTime / _flashTime);
            SetFlashAmount(currentFlashAmount);
            yield return null;
        }
    }

    private IEnumerator CR_All1ShaderFlasher()
    {
        float currentFlashAmount = 0f;
        float elapsedTime = 0f;
        while (elapsedTime < _flashTime)
        {
            elapsedTime += Time.deltaTime;
            currentFlashAmount = Mathf.Lerp(1f, 0f, elapsedTime / _flashTime);
            foreach (var mat in _materials)
            {
                mat.SetFloat("_HitEffectBlend", currentFlashAmount);
            }
            yield return null;
        }
    }

    private void SetFlashColor()
    {
        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i].SetColor("_FlashColor", _flashColor);
        }
    }

    private void SetFlashAmount(float amount)
    {
        for (int i=0; i<_materials.Length; i++)
        {
            _materials[i].SetFloat("_FlashAmount", amount);
        }
    }
}
