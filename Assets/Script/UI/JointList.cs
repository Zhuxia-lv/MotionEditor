using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JointList : MonoBehaviour
{
    [SerializeField] GameObject m_content;

    private Animator anim;

    public void CreateJointList(Animator anim) {
        this.anim = anim;
        DestroyAllChiledObject();

        if (anim != null) {
            foreach (HumanBodyBones bone in Enum.GetValues(typeof(HumanBodyBones))) {
                if (bone < 0 || bone == HumanBodyBones.LastBone)
                    continue;
                if (bone == HumanBodyBones.Hips) {
                    var joint = Instantiate(m_content, gameObject.transform);
                    joint.name = bone.ToString() + "_position";
                    joint.GetComponent<JointDetaile>().joint = anim.GetBoneTransform(bone).gameObject;
                }
                if (anim.GetBoneTransform(bone) != null) {
                    var joint = Instantiate(m_content, gameObject.transform);
                    joint.name = bone.ToString();
                    joint.GetComponent<JointDetaile>().joint = anim.GetBoneTransform(bone).gameObject;
                }
            }
        }
    }

    void DestroyAllChiledObject() {
        foreach(Transform t in gameObject.transform) {
            Destroy(t.gameObject);
        }
    }
}
