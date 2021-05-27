using System.Globalization;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace HideGameCode
{
    [HarmonyPatch]
    public static class VersionPingPatch 
    {
        [HarmonyPatch(typeof(VersionShower), nameof(VersionShower.Start))]
        private static class VersionShowerPatch 
        {
            static void Postfix(VersionShower __instance) 
            {
            __instance.text.text = $"{__instance.text.text}<color=#00FF00>\nHideGameCode v1.1.0\n<size=70%>by Herysia";
			__instance.text.alignment = TMPro.TextAlignmentOptions.BaselineRight;
			__instance.text.margin = new Vector4(0, 0, 0.5f, 0);
			__instance.text.transform.localPosition = new Vector3(0, 0, 0);
			Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
			__instance.text.transform.position = new Vector3(topRight.x - 0.1f, topRight.y - 0.3f);
            }
        }
    }
}