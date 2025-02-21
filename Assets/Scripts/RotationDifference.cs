// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class RotationDifference : MonoBehaviour
{
    // Variables
    public Transform targetTip;
    public Transform avatarTip;

    private const string Qx = "Quaternion_x";
    private const string Qy = "Quaternion_y";
    private const string Qz = "Quaternion_z";
    private const string Qw = "Quaternion_w";

    private Quaternion rotationDifference;

    // Start is called before the first frame update
    void Start()
    {
        if (targetTip ==null && avatarTip == null)
        {
            Application.Quit();
        }
        
        if (ExistsQuaternion())
            this.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!ExistsQuaternion())
        {
            rotationDifference = Quaternion.Inverse(targetTip.rotation) * avatarTip.rotation;
            SaveRotationDifference();
        }

        this.enabled = false;
    }

    private bool ExistsQuaternion()
    {
        return PlayerPrefs.HasKey(Qx) && PlayerPrefs.HasKey(Qy) && PlayerPrefs.HasKey(Qz) && PlayerPrefs.HasKey(Qw);
    }

    private void SaveRotationDifference()
    {
        PlayerPrefs.SetFloat(Qx, rotationDifference.x);
        PlayerPrefs.SetFloat(Qy, rotationDifference.y);
        PlayerPrefs.SetFloat(Qz, rotationDifference.z);
        PlayerPrefs.SetFloat(Qw, rotationDifference.w);
        PlayerPrefs.Save();
    }
}
