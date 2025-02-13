// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
// using OculusSampleFramework;

public class FingerTracking : MonoBehaviour
{
    public OVRSkeleton leftHandSkeleton;  // OVRCameraRig object for hand
    public OVRSkeleton rightHandSkeleton;

    public Transform[] leftHandFingerBones; // Avatar's bones
    public Transform[] rightHandFingerBones;

    private void Update()
    {
        if (leftHandSkeleton != null && leftHandSkeleton.Bones.Count > 0)
        {
            for (var i = 0; i < leftHandFingerBones.Length; i++)
            {
                if (i >= leftHandSkeleton.Bones.Count) continue;
                leftHandFingerBones[i].position = leftHandSkeleton.Bones[i].Transform.position;
                leftHandFingerBones[i].rotation = leftHandSkeleton.Bones[i].Transform.rotation;
            }
        }

        if (rightHandSkeleton == null || rightHandSkeleton.Bones.Count <= 0) return;
        {
            for (var i = 0; i < rightHandFingerBones.Length; i++)
            {
                if (i >= rightHandSkeleton.Bones.Count) continue;
                rightHandFingerBones[i].position = rightHandSkeleton.Bones[i].Transform.position;
                rightHandFingerBones[i].rotation = rightHandSkeleton.Bones[i].Transform.rotation;
            }
        }
    }
}