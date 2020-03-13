using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recorder
{
    private List<RecordProperty> records = new List<RecordProperty>();
    private bool isRecord = false;
    private Animator anim;

    private string path;
    private string extention = ".mcp";

    public void Awake(Animator anim,string path) {
        this.anim = anim;
        this.path = path;
    }

    public void Start() {
        if (isRecord)
            return;

        Clear();
        isRecord = true;
    }

    public void Stop() {
        if (records.Count > 0) {
            isRecord = false;
            Util.Serializer.SaveToBinaryFile(records, path, anim.name, extention);
            Debug.Log("Recoridng Frame :" + records.Count);
            Clear();
        }
    }

    public void Pause() {
        isRecord = false;
    }

    public void Resume() {
        isRecord = true;
    }

    public void Clear() {
        records.Clear();
    }

    public void AddFrame() {
        records.Add(RecordProperty.NewFrame(anim));
    }

    public bool IsRecord() {
        return isRecord;
    }
}
