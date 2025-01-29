using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forged_fury;

public class SoundPlayer
{
    private float _volume = 0.0f;
    private float _randomVolumeAmount = 0.0f;
    private float _randomPitchAmount = 0.0f;

    private List<SoundEffect> _sounds;

    public float Volume
    {
        get => _volume;
        set
        {
            _volume = Math.Clamp(value, 0.0f, 1.0f);
        }
    }

    public float RandomVolumeAmount
    {
        get => _randomVolumeAmount;
        set
        {
            _randomVolumeAmount = Math.Clamp(value, 0.0f, 1.0f);
        }
    }

    public float RandomPitchAmount
    {
        get => _randomPitchAmount;
        set
        {
            _randomPitchAmount = Math.Clamp(value, 0.0f, 1.0f);
        }
    }

    public SoundPlayer()
    {
        _sounds = new();
        _volume = 1.0f;
        _randomVolumeAmount = 0.0f;
        _randomPitchAmount = 0.25f;
    }

    public void AddSound(SoundEffect sound) => _sounds.Add(sound);
    public void ClearSounds() => _sounds.Clear();

    public void PlayRandomSound()
    {
        var rand = new Random();
        var index = rand.Next(0, _sounds.Count());
        var sound = _sounds.ElementAtOrDefault(index);

        var volume = _volume + ((float)(rand.Next(0,2000) / 1000.0f) - 1.0f) * _randomVolumeAmount;
        volume = Math.Clamp(volume, 0.0f, 1.0f);

        var pitch = ((float)(rand.Next(0, 2000) / 1000.0f) - 1.0f) * _randomPitchAmount;

        if (sound != null)
        {
            sound.Play(volume, pitch, 0);
        }
    }
}
