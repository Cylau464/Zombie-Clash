using UnityEngine;
using System.Collections;

public class Spikes : Trap
{
    [SerializeField] private float _activateTime = .2f;
    [SerializeField] private float _activeDuration = 1f;
    [SerializeField] private float _deactiveDuration = 2f;
    [SerializeField] private float _spikesSize = 1f;
    [SerializeField] private Transform _spikes = null;
    private Vector3 _startPos;
    private bool _active = true;

    private void Start()
    {
        _startPos = _spikes.localPosition;
        StartCoroutine(Activation(_activeDuration));
    }

    private IEnumerator Activation(float delay)
    {
        yield return new WaitForSeconds(delay);

        Vector3 _targetPos = _startPos;

        if (_active)
        {
            _targetPos.y = _startPos.y - _spikesSize;
            _active = false;
        }
        else
        {
            _active = true;
        }

        while (Vector3.Distance(_spikes.localPosition, _targetPos) > float.Epsilon)
        {
            _spikes.localPosition = Vector3.MoveTowards(_spikes.localPosition, _targetPos, Time.deltaTime / _activateTime * _spikesSize);
            yield return new WaitForEndOfFrame();
        }

        if (_active)
            StartCoroutine(Activation(_activeDuration));
        else
            StartCoroutine(Activation(_deactiveDuration));
    }
}