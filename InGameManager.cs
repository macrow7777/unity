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
        SpellSaveManager = new SpellSaveManager(); //SpellSaveManager ����
        WorldSaveManager = new WorldSaveManager(); //WorldSaveManager ����

        WorldEffect = WorldSaveManager.Load(); //WorldEffect�� �ҷ��´�
        if (WorldEffect == null) WorldEffect = new WorldEffect(); //WorldEffect �����Ͱ� ������ �� WorldEffect�� ����

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

        SpellDatas = SpellSaveManager.LoadPlaySpells(); //PlayerSpell�� �迭�� �ҷ��´�

        for (int i = 0; i < SpellSaveManager.MAXSKILLSIZE; i++)
        {
            dynamic spell = ConvertSkillData(SpellDatas[i]); //SpellData�� Spell �������� �ٲ�
            Instantiate(Spells[i], Player.transform.position, Player.transform.rotation);
            Debug.Log(Spells[i]);
            Spells[i].AddComponent<MagicObject>(); //MagicObject�� �߰�
            Spells[i].AddComponent(spell.GetType()); //Spell�� �߰�
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
        spell = Type.GetType(SpellData.Type); //SpellData�� Type �̸��� �������� ����
        spell.name = SpellData.Name; //Spell�� Name�� ����
        spell.Element = new Element(SpellData.Element); //Spell�� ���� ����
        for (int i = 0; i < SpellSaveManager.MAXSKILLSIZE; i++)
            spell.properties[i] = Type.GetType(SpellData.Properties[i]); //Spell�� �Ӽ� ����

        return spell;
    }
}