using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotateUIModel : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField]
    float rotationSpeed = 1f;

    [SerializeField]
    GameObject modelContainer;
    Transform model;
    Touch touch;
    Quaternion rotationX;
    Coroutine coroutine;

    void Awake()
    {
        StartCoroutine(GetModel());
    }

    IEnumerator GetModel()
    {
        yield return new WaitUntil(() => modelContainer.GetComponentInChildren<Animator>() != null);
        model = modelContainer.GetComponentInChildren<Animator>().transform.parent.transform;
    }

    public void OnDrag(PointerEventData eventData)
    {
        touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Moved)
        {
            rotationX = Quaternion.Euler(0f, -touch.deltaPosition.x * rotationSpeed, 0f);
            model.rotation *= rotationX;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        coroutine = StartCoroutine(ResetModelRotation());
    }

    IEnumerator ResetModelRotation()
    {
        yield return new WaitForSeconds(2f);
        model.DORotate(Vector3.zero, 1f, RotateMode.Fast).SetEase(Ease.OutQuint);
    }
}
