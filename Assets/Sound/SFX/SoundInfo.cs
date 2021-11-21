using UnityEngine;

public enum SoundType
{
    None,
    EnergyGun_Fire,
    ShotGun_Fire,
    FlameThrower_Fire,
    AutoRifle_Fire,
    Explosion,
    Hit,
    WeaknessHit,
    Walk,
    Jump,
    Land,
    GateOpen,
    Dash,
    Laser,
    EnergyBall,
    Warning_Floor,
    Warning_TypeC,
    Electric,
    EnergyBall_Bomb,
    Container_Drop,
    AutoRifle_Reload_1,
    AutoRifle_Reload_2,
    Parkour,
    HeartBeat,
    Gun_load,
    Explosion_1,
    DoubleJump,
    Explosion_2,
    FutureGun_Fire,
    GetMag,
    Timer,
    Enemy_roll,
    EnergyGun_Charge,
    EnergyGun_Fire2,
    Bullet_BounceOff,
    Player_Damaged,
    Explosion_Fire,
    ChainLightning_Reload,
    EnergyBall_Fire,
    Sniper_Reload,
    Boss_TypeX_WakeUp,
    Swing,
    Boss_TypeX_Idle,
    Drink
}

[System.Serializable]
public class SoundInfos
{
    public SoundType soundType;
    public AudioClip clip;

    [Range(0, 1)]
    public float volume;
    [Range(0, 3)]
    public float pitch_min = 1;
    [Range(0, 3)]
    public float pitch_max = 1;
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
