using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#pragma warning disable 0414
/***
****   作为演示的Player类，包含玩家的信息和玩家角色等信息  
***/
[AddComponentMenu("Player/Complexed Player Show"),DisallowMultipleComponent]
public class Player : MonoBehaviour {

    public string playerName = "";
    public int playerAge = 18;
    public string playerTel = "";
    public float playerWeight = 60f;
    public Texture headIcon = null;
    public Color skinColor = Color.white;

    public List<string> playerIDs = new List<string>();
    public List<Skill> playerSkills = new List<Skill>();

    [SerializeField]
    private bool enable = true;
    [SerializeField]
    private int selection = 0;

    int count = 0;
    private void Update()
    {
        
            
            Debug.Log(playerAge);
            
        
    }

}

