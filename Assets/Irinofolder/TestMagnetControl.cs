using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//�����̃R�[�h�̑傫�ȕύX��PlayArea�̎擾�ł��B������Area��ɑ��݂���ꍇ�̂݃A�C�R�����Ǐ]����悤�ɂ��Ă���܂��B�@����
public class TestMagnetControl: MonoBehaviour
{
    [SerializeField]
    private TextMeshPro nameLabel = default;
    public GameObject Area;

    //�x���@Awake�ŃG���[���o�Ă�Ɛ����������ɂ��̃X�N���v�g����A�N�e�B�u�ɂȂ��Ă��܂��B�G���[���O�ɂ��łȂ����璍��
    void Start()
    {
 
        var gamePlayerManager = GameObject.FindWithTag("GamePlayerManager");
        Area = GameObject.Find("PlayArea");

    }

    void FixedUpdate()
    {
        if (Area.GetComponent<PlayArea>()._OnPlayArea)
        {
            Vector3 MagnetScreenPosition = Input.mousePosition;//�}�E�X���W���擾

            MagnetScreenPosition.x = Mathf.Clamp(MagnetScreenPosition.x, 0.0f, Screen.width);//Clamp�ŉ�ʊO�ɏo�Ȃ��悤��
            MagnetScreenPosition.y = Mathf.Clamp(MagnetScreenPosition.y, 0.0f, Screen.height);


            MagnetScreenPosition.z = 10.0f;//�摜��\�����邽�߂ɓ��ꂽ���́i���W���J�����Ɠ������Ǝʂ�Ȃ��j

            Camera gameCamera = Camera.main;
            Vector3 MagnetWorldPosition = gameCamera.ScreenToWorldPoint(MagnetScreenPosition);

            transform.position = MagnetWorldPosition;//�ړ�������
        }
    }
    
}
