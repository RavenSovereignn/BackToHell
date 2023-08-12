using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSaveControll : MonoBehaviour
{
    [SerializeField]
    private Slider volumeSlider = null;
    [SerializeField]
    private Text volumeTextUI = null;
    public int volumeParameters;

    public void VolumeSlider(float volumeParameters)
    {
        volumeTextUI.text = volumeParameters.ToString("0,0");
        Controls.instance.volume = volumeParameters*0.01f;
    }
   
}
