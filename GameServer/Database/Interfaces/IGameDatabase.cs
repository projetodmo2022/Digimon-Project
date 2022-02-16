using System.Collections.Generic;
using GameServer.Network;
using MySql.Data.MySqlClient;
using Yggdrasil.Entities;
using Yggdrasil.Helpers;

namespace GameServer.Database.Interfaces
{
    public interface IGameDatabase
    {
        public void UserDataExecute(string query, params object[] args);
        public List<Dictionary<string, object>> UserDataQuery(string query, params object[] args);
        public MySqlConnection Connects();
        public void ResetModel(uint DigiId, int digiType);
        public Digimon GetDigimon(uint DigiId);
        public uint CreateMercenary(uint charId, string digiName, int digiModel, int digiScale, int digiSize, int intimacy);
        public void LoadUser(GameClient client, uint AccountID, int UniId);
        public Position GetTamerPosition(uint AccountID);
        public Character LoadTamer(int AccountID);
        public void LoadTamer(GameClient client);
        public Digimon LoadDigimon(uint DigiId);
        public void SaveClient(GameClient client);
        public void SaveTamer(GameClient client);
        public void SaveDigimon(Digimon digimon);
        public void SaveTamerPosition(GameClient client);
    }
}