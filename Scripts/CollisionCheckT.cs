using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class CollisionCheckT : MonoBehaviour
    {

        [SerializeField]
        List<string> TouchTags = new List<string>();//現在判定に入っているタグ

        [SerializeField]
        private string TargetTag;//目的のタグ　

        [SerializeField]
        CollisionManager Manager;
        private void Start()
        {
            Manager = transform.parent.parent.gameObject.GetComponent<CollisionManager>();
        }

    public void SetTargetTag(string TargetTag)//ターゲットタグをセットする
        {
            this.TargetTag = TargetTag;
        }

        public void ClearTotchTags()
        {
            TouchTags.Clear();
        }

        public bool CheckTouchTag()
        {
            foreach (var TouchTag in TouchTags)//触っているタグの中を全て網羅してターゲットタグとあっているか確認する
            {
                if (TargetTag == TouchTag)
                {
                    return true;
                }
            }
            return false;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Rarm" || other.gameObject.tag == "Larm" || other.gameObject.tag == "Head")
            {
                if(!TouchTags.Contains(other.gameObject.tag))TouchTags.Add(other.gameObject.tag);//タグを含んでいなかったら
            }
          
        }

        private void OnTriggerExit(Collider other)
        {
            //手足のタグがついていなかったらreturnする
            if (other.gameObject.tag == "Rarm" || other.gameObject.tag == "Larm" || other.gameObject.tag == "Head")
            {
                if (TouchTags.Contains(other.gameObject.tag)) TouchTags.Remove(other.gameObject.tag);//タグを含んでいたら
            }
            
        }
    }
