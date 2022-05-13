using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoldfastSharedMethods;
using System.Linq;

public class Players : MonoBehaviour
{
    public static Dictionary<int, joinStruct> playerJoinedDictionary = new Dictionary<int, joinStruct>(); // Dizionario persone entrate

    public static void PlayerJoined(int playerId, ulong steamId, string name, string regimentTag, bool isBot)
    {
        if (!isBot) // Bot not counted
        {
            joinStruct temp = new joinStruct()
            {
                _steamId = steamId,
                _name = name,
                _regimentTag = regimentTag,
                _spawnedOneTime = false

            };

            playerJoinedDictionary[playerId] = temp; // Re

            Debug.Log("FENIX MOD: " + name + " added to the PlayerJoined dictionary"); 
        };

    }
    public static void PlayerLeft(int playerId)
    {
        playerJoinedDictionary.Remove(playerId);
        Debug.Log("FENIX MOD: " + playerId + " rimosso dal dizionario"); 
    }

    public static void PlayerSpawned(int playerId, int spawnSectionId, FactionCountry playerFaction, PlayerClass playerClass, int uniformId)
    {
        joinStruct temp;

        if (playerJoinedDictionary.TryGetValue(playerId, out temp) && !temp._spawnedOneTime) // Se primo spawn nel round
        {
            MainScript.Greetings(playerId, temp._steamId, temp._name); // Saluti
            temp._spawnedOneTime = true;
            playerJoinedDictionary[playerId] = temp;
        }
    }
}
