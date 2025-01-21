using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Image Fuc1left_img;
    public Image Fuc1right_img;
    public Image Fuc2left_img;
    public Image Fuc2right_img;

    public Image Fuc3left_img;
    public Image Fuc3right_img;

    public Image headT_img;
    public Image headB_img;
    // public Button Fuc1_btn;
    // public Button Fuc2_btn;
    // Start is called before the first frame update
    void Start()
    {
        ResetUI();
        Logoset();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetUI()
    {
        Fuc1left_img.transform.DOLocalMoveX(-70 - 492, 0);
        Fuc1right_img.transform.DOLocalMoveX(70 + 492, 0);
        Fuc2left_img.transform.DOLocalMoveX(-70 - 492, 0);
        Fuc2right_img.transform.DOLocalMoveX(70 + 492, 0);
        Fuc3left_img.transform.DOLocalMoveX(-70 - 492, 0);
        Fuc3right_img.transform.DOLocalMoveX(70 + 492, 0);
    }

    public void Fuc1_Click()
    {
        ResetUI();
        Fuc1left_img.transform.DOLocalMoveX(60 - 492, 0.5f);
        Fuc1right_img.transform.DOLocalMoveX(-60 + 492, 0.5f);
    }

    public void Fuc2_Click()
    {
        ResetUI();
        Fuc2left_img.transform.DOLocalMoveX(60 - 492, 0.5f);
        Fuc2right_img.transform.DOLocalMoveX(-60 + 492, 0.5f);
    }

    public void Fuc3_Click()
    {
        ResetUI();
        Fuc3left_img.transform.DOLocalMoveX(60 - 492, 0.5f);
        Fuc3right_img.transform.DOLocalMoveX(-60 + 492, 0.5f);
    }

    public void Logoset()
    {
        headT_img.transform.DOLocalMoveY(35 + 238,0);
        headB_img.transform.DOLocalMoveY(-510 + 238, 0);
        /*Fuc1_btn.transform.DOLocalMoveY(-585,0);
        Fuc2_btn.transform.DOLocalMoveY(-585, 0);*/

        headT_img.transform.DOLocalMoveY(-30 + 238, 1);
        headB_img.transform.DOLocalMoveY(-450 + 238, 1).SetDelay(0.2f);
        /*Fuc1_btn.transform.DOLocalMoveY(-500, 1).SetDelay(0.4f);
        Fuc2_btn.transform.DOLocalMoveY(-500, 1).SetDelay(0.6f);*/
    }

}
