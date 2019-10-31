using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/***
****   作为Player类的内嵌属性展示在Inspector面板上  
***/
[Serializable]
public class Skill
{
    public string name;
    [SerializeField]
    private float cd;
    [SerializeField]
    public float damage;
    [SerializeField]
    public int mp;
    [SerializeField]
    private Texture icon;
    [SerializeField]
    private GameObject effect;
}