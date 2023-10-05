using Il2CppSystem.Runtime.Remoting.Messaging;
using JetAnnotations;
using RedLoader;
using Sons.Ai.Vail;
using Sons.Characters;
using Sons.Items.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheForest.Utils;
using UnityEngine;

namespace SonsAxLib;

public class ActorsSpawner
{
    static readonly string _cannibal = "CannibalFamily";
    static readonly string _fat = "FatMale";
    static readonly string _heavy = "CannibalHeavyFamily";
    static readonly string _twins = "Twins";
    static readonly string _demon = "Demon";
    static readonly string _demonboss = "DemonBoss";
    static readonly string _fingers = "Fingers";
    static readonly string _misspuffy = "MissPuffy";
    static readonly string _mrpuffton = "MrPuffton";
    static readonly string _armsy = "Armsy";
    static readonly string _creepyVirginia = "CreepyVirginia";

    public enum ActorTypes
    {
        Cannibal,
        Fat,
        Heavy,
        Twins,
        Demon,
        DemonBoss,
        Fingers,
        MissPuffy,
        MrPuffton,
        Amrsy,
        CreepyVirginia
    }

    internal static string GetActor(ActorTypes actor)
    {
        return actor switch
        {
            ActorTypes.Cannibal => _cannibal,
            ActorTypes.Fat => _fat,
            ActorTypes.Heavy => _heavy,
            ActorTypes.Twins => _twins,
            ActorTypes.Demon => _demon,
            ActorTypes.DemonBoss => _demonboss,
            ActorTypes.Fingers => _fingers,
            ActorTypes.MissPuffy => _misspuffy,
            ActorTypes.MrPuffton => _mrpuffton,
            ActorTypes.Amrsy => _armsy,
            ActorTypes.CreepyVirginia => _creepyVirginia,
            _ => "",
        };
    }

    static VailSpawner _customVailSpawner;
    public VailSpawner CustomVailSpawner
    {
        get { return _customVailSpawner; }
    }

    /// <summary>
    /// Creates a new instance of the class
    /// </summary>
    /// <returns></returns>
    public static ActorsSpawner New()
    {
        ActorsSpawner actorsSpawner = new();
        return actorsSpawner;
    }

    /// <summary>
    /// Creates a <see langword="VailSpawner"/> with the given actor type
    /// </summary>
    /// <param name="actorType"></param>
    /// <returns></returns>
    public ActorsSpawner CreateSpawner(ActorTypes actorType)
    {
        string actorId = GetActor(actorType);

        var customVailSpawnerObject = new GameObject("CustomVailSpawner");
        _customVailSpawner = customVailSpawnerObject.AddComponent<VailSpawner>();
        _customVailSpawner.name = $"{actorId}_CustomVailSpawner";
        _customVailSpawner._characterDefinition = CharacterManager.Instance.GetDefinition(actorId);
        return this;
    }

    /// <summary>
    /// Spawn actor/s at the given position
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="count"></param>
    public WorldSimActor Spawn(Vector3? pos = null, int count = 1)
    {
        pos ??= LocalPlayer.MainCamTr.position + LocalPlayer.MainCamTr.forward * 10f;
        return _customVailSpawner.SpawnWorldSimActor((Vector3)pos, 0, count);
    }
}
