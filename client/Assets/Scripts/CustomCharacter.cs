using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class CustomCharacter : Character
{
    [Header("Character Base")]
    [SerializeField]
    public CharacterBase characterBase;

    [SerializeField]
    public SkillBasic skillBasic;

    [SerializeField]
    public Skill1 skill1;

    [SerializeField]
    public Skill2 skill2;

    [SerializeField]
    public Skill3 skill3;

    [SerializeField]
    public Skill4 skill4;

    protected override void Initialization()
    {
        base.Initialization();
    }
}
