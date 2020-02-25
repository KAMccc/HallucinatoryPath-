using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickButtonAudio : MonoBehaviour
{
    private AudioSource m_AudioSource;
    private ManagerVars vars;

    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
        m_AudioSource = GetComponent<AudioSource>();
        EventCenter.AddListener(EventDefine.PlayClickAudio, PlayAudio);
        EventCenter.AddListener<bool>(EventDefine.IsMusicOn, IsMusicOn);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.PlayClickAudio, PlayAudio);
        EventCenter.RemoveListener<bool>(EventDefine.IsMusicOn, IsMusicOn);

    }

    private void PlayAudio()
    {
        m_AudioSource.PlayOneShot(vars.buttonClip);
    }

    /// <summary>
    /// 音效是否开启
    /// </summary>
    /// <param name="isMusicOn"></param>
    private void IsMusicOn(bool isMusicOn)
    {
        m_AudioSource.mute = !isMusicOn;
    }
}
