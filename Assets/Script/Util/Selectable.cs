using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable<T> {
    private T m_value;

    public T Value {
        get { return m_value; }
        set {
            m_value = value;
            OnChanged(m_value);
        }
    }

    public System.Action<T> m_Changed;

    public Selectable() {
        m_value = default(T);
    }

    public Selectable(T value) {
        m_value = value;
    }

    public void SetValueWithoutCallback(T value) {
        m_value = value;
    }

    public void SetValueIfNotEqual(T value) {
        if (m_value.Equals(value)) {
            return;
        }
        m_value = value;
        OnChanged(m_value);
    }

    private void OnChanged(T value) {
        var onChaneged = m_Changed;
        if(onChaneged == null) {
            return;
        }
        onChaneged(value);
    }

    public void Dispose() {
        m_Changed = null;
    }
}
