using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowingAltarRunes : MonoBehaviour
{
    public List<SpriteRenderer> runes;
    public float lerpSpeed;
    
    private Color _curColor;

    private Color _visible;
    private Color _invisible;
    
    // 부하를 줄이기 위한 코루틴
    private Coroutine _currentCoroutine;

    private void Start()
    {
        _visible = new Color(1, 1, 1, 1);
        _invisible = new Color(1, 1, 1, 0);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 코루틴이 실행 중이면 종료
        if(_currentCoroutine != null)
            StopCoroutine(_currentCoroutine);
        
        _currentCoroutine = StartCoroutine(SmoothColorTransition(_visible, lerpSpeed));
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // 코루틴이 실행 중이면 종료
        if(_currentCoroutine != null)
            StopCoroutine(_currentCoroutine);
        
        _currentCoroutine = StartCoroutine(SmoothColorTransition(_invisible, lerpSpeed));
    }
    
    private IEnumerator SmoothColorTransition(Color target, float duration)
    {
        var elapsed = 0f;
        var startColor = _curColor;

        while (elapsed < duration)
        {
            _curColor = Color.Lerp(startColor, target, elapsed / duration);

            foreach (var r in runes)
            {
                r.color = _curColor;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        _curColor = target;
    }
}
