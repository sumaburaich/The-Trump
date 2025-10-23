using CardGame;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameContext
{
    public readonly MonoBehaviour Runner;

    public readonly System.Random random;
    public readonly Hand player_hand = new Hand();
    public readonly DiscardPile discard = new DiscardPile();
    public HandManager handmanager;
    public HandManager en_handmanager;

    //�o�g��������
    public int Player_Wins;
    public int Enemy_Wins;
    public int Win_Target = 3;

    //���݃��E���h�ŃW���[�J�[�����ɂȂ�J�[�hID(���J����)
    public string Boosted_Card_ID;

    //UI���͗p�C�x���g
    public event Action<Phase> OnPhaseChanged;//�t�F�[�Y�\���X�V
    public event Action<List<Card>> OnHandUpdated;//��D�X�V
    public event Action<List<Card>> OnDiscardUpdated;//�X�e�D�X�V
    public event Action<string> OnBoostRevealed;//�u�[�X�g�J�[�h�X�V
    public Action<int> OnBattleScoreChanged;//�X�R�A���o(�v���C���[��)
    public Action<int> OnEnemyBattleScoreChanged;//�X�R�A���o(�G�l�~�[��)

    //�v���C���[�̓��͗v���p
    public Action<Action<List<Card>>> RequestDiscardSelection;//�C�Ӗ����̂Ă�
    public Action<Action<Card>> RequestBattleSelection;//��ɏo���J�[�h

    //�����I�Ȉꎞ�̈�(UI����󂯂Ƃ�l)
    public List<Card> Pending_discards;//UI�őI�΂ꂽ�̂ăJ�[�h
    public Card Pending_battle_card;//UI�őI�΂ꂽ�o���J�[�h

    public GameContext(MonoBehaviour runner, int seed = 0)
    {
        Runner = runner;
        random = seed == 0 ? new System.Random() : new System.Random(seed);
    }

    //�w���p�[
    public void RaisePhase(Phase p) => OnPhaseChanged?.Invoke(p);
    public void RaiseHand() => OnHandUpdated?.Invoke(new List<Card>(player_hand.cards));
    public void RaiseDiscard() => OnDiscardUpdated?.Invoke(new List<Card>(discard.cards));
    public void RaiseBoost(string id) => OnBoostRevealed?.Invoke(id);
}
