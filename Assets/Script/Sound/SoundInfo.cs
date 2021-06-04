using UnityEngine;
using UnityEditor;

public enum SoundType
{
    None,
    EnergyGun_Fire,
    ShotGun_Fire
}

[System.Serializable]
public class SoundInfos
{
    public SoundType soundType;
    public AudioClip clip;

    [Range(0, 1)]
    public float volume;
    [Range(0, 3)]
    public float pitch;
}

[CreateAssetMenu(fileName = "SoundInfo", menuName = "Scriptable Object Asset/SoundInfo")]
public class SoundInfo : ScriptableObject
{
    public SoundInfos GetInfo(SoundType soundType)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if(sounds[i].soundType == soundType)
            {
                return sounds[i];
            }
        }

        return null;
    }

    public SoundInfos[] sounds;
}
