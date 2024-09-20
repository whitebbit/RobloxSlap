using System;
using System.Collections.Generic;


namespace GBGamesPlugin
{
    public partial class GBGames
    {
        private static readonly Dictionary<string, string> PlayerWordTranslate = new()
        {
            {"ru", "Игрок"}, {"en", "Player"}, {"fr", "Joueur"}, {"it", "Giocatore"}, {"de", "Spieler"},
            {"es", "Jugador"},
            {"zh", "玩家 "}, {"pt", "Jogador"}, {"ko", "플레이어"}, {"ja", "プレイヤー"}, {"tr", "oyuncu"}, {"ar", "لاعب "},
            {"hi", "खिलाड़ी "}, {"id", "Pemain"},
        };
        
    }
}