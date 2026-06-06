using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using MegaCrit.Sts2.Core.Nodes.Screens.CustomRun;
using MegaCrit.Sts2.Core.Nodes.Screens.DailyRun;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;

namespace SlayTheSpire2.LAN.Multiplayer.Services
{
    internal class RunScreenService
    {
        private static readonly Lazy<RunScreenService> Lazy = new(() => new RunScreenService());

        public static RunScreenService Instance => Lazy.Value;

        public NCharacterSelectScreen? CharacterSelectScreen;

        public NDailyRunScreen? DailyRunScreen;

        public NCustomRunScreen? CustomRunScreen;

        public NMultiplayerLoadGameScreen? MultiplayerLoadGameScreen;

        public NDailyRunLoadScreen? DailyRunLoadScreen;

        public NCustomRunLoadScreen? CustomRunLoadScreen;

        private RunScreenService()
        {
        }

        public static async Task<bool> ShouldAllowRunToBegin(LoadRunLobby runLobby)
        {
            if (runLobby.ConnectedPlayerIds.Count >= runLobby.Run.Players.Count)
                return true;

            var locString = new LocString("gameplay_ui", "CONFIRM_LOAD_SAVE.body");
            locString.Add("MissingCount", runLobby.Run.Players.Count - runLobby.ConnectedPlayerIds.Count);

            var nGenericPopup = NGenericPopup.Create();
            if (nGenericPopup == null)
            {
                while (runLobby.ConnectedPlayerIds.Count < runLobby.Run.Players.Count)
                {
                    await Task.Delay(500);
                }

                return true;
            }

            NModalContainer.Instance?.Add(nGenericPopup);

            var nVerticalPopup = Traverse.Create(nGenericPopup).Field("_verticalPopup").GetValue<NVerticalPopup>();
            nVerticalPopup.YesButton.Visible = false;
            nVerticalPopup.NoButton.SetAnchorsPreset(Control.LayoutPreset.CenterBottom);
            nVerticalPopup.NoButton.OffsetLeft = -90;
            nVerticalPopup.NoButton.OffsetTop = -152;
            nVerticalPopup.NoButton.OffsetRight = 90;
            nVerticalPopup.NoButton.OffsetBottom = -80;

            var confirmationTask = nGenericPopup.WaitForConfirmation(locString,
                new LocString("gameplay_ui", "CONFIRM_LOAD_SAVE.header"),
                new LocString("gameplay_ui", "CONFIRM_LOAD_SAVE.cancel"),
                new LocString("gameplay_ui", "CONFIRM_LOAD_SAVE.confirm"));

            while (runLobby.ConnectedPlayerIds.Count < runLobby.Run.Players.Count)
            {
                if (confirmationTask.IsCompleted)
                {
                    try
                    {
                        NavigateBackFromLoadScreen();
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"Failed to navigate back from load screen: {ex}");
                    }

                    return true;
                }

                await Task.Delay(500);
            }

            try
            {
                nGenericPopup.QueueFree();
            }
            catch
            {
                // ignored
            }

            return true;
        }

        private static void NavigateBackFromLoadScreen()
        {
            Control? loadScreen = Instance.MultiplayerLoadGameScreen
                                  ?? (Control?)Instance.DailyRunLoadScreen
                                  ?? (Control?)Instance.CustomRunLoadScreen;

            if (loadScreen == null)
                return;

            var backButton = Traverse.Create(loadScreen).Field("_backButton").GetValue<NBackButton>();
            if (backButton != null)
            {
                backButton.CallDeferred("emit_signal", NClickableControl.SignalName.Released,
                    (NButton)(object)backButton);
            }
        }
    }
}
