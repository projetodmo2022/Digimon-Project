using System.Collections.Generic;
using CharServer.Network;
using Yggdrasil.Entities;
using Yggdrasil.Helpers;

namespace CharServer.Database.Interfaces
{
    public interface ICharacterDatabase
    {
        public void UserDataExecute(string query, params object[] args);
        public List<Dictionary<string, object>> UserDataQuery(string query, params object[] args);
        public List<Character> GetCharacters(int AccountID);
        public void LoadUser(CharClient client);
        public void LoadUser(CharClient client, uint AccountID, int UniId);
        public List<Character> GetCharacters(uint AcctId);
        public Digimon GetDigimon(uint DigiId);
        public void ResetModel(uint DigiId, int digiType);
        public uint CreateDigimon(uint charId, string digiName, int digiModel);
        public uint CreateMercenary(uint charId, string digiName, int digiModel, int digiScale, int digiSize, int intimacy);
        public int CreateCharacter(int position, uint AcctId, int pos, int charModel, string charName, int digiModel);
        public void SetPartner(int charId, int digiId);
        public void SetTamer(int charId, int digiId);
        public bool VerifyCode(uint acctId, string code);
        public bool DeleteTamer(uint acctId, int slot);
        public void DeleteDigimons(int charId);
        public void SetLastChar(uint acctId, int slot);
        public bool NameAvail(string name);
        public Position GetTamerPosition(uint acctId, int slot);

    }
}