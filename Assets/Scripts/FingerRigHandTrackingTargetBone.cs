// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;
using RootMotion.FinalIK;
using UnityEngine.XR.Management;
// using UnityEngine.XR.Interaction.Toolkit;

public class FingerRigHandTrackingTargetBone : MonoBehaviour
{
    // Variables
    public XRHandSubsystem HandSubsystem;
    public bool leftHand;
    public bool rightHand;
    public FingerRig handFingerRig;
    public Transform[] targetBones;
    private readonly Vector3 rotationOffsetE = new Vector3(90, 0, 180);   //From Thumb to Pinky
    private Quaternion rotationOffsetQ;
    
    private readonly XRHandFingerID[] fingers = { // There is not a variable in the API with all the fingers :(
        XRHandFingerID.Thumb,
        XRHandFingerID.Index,
        XRHandFingerID.Middle,
        XRHandFingerID.Ring,
        XRHandFingerID.Little
    };
    
    // Start is called before the first frame update
    void Start()
    {
        rotationOffsetQ = Quaternion.Euler(rotationOffsetE);
        
        if (HandSubsystem == null)
        {
            HandSubsystem = XRGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<XRHandSubsystem>();
            if (HandSubsystem == null)
            {
                Debug.LogError("No XR Hand Subsystem found.");
            }
        }

        if (handFingerRig == null)
            Debug.LogError("No FingerRig found.");
    }
    
    void LateUpdate()
    {
        if (leftHand && HandSubsystem.leftHand.isTracked)
            UpdateFingerTargets(HandSubsystem.leftHand, targetBones, rotationOffsetQ);
        if (rightHand && HandSubsystem.rightHand.isTracked)
            UpdateFingerTargets(HandSubsystem.rightHand, targetBones, rotationOffsetQ);
    }

    private void UpdateFingerTargets(XRHand hand, Transform[] targetTips, Quaternion rotation)
    {
        int index = 0;
        foreach (XRHandFingerID finger in fingers)
        {
            XRHandJointID jointTipId = finger.GetBackJointID();
            XRHandJoint jointTip = hand.GetJoint(jointTipId);
            if (index < targetTips.Length && jointTip.TryGetPose(out Pose pose))
            {
                targetTips[index].position = pose.position;
                targetTips[index].rotation = pose.rotation * rotation;
            }

            index++;
        }
    }
}
