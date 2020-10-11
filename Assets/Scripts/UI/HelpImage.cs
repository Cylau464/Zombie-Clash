using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpImage : MonoBehaviour
{
    [SerializeField] private RectTransform _cursor = null;
    [SerializeField] private CanvasGroup _canvasGroup = null;
    [SerializeField] private float _cursorSpeed = 1f;
    [SerializeField] private float _moveDistance = 1f;
    [SerializeField] private float _acitvationDelay = 3f;
    private Coroutine _activateCoroutine;
    private Coroutine _deactivateCoroutine;

    private void Update()
    {
        Vector3 pos = _cursor.localPosition;
        pos.x = Mathf.PingPong(Time.time * _cursorSpeed, _moveDistance * 2f) - _moveDistance;
        _cursor.localPosition = pos;

        if (InputController.moveDirection == Vector3.zero)
        {
            if (_activateCoroutine == null && InputController.movementAvailable)
                _activateCoroutine = StartCoroutine(Activate());
        }
        else
        {
            if (_deactivateCoroutine == null)
                _deactivateCoroutine = StartCoroutine(Deactivate());
        }
    }

    private IEnumerator Activate()
    {
        yield return new WaitForSeconds(_acitvationDelay);

        if (_deactivateCoroutine != null)
        {
            StopCoroutine(_deactivateCoroutine);
            _deactivateCoroutine = null;
        }

        while (_canvasGroup.alpha < 1f)
        {
            _canvasGroup.alpha += Time.deltaTime * 2f;
            yield return new WaitForEndOfFrame();

            if (InputController.movementAvailable == false)
            {
                _deactivateCoroutine = StartCoroutine(Deactivate());
                yield break;
            }
        }
    }

    private IEnumerator Deactivate()
    {
        if (_activateCoroutine != null)
        {
            StopCoroutine(_activateCoroutine);
            _activateCoroutine = null;
        }

        while (_canvasGroup.alpha > 0f)
        {
            _canvasGroup.alpha -= Time.deltaTime * 2f;
            yield return new WaitForEndOfFrame();
        }

        _deactivateCoroutine = null;
    }
}
