using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using properties;
using spells;
using System.CodeDom;
using UnityEditor.Experimental.GraphView;
using System.Reflection;

public class InGameManager : MonoBehaviour
{
    WorldEffect WorldEffect;
    SpellData[] SpellDatas;
    [SerializeField] GameObject[] Spells;
    [SerializeField] GameObject Player;
    dynamic Spell;
    SpellSaveManager SpellSaveManager;
    WorldSaveManager WorldSaveManager;

    // Start is called before the first frame update
    void Start()
    {
        SpellSaveManager = new SpellSaveManager(); //SpellSaveManager 생성
        WorldSaveManager = new WorldSaveManager(); //WorldSaveManager 생성

        WorldEffect = WorldSaveManager.Load(); //WorldEffect을 불러온다
        if (WorldEffect == null) WorldEffect = new WorldEffect(); //WorldEffect 데이터가 없으면 새 WorldEffect를 생성

        SpellData newData= new SpellData();
        newData.Properties = new string[] { "bigger" };
        newData.Type = "BulletSpell";
        newData.Name = "skilla";
        newData.Element = "Fire";

        SpellSaveManager.SavePlaySpell(newData, 1);
        SpellSaveManager.SavePlaySpell(newData, 2);
        SpellSaveManager.SavePlaySpell(newData, 3);
        SpellSaveManager.SavePlaySpell(newData, 4);
        SpellSaveManager.SavePlaySpell(newData, 5);

        SpellDatas = SpellSaveManager.LoadPlaySpells(); //PlayerSpell을 배열로 불러온다

        for (int i = 0; i < SpellSaveManager.MAXSKILLSIZE; i++)
        {
            dynamic spell = ConvertSkillData(SpellDatas[i]); //SpellData를 Spell 형식으로 바꿈
            Instantiate(Spells[i], Player.transform.position, Player.transform.rotation);
            Debug.Log(Spells[i]);
            Spells[i].AddComponent<MagicObject>(); //MagicObject를 추가
            Spells[i].AddComponent(spell.GetType()); //Spell을 추가
            Spells[i].GetComponent(spell.GetType()).Init();
        }
    }

    public void SummonSpellByDown(int index)
    {
        //if (Spells[index].GetComponent(ConvertSkillData(SpellDatas[index]).GetType()).type == PressType.Down)
        {
            Instantiate(Spells[index-1], Player.transform.position, Player.transform.rotation);
        }
    }

    public void SummonSpellByStay(int index)
    {
        if (Spells[index].GetComponent(ConvertSkillData(SpellDatas[index]).GetType()).type == PressType.Stay)
        {
            Instantiate(Spells[index]);
        }
    }

    public dynamic ConvertSkillData(SpellData SpellData)
    {
        dynamic spell;
        spell = Type.GetType(SpellData.Type); //SpellData의 Type 이름의 형식으로 설정
        spell.name = SpellData.Name; //Spell의 Name을 설정
        spell.Element = new Element(SpellData.Element); //Spell의 원소 설정
        for (int i = 0; i < SpellSaveManager.MAXSKILLSIZE; i++)
            spell.properties[i] = Type.GetType(SpellData.Properties[i]); //Spell의 속성 설정

        return spell;
    }
}