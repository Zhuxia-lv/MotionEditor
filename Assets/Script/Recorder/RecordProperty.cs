using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class RecordProperty {

    public float[] Hips;
    public float[,] Joint;
    //public float[] Face;

    public static RecordProperty NewFrame(Animator anim) {
        var frame = new RecordProperty() {
            Hips = NewHipsPosition(anim),
            Joint = NewJointRotation(anim)
        };
        return frame;
    }

    //public static RecordProperty NewFrame(Animator anim,params SkinnedMeshRenderer[] mesh) {
    //    var frame = new RecordProperty() {
    //        Hips = NewHipsPosition(anim),
    //        Joint = NewJointRotation(anim),
    //        Face = NewFaceAnimation(mesh),
    //    };
    //    return frame;
    //}

    private static float[] NewHipsPosition(Animator anim) {
        Vector3 pos = anim.GetBoneTransform(HumanBodyBones.Hips).localPosition;
        return new float[3] { pos.x, pos.y, pos.z };
    }

    private static float[,] NewJointRotation(Animator anim) {
        var joints = new float[55, 4];
        foreach(HumanBodyBones bone in Enum.GetValues(typeof(HumanBodyBones))) {
            if (bone < 0 || bone == HumanBodyBones.LastBone)
                continue;
            if (anim.GetBoneTransform(bone) != null) {
                Quaternion rotate = anim.GetBoneTransform(bone).localRotation;
                joints[(int)bone, 0] = rotate.x;
                joints[(int)bone, 1] = rotate.y;
                joints[(int)bone, 2] = rotate.z;
                joints[(int)bone, 3] = rotate.w;
            }
        }
        return joints;
    }

    //private static float[] NewFaceAnimation(params SkinnedMeshRenderer[] mesh) {
    //}
}
