// using System.Collections.Generic;
using System.Collections;
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
    // private readonly Vector3 rotationOffsetE = new Vector3(180, 0, 90);   //From Thumb to Pinky
    private Quaternion rotationOffsetQ;
    private const string Qx = "Quaternion_x";
    private const string Qy = "Quaternion_y";
    private const string Qz = "Quaternion_z";
    private const string Qw = "Quaternion_w";
    
    private readonly XRHandFingerID[] fingers = { // There is not a varible in the API with all the fingers :(
        XRHandFingerID.Thumb,
        XRHandFingerID.Index,
        XRHandFingerID.Middle,
        XRHandFingerID.Ring,
        XRHandFingerID.Little
    };
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitRotation());
        rotationOffsetQ = LoadRotationDifference();
        
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
            UpdateFingerRotation(HandSubsystem.leftHand, targetBones, rotationOffsetQ);
        if (rightHand && HandSubsystem.rightHand.isTracked)
            UpdateFingerRotation(HandSubsystem.rightHand, targetBones, rotationOffsetQ);
    }

    private void UpdateFingerRotation(XRHand hand, Transform[] targetTips, Quaternion rotation)
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
    
    private bool ExistsQuaternion()
    {
        return PlayerPrefs.HasKey(Qx) && PlayerPrefs.HasKey(Qy) && PlayerPrefs.HasKey(Qz) && PlayerPrefs.HasKey(Qw);
    }
    
    private Quaternion LoadRotationDifference()
    {
        float x = PlayerPrefs.GetFloat(Qx, 0f);
        float y = PlayerPrefs.GetFloat(Qy, 0f);
        float z = PlayerPrefs.GetFloat(Qz, 0f);
        float w = PlayerPrefs.GetFloat(Qw, 1f);
        return new Quaternion(x, y, z, w);
    }

    private IEnumerator WaitRotation()
    {
        while (!ExistsQuaternion())
            yield return new WaitForEndOfFrame();
    }
}