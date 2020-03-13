using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour {
    readonly string directory = "Assets/Resources/Character";

    [SerializeField] JointList jointList;
    [SerializeField] RecordEditor recordEditor;
    [SerializeField] Dropdown m_dropdown;

    private Selectable<Animator> selectable = new Selectable<Animator>();
    private GameObject character;
    private Animator anim;

    private void Awake() {
        SetDropdownItem();
        m_dropdown.onValueChanged.AddListener(SetAnim);
    }

    private void Init() {
        jointList.CreateJointList(anim);
        recordEditor.SetAnim(anim);
    }

    private void SetDropdownItem() {
        var files = new DirectoryInfo(directory).GetFiles("*.prefab");
        foreach (var item in files) {
            string filename = Path.GetFileNameWithoutExtension(item.ToString());
            m_dropdown.options.Add(new Dropdown.OptionData { text = filename });
        }
    }

    private void SetAnim(int i) {
        Destroy(character);
        anim = null;
        var @object = Resources.Load<GameObject>("Character/" + m_dropdown.options[i].text);
        if (@object != null) {
            character = Instantiate(@object, gameObject.transform);
            anim = character.GetComponent<Animator>();
            anim.runtimeAnimatorController = null;
        }
        Init();
    }
}
