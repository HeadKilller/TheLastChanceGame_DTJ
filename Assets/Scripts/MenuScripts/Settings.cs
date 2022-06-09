using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    Resolution[] listResol;
    [SerializeField] TMPro.TMP_Dropdown resolDrop;
    private void Start()
    {
        listResol = Screen.resolutions;
        resolDrop.ClearOptions();
        List<string> ListString = new List<string>();
        int currentScreen = 0;
        for(int i = 0; i< listResol.Length;i++)
        {
            string opt = listResol[i].width +" x "+ listResol[i].height;
            ListString.Add(opt);
            if(listResol[i].width == Screen.currentResolution.width && listResol[i].height == Screen.currentResolution.height)
            {
                currentScreen = i;
            }
        }
        resolDrop.AddOptions(ListString);
        resolDrop.value = currentScreen;
        resolDrop.RefreshShownValue();
    }
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }
    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }
    public void SetFullscreen(bool isFullcreen)
    {
        Screen.fullScreen = isFullcreen;
    }
    public void ChangeResolutin(int resol)
    {
        Resolution reslt = listResol[resol];
        Screen.SetResolution(reslt.width, reslt.height, Screen.fullScreen);
    }
}
