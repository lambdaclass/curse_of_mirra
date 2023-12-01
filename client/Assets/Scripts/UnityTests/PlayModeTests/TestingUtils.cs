using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class TestingUtils
{
    public static IEnumerator ForceClick(string elementName)
    {
        GameObject gameObjectToClick = GameObject.Find(elementName);
        var pointer = new PointerEventData(EventSystem.current);

        ExecuteEvents.Execute(gameObjectToClick, pointer, ExecuteEvents.pointerEnterHandler);
        ExecuteEvents.Execute(gameObjectToClick, pointer, ExecuteEvents.pointerDownHandler);
        yield return new WaitForSeconds(0.1f);
        ExecuteEvents.Execute(gameObjectToClick, pointer, ExecuteEvents.pointerUpHandler);
        ExecuteEvents.Execute(gameObjectToClick, pointer, ExecuteEvents.pointerClickHandler);
    }
}
