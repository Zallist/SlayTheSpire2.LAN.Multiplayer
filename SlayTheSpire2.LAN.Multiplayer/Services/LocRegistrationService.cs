using System.Collections.Generic;
using HarmonyLib;
using MegaCrit.Sts2.Core.Localization;

namespace SlayTheSpire2.LAN.Multiplayer.Services
{
    internal static class LocRegistrationService
    {
        private static bool _registered;
        private static readonly Dictionary<string, Dictionary<string, string>> MainMenuUi = new()
        {
            ["eng"] = new()
            {
                ["SlayTheSpire2.LAN.Multiplayer.COPIED"] = "Copied",
                ["SlayTheSpire2.LAN.Multiplayer.IP_ADDRESS_TITLE"] = "IP Address:",
                ["SlayTheSpire2.LAN.Multiplayer.LOCAL_IP_ADDRESS_TITLE"] = "Local IP Address:",
                ["SlayTheSpire2.LAN.Multiplayer.IPV6_ADDRESS_TITLE"] = "IPV6 Address:"
            },
            ["zhs"] = new()
            {
                ["SlayTheSpire2.LAN.Multiplayer.COPIED"] = "已复制",
                ["SlayTheSpire2.LAN.Multiplayer.IP_ADDRESS_TITLE"] = "IP地址:",
                ["SlayTheSpire2.LAN.Multiplayer.LOCAL_IP_ADDRESS_TITLE"] = "本地IP地址:",
                ["SlayTheSpire2.LAN.Multiplayer.IPV6_ADDRESS_TITLE"] = "IPV6地址:"
            },
            ["jpn"] = new()
            {
                ["SlayTheSpire2.LAN.Multiplayer.COPIED"] = "コピーしました",
                ["SlayTheSpire2.LAN.Multiplayer.IP_ADDRESS_TITLE"] = "IPアドレス:",
                ["SlayTheSpire2.LAN.Multiplayer.LOCAL_IP_ADDRESS_TITLE"] = "ローカルIPアドレス:",
                ["SlayTheSpire2.LAN.Multiplayer.IPV6_ADDRESS_TITLE"] = "IPV6アドレス:"
            },
            ["kor"] = new()
            {
                ["SlayTheSpire2.LAN.Multiplayer.COPIED"] = "복사됨",
                ["SlayTheSpire2.LAN.Multiplayer.IP_ADDRESS_TITLE"] = "IP주소:",
                ["SlayTheSpire2.LAN.Multiplayer.LOCAL_IP_ADDRESS_TITLE"] = "로컬IP주소:",
                ["SlayTheSpire2.LAN.Multiplayer.IPV6_ADDRESS_TITLE"] = "IPV6주소:"
            },
            ["fra"] = new()
            {
                ["SlayTheSpire2.LAN.Multiplayer.COPIED"] = "Copié",
                ["SlayTheSpire2.LAN.Multiplayer.IP_ADDRESS_TITLE"] = "Adresse IP:",
                ["SlayTheSpire2.LAN.Multiplayer.LOCAL_IP_ADDRESS_TITLE"] = "Adresse IP locale:",
                ["SlayTheSpire2.LAN.Multiplayer.IPV6_ADDRESS_TITLE"] = "Adresse IPV6:"
            },
            ["deu"] = new()
            {
                ["SlayTheSpire2.LAN.Multiplayer.COPIED"] = "Kopiert",
                ["SlayTheSpire2.LAN.Multiplayer.IP_ADDRESS_TITLE"] = "IP Adresse:",
                ["SlayTheSpire2.LAN.Multiplayer.LOCAL_IP_ADDRESS_TITLE"] = "Lokale IP Adresse:",
                ["SlayTheSpire2.LAN.Multiplayer.IPV6_ADDRESS_TITLE"] = "IPV6 Adresse:"
            },
            ["esp"] = new()
            {
                ["SlayTheSpire2.LAN.Multiplayer.COPIED"] = "Copiado",
                ["SlayTheSpire2.LAN.Multiplayer.IP_ADDRESS_TITLE"] = "Dirección IP:",
                ["SlayTheSpire2.LAN.Multiplayer.LOCAL_IP_ADDRESS_TITLE"] = "Dirección IP local:",
                ["SlayTheSpire2.LAN.Multiplayer.IPV6_ADDRESS_TITLE"] = "Dirección IPV6:"
            },
            ["spa"] = new()
            {
                ["SlayTheSpire2.LAN.Multiplayer.COPIED"] = "Copiado",
                ["SlayTheSpire2.LAN.Multiplayer.IP_ADDRESS_TITLE"] = "Dirección IP:",
                ["SlayTheSpire2.LAN.Multiplayer.LOCAL_IP_ADDRESS_TITLE"] = "Dirección IP local:",
                ["SlayTheSpire2.LAN.Multiplayer.IPV6_ADDRESS_TITLE"] = "Dirección IPV6:"
            },
            ["ita"] = new()
            {
                ["SlayTheSpire2.LAN.Multiplayer.COPIED"] = "Copiato",
                ["SlayTheSpire2.LAN.Multiplayer.IP_ADDRESS_TITLE"] = "Indirizzo IP:",
                ["SlayTheSpire2.LAN.Multiplayer.LOCAL_IP_ADDRESS_TITLE"] = "Indirizzo IP locale:",
                ["SlayTheSpire2.LAN.Multiplayer.IPV6_ADDRESS_TITLE"] = "Indirizzo IPV6:"
            },
            ["pol"] = new()
            {
                ["SlayTheSpire2.LAN.Multiplayer.COPIED"] = "Skopiowano",
                ["SlayTheSpire2.LAN.Multiplayer.IP_ADDRESS_TITLE"] = "Adres IP:",
                ["SlayTheSpire2.LAN.Multiplayer.LOCAL_IP_ADDRESS_TITLE"] = "Lokalny adres IP:",
                ["SlayTheSpire2.LAN.Multiplayer.IPV6_ADDRESS_TITLE"] = "Adres IPV6:"
            },
            ["ptb"] = new()
            {
                ["SlayTheSpire2.LAN.Multiplayer.COPIED"] = "Copiado",
                ["SlayTheSpire2.LAN.Multiplayer.IP_ADDRESS_TITLE"] = "Endereço IP:",
                ["SlayTheSpire2.LAN.Multiplayer.LOCAL_IP_ADDRESS_TITLE"] = "Endereço IP Local:",
                ["SlayTheSpire2.LAN.Multiplayer.IPV6_ADDRESS_TITLE"] = "Endereço IPV6:"
            },
            ["rus"] = new()
            {
                ["SlayTheSpire2.LAN.Multiplayer.COPIED"] = "Скопировано",
                ["SlayTheSpire2.LAN.Multiplayer.IP_ADDRESS_TITLE"] = "IP адрес:",
                ["SlayTheSpire2.LAN.Multiplayer.LOCAL_IP_ADDRESS_TITLE"] = "Локальный IP адрес:",
                ["SlayTheSpire2.LAN.Multiplayer.IPV6_ADDRESS_TITLE"] = "IPV6 адрес:"
            },
            ["tha"] = new()
            {
                ["SlayTheSpire2.LAN.Multiplayer.COPIED"] = "คัดลอกแล้ว",
                ["SlayTheSpire2.LAN.Multiplayer.IP_ADDRESS_TITLE"] = "ที่อยู่ IP:",
                ["SlayTheSpire2.LAN.Multiplayer.LOCAL_IP_ADDRESS_TITLE"] = "ที่อยู่ IP ภายใน:",
                ["SlayTheSpire2.LAN.Multiplayer.IPV6_ADDRESS_TITLE"] = "ที่อยู่ IPV6:"
            },
            ["tur"] = new()
            {
                ["SlayTheSpire2.LAN.Multiplayer.COPIED"] = "Kopyalandı",
                ["SlayTheSpire2.LAN.Multiplayer.IP_ADDRESS_TITLE"] = "IP Adresi:",
                ["SlayTheSpire2.LAN.Multiplayer.LOCAL_IP_ADDRESS_TITLE"] = "Yerel IP Adresi:",
                ["SlayTheSpire2.LAN.Multiplayer.IPV6_ADDRESS_TITLE"] = "IPv6 Adresi:"
            }
        };

        private static readonly Dictionary<string, Dictionary<string, string>> SettingsUi = new()
        {
            ["eng"] = new()
            {
                ["SlayTheSpire2.LAN.Multiplayer.HOST_PORT"] = "Host Port",
                ["SlayTheSpire2.LAN.Multiplayer.HOST_MAX_PLAYERS"] = "Host Max Players",
                ["SlayTheSpire2.LAN.Multiplayer.PLAYER_NAME"] = "Player Name",
                ["SlayTheSpire2.LAN.Multiplayer.NET_ID"] = "NetID"
            },
            ["zhs"] = new()
            {
                ["SlayTheSpire2.LAN.Multiplayer.HOST_PORT"] = "主机端口",
                ["SlayTheSpire2.LAN.Multiplayer.HOST_MAX_PLAYERS"] = "主机最大玩家数",
                ["SlayTheSpire2.LAN.Multiplayer.PLAYER_NAME"] = "玩家名称",
                ["SlayTheSpire2.LAN.Multiplayer.NET_ID"] = "NetID"
            },
            ["jpn"] = new()
            {
                ["SlayTheSpire2.LAN.Multiplayer.HOST_PORT"] = "ホストポート",
                ["SlayTheSpire2.LAN.Multiplayer.HOST_MAX_PLAYERS"] = "最大プレイヤー数",
                ["SlayTheSpire2.LAN.Multiplayer.PLAYER_NAME"] = "プレイヤー名",
                ["SlayTheSpire2.LAN.Multiplayer.NET_ID"] = "NetID"
            },
            ["kor"] = new()
            {
                ["SlayTheSpire2.LAN.Multiplayer.HOST_PORT"] = "호스트 포트",
                ["SlayTheSpire2.LAN.Multiplayer.HOST_MAX_PLAYERS"] = "호스트 최대 인원",
                ["SlayTheSpire2.LAN.Multiplayer.PLAYER_NAME"] = "플레이어 이름",
                ["SlayTheSpire2.LAN.Multiplayer.NET_ID"] = "NetID"
            },
            ["fra"] = new()
            {
                ["SlayTheSpire2.LAN.Multiplayer.HOST_PORT"] = "Port de l'hôte",
                ["SlayTheSpire2.LAN.Multiplayer.HOST_MAX_PLAYERS"] = "Nombre max de joueurs",
                ["SlayTheSpire2.LAN.Multiplayer.PLAYER_NAME"] = "Nom du joueur",
                ["SlayTheSpire2.LAN.Multiplayer.NET_ID"] = "NetID"
            },
            ["deu"] = new()
            {
                ["SlayTheSpire2.LAN.Multiplayer.HOST_PORT"] = "Host Port",
                ["SlayTheSpire2.LAN.Multiplayer.HOST_MAX_PLAYERS"] = "Max Spieleranzahl",
                ["SlayTheSpire2.LAN.Multiplayer.PLAYER_NAME"] = "Spielername",
                ["SlayTheSpire2.LAN.Multiplayer.NET_ID"] = "NetID"
            },
            ["esp"] = new()
            {
                ["SlayTheSpire2.LAN.Multiplayer.HOST_PORT"] = "Puerto del host",
                ["SlayTheSpire2.LAN.Multiplayer.HOST_MAX_PLAYERS"] = "Máximo de jugadores",
                ["SlayTheSpire2.LAN.Multiplayer.PLAYER_NAME"] = "Nombre del jugador",
                ["SlayTheSpire2.LAN.Multiplayer.NET_ID"] = "NetID"
            },
            ["spa"] = new()
            {
                ["SlayTheSpire2.LAN.Multiplayer.HOST_PORT"] = "Puerto del anfitrión",
                ["SlayTheSpire2.LAN.Multiplayer.HOST_MAX_PLAYERS"] = "Máximo de jugadores",
                ["SlayTheSpire2.LAN.Multiplayer.PLAYER_NAME"] = "Nombre del jugador",
                ["SlayTheSpire2.LAN.Multiplayer.NET_ID"] = "NetID"
            },
            ["ita"] = new()
            {
                ["SlayTheSpire2.LAN.Multiplayer.HOST_PORT"] = "Porta Host",
                ["SlayTheSpire2.LAN.Multiplayer.HOST_MAX_PLAYERS"] = "Numero massimo giocatori",
                ["SlayTheSpire2.LAN.Multiplayer.PLAYER_NAME"] = "Nome giocatore",
                ["SlayTheSpire2.LAN.Multiplayer.NET_ID"] = "NetID"
            },
            ["pol"] = new()
            {
                ["SlayTheSpire2.LAN.Multiplayer.HOST_PORT"] = "Port hosta",
                ["SlayTheSpire2.LAN.Multiplayer.HOST_MAX_PLAYERS"] = "Maks. liczba graczy",
                ["SlayTheSpire2.LAN.Multiplayer.PLAYER_NAME"] = "Nazwa gracza",
                ["SlayTheSpire2.LAN.Multiplayer.NET_ID"] = "NetID"
            },
            ["ptb"] = new()
            {
                ["SlayTheSpire2.LAN.Multiplayer.HOST_PORT"] = "Porta do Host",
                ["SlayTheSpire2.LAN.Multiplayer.HOST_MAX_PLAYERS"] = "Máximo de Jogadores",
                ["SlayTheSpire2.LAN.Multiplayer.PLAYER_NAME"] = "Nome do Jogador",
                ["SlayTheSpire2.LAN.Multiplayer.NET_ID"] = "NetID"
            },
            ["rus"] = new()
            {
                ["SlayTheSpire2.LAN.Multiplayer.HOST_PORT"] = "Порт хоста",
                ["SlayTheSpire2.LAN.Multiplayer.HOST_MAX_PLAYERS"] = "Макс. игроков",
                ["SlayTheSpire2.LAN.Multiplayer.PLAYER_NAME"] = "Имя игрока",
                ["SlayTheSpire2.LAN.Multiplayer.NET_ID"] = "NetID"
            },
            ["tha"] = new()
            {
                ["SlayTheSpire2.LAN.Multiplayer.HOST_PORT"] = "พอร์ตโฮสต์",
                ["SlayTheSpire2.LAN.Multiplayer.HOST_MAX_PLAYERS"] = "จำนวนผู้เล่นสูงสุด",
                ["SlayTheSpire2.LAN.Multiplayer.PLAYER_NAME"] = "ชื่อผู้เล่น",
                ["SlayTheSpire2.LAN.Multiplayer.NET_ID"] = "NetID"
            },
            ["tur"] = new()
            {
                ["SlayTheSpire2.LAN.Multiplayer.HOST_PORT"] = "Sunucu Portu",
                ["SlayTheSpire2.LAN.Multiplayer.HOST_MAX_PLAYERS"] = "Maksimum Oyuncu",
                ["SlayTheSpire2.LAN.Multiplayer.PLAYER_NAME"] = "Oyuncu Adı",
                ["SlayTheSpire2.LAN.Multiplayer.NET_ID"] = "NetID"
            }
        };

        private static readonly Dictionary<string, Dictionary<string, string>> GameplayUi = new()
        {
            ["eng"] = new()
            {
                ["SlayTheSpire2.LAN.Multiplayer.DISABLE_DRAWING"] = "Disable drawing"
            },
            ["zhs"] = new()
            {
                ["SlayTheSpire2.LAN.Multiplayer.DISABLE_DRAWING"] = "禁用绘图"
            },
            ["jpn"] = new()
            {
                ["SlayTheSpire2.LAN.Multiplayer.DISABLE_DRAWING"] = "描画を無効にする"
            },
            ["kor"] = new()
            {
                ["SlayTheSpire2.LAN.Multiplayer.DISABLE_DRAWING"] = "그리기 비활성화"
            },
            ["fra"] = new()
            {
                ["SlayTheSpire2.LAN.Multiplayer.DISABLE_DRAWING"] = "Désactiver le dessin"
            },
            ["deu"] = new()
            {
                ["SlayTheSpire2.LAN.Multiplayer.DISABLE_DRAWING"] = "Zeichnen deaktivieren"
            },
            ["esp"] = new()
            {
                ["SlayTheSpire2.LAN.Multiplayer.DISABLE_DRAWING"] = "Deshabilitar dibujo"
            },
            ["spa"] = new()
            {
                ["SlayTheSpire2.LAN.Multiplayer.DISABLE_DRAWING"] = "Deshabilitar dibujo"
            },
            ["ita"] = new()
            {
                ["SlayTheSpire2.LAN.Multiplayer.DISABLE_DRAWING"] = "Disabilita il disegno"
            },
            ["pol"] = new()
            {
                ["SlayTheSpire2.LAN.Multiplayer.DISABLE_DRAWING"] = "Wyłącz rysowanie"
            },
            ["ptb"] = new()
            {
                ["SlayTheSpire2.LAN.Multiplayer.DISABLE_DRAWING"] = "Desativar desenho"
            },
            ["rus"] = new()
            {
                ["SlayTheSpire2.LAN.Multiplayer.DISABLE_DRAWING"] = "Отключить рисование"
            },
            ["tha"] = new()
            {
                ["SlayTheSpire2.LAN.Multiplayer.DISABLE_DRAWING"] = "ปิดใช้งานการวาดภาพ"
            },
            ["tur"] = new()
            {
                ["SlayTheSpire2.LAN.Multiplayer.DISABLE_DRAWING"] = "Çizimi devre dışı bırak"
            }
        };

        public static void Register()
        {
            TryRegister();
        }

        internal static void TryRegister()
        {
            var manager = LocManager.Instance;
            if (manager == null) return;

            if (!_registered)
            {
                _registered = true;
                manager.SubscribeToLocaleChange(OnLocaleChanged);
            }

            var lang = manager.Language;

            RegisterTable(manager, "main_menu_ui", MainMenuUi, lang);
            RegisterTable(manager, "settings_ui", SettingsUi, lang);
            RegisterTable(manager, "gameplay_ui", GameplayUi, lang);
        }

        private static void OnLocaleChanged()
        {
            TryRegister();
        }

        private static void RegisterTable(LocManager manager, string tableName,
            Dictionary<string, Dictionary<string, string>> allLangData, string lang)
        {
            var table = manager.GetTable(tableName);
            if (table == null) return;

            if (!allLangData.TryGetValue(lang, out var data) && !allLangData.TryGetValue("eng", out data))
                return;

            table.MergeWith(data);
        }
    }

    [HarmonyPatch(typeof(LocManager), "Initialize")]
    internal static class LocManagerInitPatch
    {
        private static void Postfix()
        {
            LocRegistrationService.TryRegister();
        }
    }

    [HarmonyPatch(typeof(LocManager), "SetLanguage")]
    internal static class LocManagerSetLanguagePatch
    {
        private static void Postfix()
        {
            LocRegistrationService.TryRegister();
        }
    }
}
