using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CameraContoroller : ForceRaycaster, IPointerDownHandler, IPointerUpHandler {

    [SerializeField] int m_Restriction = 5;
    [SerializeField] int m_Coefficient = 5;
    [SerializeField] GameObject target;
    [SerializeField] Button m_resetButton;
    private bool isPointerDown;
    private Vector3 mousePos = new Vector3();
    private Vector3 defoultPos;
    private Vector3 defoultRot;

    void Start()
    {
        defoultPos = gameObject.transform.localPosition;
        defoultRot = gameObject.transform.localEulerAngles;
        
        m_resetButton.onClick.AddListener(ResetCameraPosition);
    }

    private void Update() {
        if (isPointerDown) {
            var diff = mousePos - Input.mousePosition;
            if (diff.x > m_Restriction || diff.x < -m_Restriction) {
                transform.RotateAround(target.transform.position, -transform.up, diff.x * Time.deltaTime);
            }
            if (diff.y > m_Restriction || diff.y < -m_Restriction) {
                transform.RotateAround(target.transform.position, transform.right, diff.y * Time.deltaTime);
            }
        }
        var scroll = Input.GetAxis("Mouse ScrollWheel");
        transform.position += transform.forward * scroll * m_Coefficient;
    }

    private void ResetCameraPosition() {
        gameObject.transform.localPosition = defoultPos;
        gameObject.transform.localEulerAngles = defoultRot;
    }

    public void OnPointerDown(PointerEventData eventData) {
        mousePos = Input.mousePosition;
        isPointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData) {
        isPointerDown = false;
    }
}
