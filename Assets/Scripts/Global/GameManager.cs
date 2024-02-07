using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public ItemDB itemDB;
    public GameObject player;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        itemDB.InitDict();
    }
}
