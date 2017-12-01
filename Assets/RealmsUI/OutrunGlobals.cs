using System.Collections.Generic;
using UnityEngine;

static class OutrunGlobals
{
    public const int WALL_LAYER = 9;
    public const int WALL_LAYER_FLAG = 1 << WALL_LAYER;
    public const int VEHICLE_LAYER = 1 << 12;
    public const int VEHICLE_LAYER_FLAG = 1 << VEHICLE_LAYER;

    public enum GlobalStates { beforeCalibrate, running, dying };
    public static GlobalStates GlobalGameStates = GlobalStates.beforeCalibrate;

    public static Dictionary<string, Color> ColorScheme = new Dictionary<string, Color>{
        { "mainColor", new Color(252 /255f, 52 /255f, 255 /255f, 1) },
        { "secondaryColor", new Color(40 /255f, 213 /255f, 238 /255f, 1) },
        {"darkGrey", new Color(61 /255f, 61 /255f, 61 /255f,1) }
    };

//    public static FMOD.VECTOR ToFMODVector(this UnityEngine.Vector3 vec)
//    {
//        FMOD.VECTOR temp;
//        temp.x = vec.x;
//        temp.y = vec.y;
//        temp.z = vec.z;
//
//        return temp;
//    }
//
//    public static FMOD.ATTRIBUTES_3D To3DAttributes(this UnityEngine.Vector3 pos)
//    {
//        FMOD.ATTRIBUTES_3D attributes = new FMOD.ATTRIBUTES_3D();
//        attributes.forward = UnityEngine.Vector3.forward.ToFMODVector();
//        attributes.up = UnityEngine.Vector3.up.ToFMODVector();
//        attributes.position = pos.ToFMODVector();
//
//        return attributes;
//    }

    public static T GetRandom<T>(this List<T> self)
    {
        return self[UnityEngine.Random.Range(0, self.Count)];
    }

    public static T GetRandom<T>(this T[] self)
    {
        return self[UnityEngine.Random.Range(0, self.Length)];
    }

    public static T PopRandom<T>(this List<T> self)
    {
        int randomIdx = UnityEngine.Random.Range(0, self.Count);
        T result = self[randomIdx];
        self.RemoveAt(randomIdx);
        return result;
    }
}
