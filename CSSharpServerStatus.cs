using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API.Modules.Entities;

namespace CSSharpServerStatus
{
    public class CSSharpServerStatus : BasePlugin
    {
        public override string ModuleName => "CS# Server status";
        public override string ModuleVersion => "1.0.0";
        public override string ModuleAuthor => "HKS 27D";
        public override string ModuleDescription => "";

        [ConsoleCommand("css_status", "Server status")]
        [RequiresPermissions("@css/chat")]
        [CommandHelper(whoCanExecute: CommandUsage.CLIENT_AND_SERVER)]
        public void OnLastCommand(CCSPlayerController? Player, CommandInfo commandInfo)
        {
            if (Player != null && Player.IsValid && Player.PlayerPawn.IsValid && Player.Connected == PlayerConnectedState.PlayerConnected)
            {
                Player.PrintToChat($" {ChatColors.Red}Modders {ChatColors.Default}- See console for {ChatColors.Green}server status {ChatColors.Default}output.");
                Player.PrintToConsole("UserID | SteamID64         | IPv4 address    | Nickname");
                Player.PrintToConsole("-------------------------------------------------------");

                int OnlinePlayers = 0;
                var PlayersEntities = Utilities.FindAllEntitiesByDesignerName<CCSPlayerController>("cs_player_controller");
                foreach (var PlayerEntity in PlayersEntities)
                {
                    if (PlayerEntity != null && PlayerEntity.IsValid && PlayerEntity.PlayerPawn.IsValid && PlayerEntity.Connected == PlayerConnectedState.PlayerConnected && PlayerEntity.UserId.HasValue)
                    {
                        OnlinePlayers++;
                        if (PlayerEntity.IsBot)
                            Player.PrintToConsole($"#{PlayerEntity.UserId,-5} | {"BOT",-17} | {"BOT PROTECTED",-15} | {PlayerEntity.PlayerName}");
                        else if ((AdminManager.PlayerHasPermissions(PlayerEntity, "@css/root")))
                            Player.PrintToConsole($"#{PlayerEntity.UserId,-5} | {PlayerEntity.SteamID,-17} | {"ROOT PROTECTED",-15} | {PlayerEntity.PlayerName}");
                        else if (AdminManager.PlayerHasPermissions(PlayerEntity, "@css/generic"))
                            Player.PrintToConsole($"#{PlayerEntity.UserId,-5} | {PlayerEntity.SteamID,-17} | {"PROTECTED",-15} | {PlayerEntity.PlayerName}");
                        else
                        {
                            if (PlayerEntity.IpAddress!.Contains(':'))
                                Player.PrintToConsole($"#{PlayerEntity.UserId,-5} | {PlayerEntity.SteamID,-17} | {PlayerEntity.IpAddress[..PlayerEntity.IpAddress.IndexOf(':')],-15} | {PlayerEntity.PlayerName}");
                            else
                            {
                                Player.PrintToConsole($"#{PlayerEntity.UserId,-5} | {PlayerEntity.SteamID,-17} | {PlayerEntity.IpAddress ?? "",-15} | {PlayerEntity.PlayerName}");
                            }
                        }
                    }
                }
                if (OnlinePlayers == 0)
                    Player.PrintToConsole($"0 player(s) online");
                else
                {
                    Player.PrintToConsole("-------------------");
                    Player.PrintToConsole($"{OnlinePlayers,-2} player(s) online");
                }
            }
            else
            {
                commandInfo.ReplyToCommand("UserID | SteamID64         | IPv4 address    | Nickname");
                commandInfo.ReplyToCommand("-------------------------------------------------------");

                int OnlinePlayers = 0;
                var PlayersEntities = Utilities.FindAllEntitiesByDesignerName<CCSPlayerController>("cs_player_controller");
                foreach (var PlayerEntity in PlayersEntities)
                {
                    if (PlayerEntity != null && PlayerEntity.IsValid && PlayerEntity.PlayerPawn.IsValid && PlayerEntity.Connected == PlayerConnectedState.PlayerConnected && PlayerEntity.UserId.HasValue)
                    {
                        OnlinePlayers++;
                        if (PlayerEntity.IsBot)
                            commandInfo.ReplyToCommand($"#{PlayerEntity.UserId,-5} | {"BOT",-17} | {"BOT PROTECTED",-15} | {PlayerEntity.PlayerName}");
                        else if (AdminManager.PlayerHasPermissions(PlayerEntity, "@css/root"))
                            commandInfo.ReplyToCommand($"#{PlayerEntity.UserId,-5} | {PlayerEntity.SteamID,-17} | {"ROOT PROTECTED",-15} | {PlayerEntity.PlayerName}");
                        else if (AdminManager.PlayerHasPermissions(PlayerEntity, "@css/generic"))
                            commandInfo.ReplyToCommand($"#{PlayerEntity.UserId,-5} | {PlayerEntity.SteamID,-17} | {"PROTECTED",-15} | {PlayerEntity.PlayerName}");
                        else
                        {
                            if (PlayerEntity.IpAddress!.Contains(':'))
                                commandInfo.ReplyToCommand($"#{PlayerEntity.UserId,-5} | {PlayerEntity.SteamID,-17} | {PlayerEntity.IpAddress[..PlayerEntity.IpAddress.IndexOf(':')],-15} | {PlayerEntity.PlayerName}");
                            else
                            {
                                commandInfo.ReplyToCommand($"#{PlayerEntity.UserId,-5} | {PlayerEntity.SteamID,-17} | {PlayerEntity.IpAddress ?? "",-15} | {PlayerEntity.PlayerName}");
                            }
                        }
                    }
                }
                if (OnlinePlayers == 0)
                    commandInfo.ReplyToCommand($"0 player(s) online");
                else
                {
                    commandInfo.ReplyToCommand("-------------------");
                    commandInfo.ReplyToCommand($"{OnlinePlayers,-2} player(s) online");
                }
            }
        }
    }
};
