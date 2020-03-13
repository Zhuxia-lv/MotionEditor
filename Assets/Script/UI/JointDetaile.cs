using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JointDetaile : MonoBehaviour,IPointerDownHandler{

    [SerializeField] public GameObject joint;
    [SerializeField] Text m_text;
    [SerializeField] InputField x_input;
    [SerializeField] InputField y_input;
    [SerializeField] InputField z_input;

    private Selectable<Quaternion> selectable = new Selectable<Quaternion>();
    private Quaternion q;

    private void Start() {
        selectable.m_Changed += q => UpdateJointData();
        m_text.text = gameObject.name;
        x_input.onEndEdit.AddListener(SetToJoint);
        y_input.onEndEdit.AddListener(SetToJoint);
        z_input.onEndEdit.AddListener(SetToJoint);
    }

    void Update() {
        selectable.SetValueIfNotEqual(joint.transform.localRotation);
    }

    void UpdateJointData() {
        if (m_text.text == "Hips_position") {
            x_input.text = joint.transform.localPosition.x.ToString();
            y_input.text = joint.transform.localPosition.y.ToString();
            z_input.text = joint.transform.localPosition.z.ToString();
        } else {
            x_input.text = joint.transform.localRotation.x.ToString();
            y_input.text = joint.transform.localRotation.y.ToString();
            z_input.text = joint.transform.localRotation.z.ToString();
        }
    }

    void SetToJoint(string str) {
        var q = new Vector3() {
            x = float.Parse(x_input.text),
            y = float.Parse(y_input.text),
            z = float.Parse(z_input.text)
        };
        if (m_text.text == "Hips_position") {
            joint.transform.localPosition = q;
        } else {
            joint.transform.localRotation = Quaternion.Euler(q);
        }
    }

    private void OnDestroy() {
        selectable.Dispose();
    }

    private void Test() {
        //EventSystem eventData = EventSystem.current;
        //if (eventData. == x_input) {
        //    Debug.Log("Press!");
        //}
    }

    public void OnPointerDown(PointerEventData eventData) {
        x_input.OnPointerDown(eventData);
        {
            Debug.Log("Press!");
        }
    }
}
