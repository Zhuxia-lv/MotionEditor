//https://caitsithware.com/wordpress/archives/1317
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class ForceRaycaster : BaseRaycaster {
    public override Camera eventCamera {
        get {
            return null;
        }
    }

    public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList) {
        RaycastResult result = new RaycastResult();
        result.distance = 0.0f;
        result.gameObject = gameObject;
        result.index = 0;
        result.module = this;

        resultAppendList.Add(result);
    }
}
