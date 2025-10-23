using CardGame;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameFlowRunner : MonoBehaviour
{
    [Header("設定")]
    public int seed = 0;
    public HandManager hm;
    public HandManager en_hm;
    public discard_manager pl_dm;
    public discard_manager en_dm;
    private GameContext gctx;
    private Coroutine loop;
    public Queue<IPhase> phaseQueue;
    public BoostManager bm;
    public BattleManager battleManager;
    static public bool donext_phase;
    static public bool cant_card_drag;

    // デモ用：エディタ再生時に自動開始
    private void Start()
    {
        donext_phase = true;
        gctx = new GameContext(this);
        gctx.handmanager = hm;
        gctx.en_handmanager = en_hm;
        WireDemoCallBacks(gctx);

        phaseQueue = new Queue<IPhase>(new IPhase[]
        {
            new RoundStartPhase(),
            new DrawPhase(5),
            new DiscardPhase(),
            new RedrawPhase(5),
            new RevealBoostCardPhase(),
            new BattlePhase(),
            new GameEndPhase()
        });

    }

    private void Update()
    {
        Debug.Log(phaseQueue.Peek().ToString());
        if (donext_phase)
        {
            if (phaseQueue.Count > 0)
            {
                IPhase nextPhase = phaseQueue.Dequeue();
                StartCoroutine(nextPhase.Execute(gctx));

                if (nextPhase.ToString() == "CardGame.DrawPhase")
                {
                    cant_card_drag = true;
                }

                else if (nextPhase.ToString() == "CardGame.DiscardPhase")
                {
                    pl_dm.start_discard = true;

                    for (int i = 0; i < pl_dm.enable_obj.Count; i++)
                    {
                        pl_dm.enable_obj[i].SetActive(true);
                    }
                    for (int i = 0; i < pl_dm.candidate_state.Count; i++)
                    {
                        pl_dm.candidate_cards.Add(pl_dm.transform.Find("cards").GetChild(i).GetComponent<card_state>());
                    }

                    for (int i = 0; i < en_dm.candidate_state.Count; i++)
                    {
                        en_dm.candidate_cards.Add(en_hm.cs[i]);
                    }
                }

                else if (nextPhase.ToString() == "CardGame.RevealBoostCardPhase")
                {
                    bm.is_boost = true;
                }

                else if (nextPhase.ToString() == "CardGame.BattlePhase")
                {
                    battleManager.enable_obj[3].SetActive(true);
                    cant_card_drag = false;
                }
                donext_phase = false;
            }
            else
            {
                Debug.Log("全フェーズ終了");
            }
        }
    }
    private void WireDemoCallBacks(GameContext ctx)
    {
        //コンソールにログだし
        ctx.OnPhaseChanged += p => Debug.Log($"[PHASE]{p}");
        ctx.OnBoostRevealed += id => Debug.Log($"Boost Card Revealed: {id}");
        ctx.OnHandUpdated += hand => Debug.Log($"Hand: {string.Join(",", hand.ConvertAll(c => c.ID))}");
        ctx.OnDiscardUpdated += disc => Debug.Log($"Discard: {disc.Count}枚");
        ctx.OnBattleScoreChanged += s => Debug.Log($"Player Score: {s}");
        ctx.OnEnemyBattleScoreChanged += s => Debug.Log($"Enemy Score: {s}");

        // 入力要求：実際はUIで選ばせて、引数のコールバックに結果を返す
        ctx.RequestDiscardSelection = (done) =>
        {
            // デモ：常に0枚捨てる
            done?.Invoke(new List<Card>());
        };

        ctx.RequestBattleSelection = (done) =>
        {
            // デモ：手札の先頭
            var choice = ctx.player_hand.count > 0 ? ctx.player_hand.cards[0] : null;
            done?.Invoke(choice);
        };
    }
}
