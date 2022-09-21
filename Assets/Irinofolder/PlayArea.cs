using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PlayAreaというオブジェクトを作成し(テストシーンではPanelを使用)、オブジェクト上にポインターがあった場合に反応します
//TestMagnetControlの方にboolを受け渡し、trueの時(=エリア内にいる時)にマグネットのアイコンがマウスポインタの位置と合わさります。
//しかし、マッチメイキング上で上手く動作しなかったのでCanvasとの相性があまり良くないのかもしれません。
//解決策を調べ実行していますが現時点で成功していません申し訳ないです。 8/17 入野
public class PlayArea : MonoBehaviour
{
    public bool _OnPlayArea = false;
    private void OnMouseEnter()
    {
        _OnPlayArea = true;
    }

    private void OnMouseExit()
    {
        _OnPlayArea = false;
    }
}
