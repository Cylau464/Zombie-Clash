using UnityEngine;
using System.Collections;
using System.Configuration;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform _rotationPivot = null;
    [SerializeField] private float _rotationSpeed = 2f;
    [SerializeField] private float _rotationAngle = 90f;
    [SerializeField] private LayerMask _friendlyLayer = 0;
    private bool isDestroyable;
    private bool isOpened;

    [SerializeField] private AudioClip _destroyClip = null;
    [SerializeField] private GameObject _destroyParticle = null;

    void Start()
    {
        FightStage.fightStart.AddListener(OpenDoor);
        FightStage.fightEnd.AddListener(SetDestroyable);
    }

    private void OpenDoor()
    {
        StartCoroutine(RotateDoor(true));
        isOpened = true;
    }

    private void CloseDoor()
    {
        StartCoroutine(RotateDoor(false));
        isOpened = false;
    }

    private void SetDestroyable()
    {
        isDestroyable = true;
    }

    private IEnumerator RotateDoor(bool open)
    {
        int direction = open ? -1 : 1;
        float duration = Time.time + 1f / _rotationSpeed;

        while (duration > Time.time)
        {
            _rotationPivot.Rotate(Vector3.up * direction, _rotationAngle * Time.deltaTime * _rotationSpeed, Space.Self);
            yield return new WaitForEndOfFrame();
        }
    }

    public void DestroyDoor()
    {
        Instantiate(_destroyParticle, transform.position, _destroyParticle.transform.rotation);
        AudioManager.PlayClipAtPosition(_destroyClip, transform.position, .5f);
        CastleSiege.siegeStart.Invoke();
        Destroy(gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Defender" && isOpened)
        {
            CancelInvoke(nameof(CloseDoor));
            Invoke(nameof(CloseDoor), .5f);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer == Mathf.Log(_friendlyLayer.value, 2) && isDestroyable)
        {
            DestroyDoor();
        }
    }
}