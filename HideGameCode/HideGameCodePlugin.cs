using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
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
                copyToClipboard();
                var btn = __instance.MakePublicButton.GetComponent<PassiveButton>();
                btn.OnClick.AddListener((UnityAction)copyToClipboard);
            }
        }
    }
}
