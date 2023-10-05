using UnityEngine;
using SonsSdk;
using FMODCustom;
using Sons.Settings;

namespace SonsAxLib;

/// <summary>
/// Class to manage sounds registered with SoundTools
/// </summary>
public class AudioController
{
    static Dictionary<string, Channel> _idChannelPairMusic = new();
    static Dictionary<string, Channel> _idChannelPairSfx = new();
    static float _correctionFactor = 0.5f;

    public enum SoundType
    {
        Music = 0,
        Sfx = 1,
    }

    /// <summary>
    /// Play a registered sound with additional options
    /// </summary>
    /// <param name="id">Sound ID</param>
    /// <param name="type">Sound type: <see langword="Music"/> = uses game settings music volume | <see langword="Sfx"/> = uses game settings sfx volume</param>
    /// <param name="loop"></param>
    /// <param name="volAdjustmant">Additional volume adjustment</param>
    /// <param name="pitch"></param>
    /// <returns>The <see langword="Channel"/> of the played sound</returns>
    public static Channel PlaySound(string id, SoundType type, bool loop = false, float volAdjustmant = 1, float? pitch = null)
    {
        float volume = (type == SoundType.Music) ? AudioSettings._musicVolume : AudioSettings._sfxVolume;

        Channel ch = SoundTools.PlaySound(id, volume * AudioSettings._masterVolume * _correctionFactor * volAdjustmant, pitch);

        if (loop)
        {
            ch.setMode(MODE.LOOP_NORMAL);
            var dictType = (type == SoundType.Music) ? _idChannelPairMusic : _idChannelPairSfx;
            dictType.Add(id, ch);
        }
        return ch;
    }

    /// <summary>
    /// Play a 3D registered sound with additional options
    /// </summary>
    /// <param name="id">Sound ID</param>
    /// <param name="type">Sound type: <see langword="Music"/> = uses game settings music volume | <see langword="Sfx"/> = uses game settings sfx volume</param>
    /// <param name="pos">World position where the sound will be played</param>
    /// <param name="maxDist">Maximum distance where the sound can be heard</param>
    /// <param name="loop"></param>
    /// <param name="volAdjustmant">Additional volume adjustment</param>
    /// <param name="pitch"></param>
    /// <returns>The <see langword="Channel"/> of the played sound</returns>
    public static Channel Play3DSound(string id, SoundType type, Vector3 pos, float maxDist, bool loop = false, float volAdjustmant = 1, float? pitch = null)
    {
        float volume = (type == SoundType.Music) ? AudioSettings._musicVolume : AudioSettings._sfxVolume;

        Channel ch = SoundTools.PlaySound(id, pos, maxDist, volume * AudioSettings._masterVolume * _correctionFactor * volAdjustmant, pitch);
        if (loop)
        {
            ch.setMode(MODE.LOOP_NORMAL);
            var dictType = (type == SoundType.Music) ? _idChannelPairMusic : _idChannelPairSfx;
            dictType.Add(id, ch);
        }
        return ch;
    }

    /// <summary>
    /// Stop a looping sound
    /// </summary>
    /// <param name="id">Sound ID</param>
    /// <returns><see langword="true"/> if stopped, <see langword="false"/> if couldn't find the given id or sound isn't looping</returns>
    public static bool StopSound(string id)
    {
        if (_idChannelPairMusic.TryGetValue(id, out Channel chMusic))
        {
            chMusic.stop();
            _idChannelPairMusic.Remove(id);
            return true;
        }

        if (_idChannelPairSfx.TryGetValue(id, out Channel chSfx))
        {
            chSfx.stop();
            _idChannelPairSfx.Remove(id);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Stop all looping sounds
    /// </summary>
    public static void StopAllSounds()
    {
        _idChannelPairMusic.ToList().ForEach(ch => { 
            ch.Value.stop();
            _idChannelPairMusic.Remove(ch.Key);
        });

        _idChannelPairSfx.ToList().ForEach(ch =>
        {
            ch.Value.stop();
            _idChannelPairSfx.Remove(ch.Key);
        });       
    }

    /// <summary>
    /// Set the volume of a looping sound given it's id
    /// </summary>
    /// <param name="id">Sound ID</param>
    /// <param name="volume"></param>
    /// <returns><see langword="true"/> if set, <see langword="false"/> if couldn't set or given id is invalid</returns>
    public static bool SetVolume(string id, float volume = 1f)
    {
        if (_idChannelPairMusic.TryGetValue(id, out Channel chMusic))
        {
            chMusic.setVolume(volume / _correctionFactor);
            return true;
        }

        if (_idChannelPairSfx.TryGetValue(id, out Channel chSfx))
        {
            chSfx.setVolume(volume / _correctionFactor);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Set the volume of a looping sound given it's channel
    /// </summary>
    /// <param name="channel">Channel of the sound</param>
    /// <param name="volume"></param>
    public static void SetVolume(Channel channel, float volume = 1f)
    {
        channel.setVolume(volume / _correctionFactor);
    }

    /// <summary>
    /// Set the pitch of a looping sound given it's id
    /// </summary>
    /// <param name="id">Sound ID</param>
    /// <param name="pitch"></param>
    /// <returns><see langword="true"/> if set, <see langword="false"/> if couldn't set or given id is invalid</returns>
    public static bool SetPitch(string id, float pitch = 1f)
    {
        if (_idChannelPairMusic.TryGetValue(id, out Channel chMusic))
        {
            chMusic.setPitch(pitch);
            return true;
        }

        if (_idChannelPairSfx.TryGetValue(id, out Channel chSfx))
        {
            chSfx.setPitch(pitch);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Set the pitch of a looping sound given it's channel
    /// </summary>
    /// <param name="channel">Channel of the sound</param>
    /// <param name="pitch"></param>
    public static void SetPitch(Channel channel, float pitch = 1f)
    {
        channel.setPitch(pitch);
    }
}
