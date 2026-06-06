using System.Reflection;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using MegaCrit.Sts2.Core.Nodes.Screens.CustomRun;
using MegaCrit.Sts2.Core.Nodes.Screens.DailyRun;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Platform;
using SlayTheSpire2.LAN.Multiplayer.Helpers;
using SlayTheSpire2.LAN.Multiplayer.Services;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace SlayTheSpire2.LAN.Multiplayer.Patchs.Screens
{
    [HarmonyPatch]
    internal class RunLoadScreenInitializeAsHostPatchs
    {
        private static IEnumerable<MethodInfo> TargetMethods()
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;

            yield return typeof(NMultiplayerLoadGameScreen).GetMethod("InitializeAsHost", flags)!;
            yield return typeof(NDailyRunLoadScreen).GetMethod("InitializeAsHost", flags)!;
            yield return typeof(NCustomRunLoadScreen).GetMethod("InitializeAsHost", flags)!;
        }

        private static void Prefix(NSubmenu __instance, INetGameService gameService)
        {
            if (gameService.Platform == PlatformType.None)
            {
                var runScreenService = RunScreenService.Instance;

                switch (__instance)
                {
                    case NMultiplayerLoadGameScreen multiplayerLoadGameScreen:
                        runScreenService.MultiplayerLoadGameScreen = multiplayerLoadGameScreen;
                        break;
                    case NDailyRunLoadScreen dailyRunLoadScreen:
                        runScreenService.DailyRunLoadScreen = dailyRunLoadScreen;
                        break;
                    case NCustomRunLoadScreen customRunLoadScreen:
                        runScreenService.CustomRunLoadScreen = customRunLoadScreen;
                        break;
                }
            }
        }
    }

    [HarmonyPatch]
    internal class RunLoadScreenInitializeAsClientPatchs
    {
        private static IEnumerable<MethodInfo> TargetMethods()
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;

            yield return typeof(NMultiplayerLoadGameScreen).GetMethod("InitializeAsClient", flags)!;
            yield return typeof(NDailyRunLoadScreen).GetMethod("InitializeAsClient", flags)!;
            yield return typeof(NCustomRunLoadScreen).GetMethod("InitializeAsClient", flags)!;
        }

        private static void Prefix(NSubmenu __instance, INetGameService gameService)
        {
            if (gameService.Platform == PlatformType.None)
            {
                var runScreenService = RunScreenService.Instance;

                switch (__instance)
                {
                    case NMultiplayerLoadGameScreen multiplayerLoadGameScreen:
                        runScreenService.MultiplayerLoadGameScreen = multiplayerLoadGameScreen;
                        break;
                    case NDailyRunLoadScreen dailyRunLoadScreen:
                        runScreenService.DailyRunLoadScreen = dailyRunLoadScreen;
                        break;
                    case NCustomRunLoadScreen customRunLoadScreen:
                        runScreenService.CustomRunLoadScreen = customRunLoadScreen;
                        break;
                }
            }
        }
    }

    [HarmonyPatch]
    internal class RunLoadScreenShouldAllowRunToBeginPatchs
    {
        private static IEnumerable<MethodInfo> TargetMethods()
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;

            yield return typeof(NDailyRunLoadScreen).GetMethod("ShouldAllowRunToBegin", flags)!;
            yield return typeof(NCustomRunLoadScreen).GetMethod("ShouldAllowRunToBegin", flags)!;
        }

        private static bool Prefix(LoadRunLobby ____lobby, ref Task<bool> __result)
        {
            if (____lobby.NetService.Platform == PlatformType.None)
            {
                __result = TaskGenericHelper.RunSafely(RunScreenService.ShouldAllowRunToBegin(____lobby));
                return false;
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(NMultiplayerLoadGameScreen), "ShouldAllowRunToBegin")]
    internal class RunLoadScreenShouldAllowRunToBeginPatch
    {
        private static bool Prefix(LoadRunLobby ____runLobby, ref Task<bool> __result)
        {
            if (____runLobby.NetService.Platform == PlatformType.None)
            {
                __result = TaskGenericHelper.RunSafely(RunScreenService.ShouldAllowRunToBegin(____runLobby));
                return false;
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(NMultiplayerLoadGameScreen), "_Process")]
    internal class MultiplayerLoadGameScreenProcessPatch
    {
        private static void Postfix(NMultiplayerLoadGameScreen __instance, LoadRunLobby ____runLobby)
        {
            LoadScreenButtonHelper.DisableConfirmButtonIfPlayersMissing(__instance, ____runLobby);
        }
    }

    [HarmonyPatch(typeof(NDailyRunLoadScreen), "_Process")]
    internal class DailyRunLoadScreenProcessPatch
    {
        private static void Postfix(NSubmenu __instance, LoadRunLobby ____lobby)
        {
            LoadScreenButtonHelper.DisableConfirmButtonIfPlayersMissing(__instance, ____lobby);
        }
    }

    [HarmonyPatch(typeof(NCustomRunLoadScreen), "_Process")]
    internal class CustomRunLoadScreenProcessPatch
    {
        private static void Postfix(NSubmenu __instance, LoadRunLobby ____lobby)
        {
            LoadScreenButtonHelper.DisableConfirmButtonIfPlayersMissing(__instance, ____lobby);
        }
    }

    internal static class LoadScreenButtonHelper
    {
        internal static void DisableConfirmButtonIfPlayersMissing(NSubmenu instance, LoadRunLobby lobby)
        {
            if (lobby?.NetService is not { Platform: PlatformType.None })
                return;

            if (lobby.ConnectedPlayerIds.Count >= lobby.Run.Players.Count)
                return;

            var confirmButton =
                Traverse.Create(instance).Field("_confirmButton").GetValue<BaseButton>();
            if (confirmButton != null)
            {
                confirmButton.Disabled = true;
            }
        }
    }
}