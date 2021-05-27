using System;
using System.Drawing;
using System.Globalization;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;
using Il2CppSystem.Text.RegularExpressions;
using InnerNet;
using UnityEngine;
using UnityEngine.Events;
namespace HideGameCode
{
    [BepInPlugin(Id, "HideGameCode", "1.1.2")]
    [BepInProcess("Among Us.exe")]
    public class HideGameCodePlugin : BasePlugin
    {
        public const string Id = "com.herysia.hidegamecode";
        public Harmony Harmony { get; } = new Harmony(Id);
        public static HideGameCodePlugin Instance;
        public static ConfigEntry<string> Placeholder { get; set; }
        public static ConfigEntry<string> CodeColor { get; set; }


        public override void Load()
        {
            Placeholder = Config.Bind("Config", "Placeholder", "TWITCH\n\rPRIME");
            CodeColor = Config.Bind("Config", "Color", "#9147ff");
            Harmony.PatchAll();
        }

        [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Start))]
        public static class GameStartManager_Start
        {
            public static void copyToClipboard()
            {
                GUIUtility.systemCopyBuffer = GameCode.IntToGameName(AmongUsClient.Instance.GameId);
            }

            public static void Postfix(GameStartManager __instance)
            {
                __instance.GameRoomName.text = $"<color={HideGameCodePlugin.CodeColor.Value}>{HideGameCodePlugin.Placeholder.Value}</color>";
                __instance.GameRoomName.transform.localPosition = new Vector3(0.0f, -0.95f);
                copyToClipboard();
                var btn = __instance.MakePublicButton.GetComponent<PassiveButton>();
                btn.OnClick.AddListener((UnityAction) copyToClipboard);
            }
        }

        [HarmonyPatch(typeof(TextBoxTMP), nameof(TextBoxTMP.SetText))]
        public static class TextBoxTMP_Update {
            private static Regex pattern = new Regex(".+");
            public static void Postfix(TextBoxTMP __instance) {
                if (__instance.name == "GameIdText")
                {
                    __instance.outputText.text = new String('*', __instance.text.Length);
                }
            }
        }
    }
}