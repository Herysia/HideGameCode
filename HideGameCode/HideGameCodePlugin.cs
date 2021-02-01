using System;
using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using Il2CppSystem.Text.RegularExpressions;
using Reactor;
using UnityEngine;
using UnityEngine.Events;

namespace HideGameCode
{
    [BepInPlugin(Id)]
    [BepInProcess("Among Us.exe")]
    [BepInDependency(ReactorPlugin.Id)]
    public class HideGameCodePlugin : BasePlugin
    {
        public const string Id = "com.herysia.hidegamecode";

        public Harmony Harmony { get; } = new Harmony(Id);


        public override void Load()
        {
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
                __instance.GameRoomName.Text = "TWITCH\r\nPRIME";
                __instance.GameRoomName.Color = new Color(145 / 255f, 71 / 255f, 255 / 255f);//Twitch color
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