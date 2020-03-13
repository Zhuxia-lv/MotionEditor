using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlideInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    [SerializeField] int m_Restriction = 5;
    [SerializeField] int m_Coefficient = 100;
    [SerializeField] int m_Max = 10;

    private Vector3 mousePos = new Vector3();
    private InputField m_input;
    private bool isPointerDown;

    private void Awake() {
        m_input = GetComponent<InputField>();
    }

    private void Update() {
        if (isPointerDown) {
            var diff = mousePos.x - Input.mousePosition.x;
            if (Mathf.Abs(diff) > m_Restriction) {
                var f = float.Parse(m_input.text) - Mathf.Clamp((float)1 / m_Coefficient * diff, -m_Max, m_Max);
                m_input.text = f.ToString();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        mousePos = Input.mousePosition;
        isPointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData) {
        isPointerDown = false;
    }
}
