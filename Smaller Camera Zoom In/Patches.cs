using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Smaller_Camera_Zoom_In
{
    public class Patches
    {
        [HarmonyPatch(typeof(CameraController), "OnPrefabInit")]
        public class OnPrefabInit
        {
            public static void Postfix(CameraController __instance)
            {
                __instance.minOrthographicSize = 1f;
            }
        }

    }
}
