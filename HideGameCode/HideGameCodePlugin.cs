using System;
using System.Drawing;
using System.Globalization;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;
using Il2CppSystem.Text.RegularExpressions;
using InnerNet;
using Reactor;
using UnityEngine;
using UnityEngine.Events;
using Color = UnityEngine.Color;

namespace HideGameCode
{
    [BepInPlugin(Id)]
    [BepInProcess("Among Us.exe")]
    [BepInDependency(ReactorPlugin.Id)]
    public class HideGameCodePlugin : BasePlugin
    {
        public const string Id = "com.herysia.hidegamecode";

        public Harmony Harmony { get; } = new Harmony(Id);
        public ConfigEntry<string> Placeholder { get; private set; }
        public ConfigEntry<string> CodeColor { get; private set; }


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
                __instance.GameRoomName.Text = PluginSingleton<HideGameCodePlugin>.Instance.Placeholder.Value;
                int rgb = Int32.Parse(PluginSingleton<HideGameCodePlugin>.Instance.CodeColor.Value.Replace("#", ""), NumberStyles.HexNumber);
                __instance.GameRoomName.Color = new Color(((rgb >> 16) & 0xff) / 255f, ((rgb >> 8) & 0xff) / 255f, (rgb & 0xff) / 255f);
                copyToClipboard();
                var btn = __instance.MakePublicButton.GetComponent<PassiveButton>();
                btn.OnClick.AddListener((UnityAction) copyToClipboard);
            }
        }

        [HarmonyPatch(typeof(TextBox), nameof(TextBox.SetText))]
        public static class TextBox_Update
        {
            private static Regex pattern = new Regex(".+");

            public static void Postfix(TextBox __instance)
            {
                if (__instance.name == "GameIdText")
                {
                    __instance.outputText.Text = new String('*', __instance.text.Length);
                }
            }
        }
    }
}