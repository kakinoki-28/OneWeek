using UnityEngine;

public class PlaySEPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip seClip;
    public void PlaySE(AudioClip clip = null)
    {
        if (clip == null) clip = seClip;
        if (seClip == null) return;

        // 一時的なEmptyオブジェクトを作成
        GameObject tempAudioObj = new GameObject("TempSE_" + seClip.name);
        tempAudioObj.transform.position = transform.position;
        AudioSource tempSource = tempAudioObj.AddComponent<AudioSource>();
        tempSource.clip = clip;
        
        tempSource.spatialBlend = 0.0f; 

        tempSource.Play();

        Destroy(tempAudioObj, clip.length);
    }
}