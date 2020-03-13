using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordingComponent : MonoBehaviour
{
    [SerializeField] Animator m_Animator;
    [SerializeField] string m_FilePath;
    [SerializeField] int m_FPS;

    private float Timer = 0;
    private Recorder recorder = new Recorder();

    private void Awake() {
        recorder.Awake(m_Animator,m_FilePath);
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            recorder.Start();
            Debug.Log("Recording Started");
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            recorder.Stop();
            Debug.Log("Recoridng Stoped");
        }

        if (recorder.IsRecord()) {
            if (m_FPS == 0) {
                Debug.LogAssertion("FPSが0です");
                recorder.Stop();
            } else {
                Timer += Time.deltaTime;
                if (Timer >= (float)1 / m_FPS) {
                    recorder.AddFrame();
                    Timer = 0;
                }
            }
        }
    }
}
