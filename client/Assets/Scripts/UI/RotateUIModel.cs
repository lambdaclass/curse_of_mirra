using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotateUIModel : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField]
    float rotationSpeed = 0.5f;

    [SerializeField]
    GameObject modelContainer;
    Transform model;
    Touch touch;
    Vector2 touchPosition;
    Quaternion rotationX;
    bool restRotation;
    Vector2 rotationValue;

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
        if (Input.touchCount > 0 && modelContainer.GetComponentInChildren<Animator>() != null)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                rotationX = Quaternion.Euler(0f, -touch.deltaPosition.x * rotationSpeed, 0f);
                model.rotation *= rotationX;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        restRotation = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        restRotation = true;
        StartCoroutine(ResetModelRotation());
    }

    IEnumerator ResetModelRotation()
    {
        yield return new WaitForSeconds(3f);
        if (restRotation)
        {
            model.DORotate(Vector3.zero, .5f, RotateMode.Fast);
        }
    }
}
