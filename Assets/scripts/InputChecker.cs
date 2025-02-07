using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class InputChecker
{
    public static bool IsTouchBegan()
    {
        if (Time.timeScale == 1f)
            return (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began));
        return false;
    }
}

