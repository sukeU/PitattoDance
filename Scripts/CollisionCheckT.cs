using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class CollisionCheckT : MonoBehaviour
    {

        [SerializeField]
        List<string> TouchTags = new List<string>();//���ݔ���ɓ����Ă���^�O

        [SerializeField]
        private string TargetTag;//�ړI�̃^�O�@

        [SerializeField]
        CollisionManager Manager;
        private void Start()
        {
            Manager = transform.parent.parent.gameObject.GetComponent<CollisionManager>();
        }

    public void SetTargetTag(string TargetTag)//�^�[�Q�b�g�^�O���Z�b�g����
        {
            this.TargetTag = TargetTag;
        }

        public void ClearTotchTags()
        {
            TouchTags.Clear();
        }

        public bool CheckTouchTag()
        {
            foreach (var TouchTag in TouchTags)//�G���Ă���^�O�̒���S�Ėԗ����ă^�[�Q�b�g�^�O�Ƃ����Ă��邩�m�F����
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
                if(!TouchTags.Contains(other.gameObject.tag))TouchTags.Add(other.gameObject.tag);//�^�O���܂�ł��Ȃ�������
            }
          
        }

        private void OnTriggerExit(Collider other)
        {
            //�葫�̃^�O�����Ă��Ȃ�������return����
            if (other.gameObject.tag == "Rarm" || other.gameObject.tag == "Larm" || other.gameObject.tag == "Head")
            {
                if (TouchTags.Contains(other.gameObject.tag)) TouchTags.Remove(other.gameObject.tag);//�^�O���܂�ł�����
            }
            
        }
    }
