using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class RecordEditor : MonoBehaviour {
    readonly string directory = "Assets/mcp";

    [HideInInspector] public List<RecordProperty> recordProperty;

    [SerializeField] int FPS = 30;
    [SerializeField] float lerpSpeed = 30;
    [SerializeField] bool isPlay;
    [SerializeField] bool repeat;
    [SerializeField] int currentFrame = 0;
    [SerializeField] Dropdown m_dropdown;
    [SerializeField] Text m_TotalFrame;
    [SerializeField] InputField m_CurrentFrame;

    [Header("Buttons")]
    [SerializeField] Button m_StartButton;
    [SerializeField] Button m_StopButton;
    [SerializeField] Button m_PauseButton;
    [SerializeField] Button m_NextButton;
    [SerializeField] Button m_BackButton;
    [SerializeField] Button m_RepeatButton;

    Selectable<int> selectable = new Selectable<int>();
    private Animator anim = null;
    private float timer = 0;

    public void SetAnim(Animator anim) {
        this.anim = anim;
    }

    private void Awake() {
        m_dropdown.value = 0;
        SetDropdownItem();
        SetProperty(0);
        SetCallBack();
    }

    void SetCallBack() {
        selectable.m_Changed += currentFrame => UpdateFrame();
        m_dropdown.onValueChanged.AddListener(SetProperty);
        m_CurrentFrame.onEndEdit.AddListener(SetFrame);
        m_StartButton.onClick.AddListener(StartButton);
        m_StopButton.onClick.AddListener(StopButton);
        m_PauseButton.onClick.AddListener(PauseButton);
        m_NextButton.onClick.AddListener(NextButton);
        m_BackButton.onClick.AddListener(BackButton);
        m_RepeatButton.onClick.AddListener(RepeatButton);
    }

    void Update() {
        UpdateButton();
        UpdateCurrentFrame();
    }

    private void UpdateCurrentFrame() {
        m_CurrentFrame.text = currentFrame.ToString();
        selectable.SetValueIfNotEqual(currentFrame);
        if (isPlay) {
            timer += Time.deltaTime;
            if (timer >= (float)1 / FPS && currentFrame < recordProperty.Count) {
                timer = 0;
                if (currentFrame < recordProperty.Count - 1) {
                    currentFrame += 1;
                } else {
                    isPlay = false;
                }
            }
            if (repeat && currentFrame >= recordProperty.Count - 1) {
                currentFrame = 0;
            }
        } else if (recordProperty.Count > 0) {
            currentFrame = Mathf.Clamp(currentFrame, 0, recordProperty.Count - 1);
        }
    }

    private void UpdateFrame() {
        if (anim == null) {
            Debug.Log("No Animator Component");
        } else {
            SetRecordProperty(anim, recordProperty[currentFrame],isPlay);
        }
    }

    private void UpdateButton() {
        m_PauseButton.gameObject.SetActive(isPlay);
        m_StartButton.gameObject.SetActive(!isPlay);
        if (repeat) {
            m_RepeatButton.image.color = Color.cyan;
        } else {
            m_RepeatButton.image.color = Color.white;
        }
    }

    private void SetFrame(string str) {
        if (str != null) {
            m_CurrentFrame.text = Mathf.Clamp(int.Parse(str), 0, recordProperty.Count - 1).ToString();
            currentFrame = int.Parse(m_CurrentFrame.text);
        } else {
            currentFrame = 0;
        }
    }

    private void SetDropdownItem() {
        var files = new DirectoryInfo(directory).GetFiles("*.mcp");
        foreach (var item in files) {
            m_dropdown.options.Add(new Dropdown.OptionData { text = item.Name });
        }
    }

    private void SetProperty(int i) {
        if (i != 0) {
            recordProperty = (List<RecordProperty>)Util.Serializer.LoadFromBinaryFile(Path.Combine(directory, m_dropdown.options[i].text));
        } else {
            recordProperty.Clear();
        }
        currentFrame = 0;
        m_CurrentFrame.text = "0";
        m_TotalFrame.text = "/ " + (recordProperty.Count > 0 ? recordProperty.Count - 1 : 0).ToString();
        isPlay = false;
    }

    private void SetRecordProperty(Animator anim, RecordProperty recordProperty, bool lerp = true) {
        foreach (HumanBodyBones bone in Enum.GetValues(typeof(HumanBodyBones))) {
            if (bone < 0 || bone == HumanBodyBones.LastBone)
                continue;
            if (anim.GetBoneTransform(bone) != null) {
                anim.GetBoneTransform(bone).localRotation =
                    Quaternion.Lerp(
                        anim.GetBoneTransform(bone).localRotation,
                        new Quaternion() {
                            x = recordProperty.Joint[(int)bone, 0],
                            y = recordProperty.Joint[(int)bone, 1],
                            z = recordProperty.Joint[(int)bone, 2],
                            w = recordProperty.Joint[(int)bone, 3]
                        },
                        lerp ? lerpSpeed * Time.deltaTime : 1);
                if (bone == HumanBodyBones.Hips) {
                    anim.GetBoneTransform(bone).localPosition =
                        Vector3.Lerp(
                            anim.GetBoneTransform(bone).localPosition,
                            new Vector3() {
                                x = recordProperty.Hips[0],
                                y = recordProperty.Hips[1],
                                z = recordProperty.Hips[2]
                            },
                            lerp ? lerpSpeed * Time.deltaTime : 1);
                }
            }
        }
    }

    private void StartButton() {
        if (recordProperty.Count != 0) {
            isPlay = true;
        }
        if (currentFrame == recordProperty.Count - 1) {
            currentFrame = 0;
        }
    }

    private void StopButton() {
        isPlay = false;
        currentFrame = 0;
    }

    private void PauseButton() {
        isPlay = false;
    }

    private void NextButton() {
        currentFrame = Mathf.Clamp(currentFrame + 1, 0, recordProperty.Count);
    }

    private void BackButton() {
        currentFrame = Mathf.Clamp(currentFrame - 1, 0, recordProperty.Count);
    }

    private void RepeatButton() {
        repeat = !repeat;
    }
}
