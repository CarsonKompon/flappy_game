using Sandbox;
using Sandbox.Menu;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlappyGame;

enum NETWORK_MESSAGE
{
    NONE,
    PLAYER_SPAWN,
    PLAYER_UPDATE,
    PLAYER_KILL,
    NEW_HIGHSCORE
}

public partial class GameMenu
{
    void OnNetworkMessage(ILobby.NetworkMessage msg)
    {
        if(msg.Source.Id == Game.SteamId) return;

        ByteStream data = msg.Data;

        ushort messageId = data.Read<ushort>();

        switch((NETWORK_MESSAGE)messageId)
        {
            case NETWORK_MESSAGE.PLAYER_SPAWN:
                SpawnPlayer(msg.Source.Id);
                break;
            
            case NETWORK_MESSAGE.PLAYER_UPDATE:

                bool hasPlayer = false;
                foreach(var child in GamePanel.Children)
                {
                    if(child is Player player && player.Member.Id == msg.Source.Id)
                    {
                        hasPlayer = true;
                        break;
                    }
                }
                if(!hasPlayer) SpawnPlayer(msg.Source.Id);

                ulong x = data.Read<ulong>();
                ulong y = data.Read<ulong>();
                float vspd = data.Read<float>();
                UpdatePlayer(msg.Source.Id, x, y, vspd);
                break;

            case NETWORK_MESSAGE.PLAYER_KILL:
                Kill(msg.Source.Id);
                break;
            
            case NETWORK_MESSAGE.NEW_HIGHSCORE:
                ulong score = data.Read<ulong>();
                Friend friend = new Friend(msg.Source.Id);
                CreateChatEntry(friend.Name, " got a new PB of " + score + "!", "highscore", friend.Id);
                break;
        }
    }

    void NetworkPlayerSpawn()
    {
        ByteStream data = ByteStream.Create(2);
        data.Write((ushort)NETWORK_MESSAGE.PLAYER_SPAWN);

        Lobby.BroadcastMessage(data);
    }

    public void NetworkPlayerUpdate(Player player)
    {
        ByteStream data = ByteStream.Create(22);
        data.Write((ushort)NETWORK_MESSAGE.PLAYER_UPDATE);
        data.Write((ulong)player.Position.x);
        data.Write((ulong)player.Position.y);
        data.Write((float)player.Vspd);

        Lobby.BroadcastMessage(data);
    }

    void NetworkPlayerKill()
    {
        ByteStream data = ByteStream.Create(2);
        data.Write((ushort)NETWORK_MESSAGE.PLAYER_KILL);

        Lobby?.BroadcastMessage(data);
    }

    void NetworkNewHighscore(ulong score)
    {
        ByteStream data = ByteStream.Create(2);
        data.Write((ushort)NETWORK_MESSAGE.NEW_HIGHSCORE);
        data.Write((ulong)score);

        Lobby?.BroadcastMessage(data);
    }
}