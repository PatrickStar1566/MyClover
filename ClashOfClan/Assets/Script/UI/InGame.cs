using UnityEngine;
using System.Collections;

public class InGame : MonoBehaviour {
    public UIButton createHero1Btn1;
    public UIButton restartBtn;

    void Start()
    {
        UIEventListener.Get(createHero1Btn1.gameObject).onClick += OnCreateClick1;
        UIEventListener.Get(restartBtn.gameObject).onClick += OnRestartClick;
        UIManager.Instance.InGameUI = this;
    }

    void OnCreateClick1(GameObject obj)
    {
        GameObject startPoint = GameObject.Find("StartPoint");
        GameManager.Instance.SpwanCharacter(CharaData.CharClassType.CHARACTER, CharaData.CharModel.BOWMAN, 0, 1, startPoint.transform.position,
            startPoint.transform.eulerAngles, CharaStatus.Pose.Run);
    }

    void OnRestartClick(GameObject obj)
    {
        GameManager.Instance.DestroyAll();
    }

    void OnDestroy()
    {
        UIEventListener.Get(createHero1Btn1.gameObject).onClick -= OnCreateClick1;
        UIEventListener.Get(restartBtn.gameObject).onClick -= OnRestartClick;
    }
}
