using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    void Start()
    {
        LimitFPS();
    }

    private void LimitFPS()
    {
        QualitySettings.vSyncCount = 1;
    }
}
