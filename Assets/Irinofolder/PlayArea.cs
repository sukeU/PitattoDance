using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PlayArea�Ƃ����I�u�W�F�N�g���쐬��(�e�X�g�V�[���ł�Panel���g�p)�A�I�u�W�F�N�g��Ƀ|�C���^�[���������ꍇ�ɔ������܂�
//TestMagnetControl�̕���bool���󂯓n���Atrue�̎�(=�G���A���ɂ��鎞)�Ƀ}�O�l�b�g�̃A�C�R�����}�E�X�|�C���^�̈ʒu�ƍ��킳��܂��B
//�������A�}�b�`���C�L���O��ŏ�肭���삵�Ȃ������̂�Canvas�Ƃ̑��������܂�ǂ��Ȃ��̂�������܂���B
//������𒲂׎��s���Ă��܂��������_�Ő������Ă��܂���\����Ȃ��ł��B 8/17 ����
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
