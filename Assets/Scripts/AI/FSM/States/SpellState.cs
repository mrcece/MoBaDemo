﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FSM;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 施法状态,
///     1.当释放的是原地释放型技能时,播放动画(首先重置动画状态),
///       直接释放该技能.
///     2.当释放的是指向型技能时,
///     
/// 
/// </summary>
public class SpellState : FSMState {
    public CharacterMono enemryMono;    
    public Transform enermyTransform;
    public Vector3 enermyPosition;
    public CharacterMono spllerMono;       // 施放技能者
    public CharacterModel speller;

    public override void OnEnter() {
        spllerMono = BlackBorad.CharacterMono;
        if (spllerMono.prepareSkill.IsMustDesignation) {
            enemryMono = BlackBorad.GetCharacterMono("Enemry");
            enermyTransform = BlackBorad.GetTransform("EnemryTransform");
        } else {
            enermyPosition = BlackBorad.GetVector3("EnemryPosition");
        }

        // 重置攻击状态
        spllerMono.ResetAttackStateAnimator();
        Debug.Log("进入spell状态");
    }

    public override void OnExit() {
        spllerMono.isPrepareUseSkill = false;
        Debug.Log("离开spell状态");
    }

    public override void OnUpdate() {
        Debug.Log("SpellUpdate:"+spllerMono.name);

        // reuslt为施法结束状态,为true表示施法结束,其中施法成功、施法失败均表示施法结束，也就是
        bool result = false;
        // 如果施放技能状态结束,就自动回到Idle状态,为黑板设置变量
        if (spllerMono != null) {
            Debug.Log("进入spell状态，准备释放技能："+spllerMono.prepareSkill.SkillName);
            if (spllerMono.prepareSkill.IsMustDesignation && enemryMono != null) {
                result = spllerMono.Spell(enemryMono, enermyTransform.position);
            } else {
                result = spllerMono.Spell(enermyPosition);
            }

            if (result) {
                BlackBorad.SetBool("IsUseSkillFinish", true);
                BlackBorad.SetBool("isPrePareUseSkill", false);
            }

            //// 如果目标单位不能被攻击,回到Idle状态
            //if (enemryMono!=null && !enemryMono.IsCanBeAttack()) {
            //    BlackBorad.SetBool("IsUseSkillFinish", true);
            //    BlackBorad.SetBool("isPrePareUseSkill", false);
            //}
        }
    }
}

