using UnityEngine;

public enum BGMSoundType
{
    BGM_Field1,
    BGM_Boss1,
    BGM_Field2,
    BGM_CutScene,
    BGM_Boss2,
    BGM_Credit,
    BGM_Field3,
    BGM_Field4,
}

[System.Serializable]
public class BGMSoundInfos
{
    public BGMSoundType soundType;
    public AudioClip clip;

    [Range(0, 1)]
    public float volume;
    [Range(0, 3)]
    public float pitch_min = 1;
    [Range(0, 3)]
    public float pitch_max = 1;
}

[CreateAssetMenu(fileName = "BGMSoundInfo", menuName = "Scriptable Object Asset/BGMSoundInfo")]
public class BGMSoundInfo : ScriptableObject
{
    public BGMSoundInfos GetInfo(BGMSoundType soundType)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].soundType == soundType)
            {
                return sounds[i];
            }
        }

        return null;
    }

    public BGMSoundInfos[] sounds;
}
