#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using System;
using Microsoft.Xna.Framework;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Mvvm;
using EmptyKeys.UserInterface.Generated;
using GameUILibrary.Models;
using Welt.Extensions;
using Welt.Core.Forge;
using System.Net;
using System.Collections.Generic;
using System.Linq;

namespace Welt.Scenes
{
    public class MainMenuScene : Scene
    {
        private MainMenu MainMenuUi => new MainMenu
        {
            DataContext = m_MainMenuViewModel
        };
        private SingleplayerMenu SpUi => new SingleplayerMenu
        {
            DataContext = m_SingleplayerViewModel
        };
        private MultiplayerMenu MpUi => new MultiplayerMenu
        {
            DataContext = m_MultiplayerViewModel
        };
        private SettingsMenu SeUi => new SettingsMenu
        {
            DataContext = m_SettingsViewModel
        };

        private MainMenuViewModel m_MainMenuViewModel;
        private SingleplayerViewModel m_SingleplayerViewModel;
        private MultiplayerViewModel m_MultiplayerViewModel;
        private SettingsModel m_SettingsViewModel;
        private List<ServerModel> m_Servers;

        internal override ViewModelBase DataContext { get; set; }
        internal override UIRoot UI { get; set; } = new MainMenu();
        internal override Color BackColor => Color.FromNonPremultiplied(15, 15, 15, 255);

        public MainMenuScene(WeltGame game) : base(game)
        {
            m_Servers = new List<ServerModel>();
            m_MainMenuViewModel = new MainMenuViewModel
            {
                SingleplayerButtonCommand =
                    new Action(SwitchToSingleplayer).CreateButtonCommand(),
                MultiplayerButtonCommand =
                    new Action(SwitchToMultiplayer).CreateButtonCommand(),
                SettingsButtonCommand =
                    new Action(SwitchToSettings).CreateButtonCommand(),
                ExitButtonCommand =
                    new Action(() => Game.Exit()).CreateButtonCommand(),
            };
            m_SingleplayerViewModel = new SingleplayerViewModel
            {
                WorldName = "New World",
                WorldSeed = "12345",
                CreateWorldCommand =
                    new Action(() => Next(new LoadScene(Game))).CreateButtonCommand(),
                ExitCommand =
                    new Action(SwitchToMainMenu).CreateButtonCommand()
            };
            m_MultiplayerViewModel = new MultiplayerViewModel
            {
                ServerList = m_Servers.ToArray(),
                AddNewIPCommand =
                    new Action(() => ResolveServerInfo(m_MultiplayerViewModel.NewIPAddressText)).CreateButtonCommand(),
                ExitCommand =
                    new Action(SwitchToMainMenu).CreateButtonCommand()
            };
            m_SettingsViewModel = new SettingsModel
            {
                ExitCommand =
                    new Action(SwitchToMainMenu).CreateButtonCommand()
            };
            
            SwitchToMainMenu();
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(GameTime time)
        {
            
        }

        public override void Initialize()
        {
            Game.Client.ServerDiscovered += (sender, args) =>
            {
                m_Servers.Add(new ServerModel
                {
                    ServerName = args.ServerName,
                    ServerMotd = args.MessageOfTheDay,
                    CurrentPlayerCount = $"{args.CurrentPlayers}/{args.MaxPlayers}",
                    JoinServerCommand = new Action(() => Next(new LoadScene(Game))).CreateButtonCommand()
                });
                m_MultiplayerViewModel.ServerList = m_Servers.ToArray();
                m_MultiplayerViewModel.SetStatus($"Discovered {args.ServerName}");
                //Game.Client.Disconnect();
            };
        }

        private void SwitchToMainMenu()
        {
            UI = MainMenuUi;
            //UI.DataContext = MainMenuVm;
        }

        private void SwitchToSingleplayer()
        {
            UI = SpUi;
            //UI.DataContext = SpVm;
        }

        private void SwitchToMultiplayer()
        {
            UI = MpUi;
        }

        private void SwitchToSettings()
        {
            UI = SeUi;
            //UI.DataContext = SeVm;
        }

        private void ResolveServerInfo(string serverIp)
        {
            m_MultiplayerViewModel.IsCompletedAddition = false;
            m_MultiplayerViewModel.SetStatus($"Resolving server at {serverIp}...");

            IPAddress address;
            int port = 3456;

            if (serverIp.ToLower() == "localhost")
                address = IPAddress.Parse("127.0.0.1");
            else if (!IPAddress.TryParse(serverIp.Split(':')[0], out address))
            {
                m_MultiplayerViewModel.SetStatus("Could not resolve host.");
                return;
            }
            if (serverIp.Contains(":") && !int.TryParse(serverIp.Split(':')[1], out port))
            {
                m_MultiplayerViewModel.SetStatus("Could not resolve port.");
                return;
            }
            Game.Client.Connect(new IPEndPoint(address, port));
            m_MultiplayerViewModel.IsCompletedAddition = true;
            m_MultiplayerViewModel.SetStatus("");
            //Game.Client.Disconnect();
        }
    }
}
