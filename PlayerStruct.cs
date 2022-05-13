using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoldfastSharedMethods;

public struct joinStruct
{
    public ulong _steamId;
    public string _name;
    public string _regimentTag;
    public bool _spawnedOneTime;
}
public struct playerStruct // Not used
{
    public string _name;
    public ulong _steamId;
    public FactionCountry _playerFaction;
    public PlayerClass _playerClass;
    public int _uniformId;
    public string _regimentTag;

}