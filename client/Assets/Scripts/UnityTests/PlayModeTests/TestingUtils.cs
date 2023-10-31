using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class TestingUtils
{
    public static IEnumerator CoClickButton(GameObject go)
    {
        // simulate a button click  
        var pointer = new PointerEventData(EventSystem.current);

        ExecuteEvents.Execute(go, pointer, ExecuteEvents.pointerEnterHandler);
        ExecuteEvents.Execute(go, pointer, ExecuteEvents.pointerDownHandler);
        yield return new WaitForSeconds(0.1f);
        ExecuteEvents.Execute(go, pointer, ExecuteEvents.pointerUpHandler);
        ExecuteEvents.Execute(go, pointer, ExecuteEvents.pointerClickHandler);
    }
}
