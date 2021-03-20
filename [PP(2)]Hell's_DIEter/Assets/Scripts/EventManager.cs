using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이벤트 종류
public enum EVENT_TYPE
{
    STORY,
    TUTORIAL,
    ESCAPE,
    DEAD
}

// 아바타 종류
public enum AVATAR_TYPE
{
    HEALTHY,
    WEAK,
    SICK
}

// 튜토리얼 종류
public enum TUTORIAL_TYPE
{
    MOVE = 1,
    JUMP,
    JETPACK,
    WEIGHT
}
// 객체가 특정 이벤트를 받을 수 있도록 하는 클래스
public class EventManager : MonoBehaviour
{
    #region C# Properties
    // [싱글톤]이벤트 매니저
    public static EventManager Instance
    {
        get { return instance; }
        set { }
    }
    #endregion

    #region Variables 
    // [싱글톤]NotificationManager 인스턴스에 대한 내부 참조
    private static EventManager instance = null;

    // 이벤트에 대한 delegate 타입 정의
    public delegate void OnEvent(EVENT_TYPE eventType, Component Sender, object Param = null);

    // 모든 오브젝트는 이벤트를 듣기 위해 listener 객체 사전에 등록
    private Dictionary<EVENT_TYPE, List<OnEvent>> Listeners = new Dictionary<EVENT_TYPE, List<OnEvent>>();
    #endregion

    #region Methods
    // 초기화를 위해 시작하자마자 호출
    private void Awake()
    {
        // 인스턴스가 없으면 새로 할당
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // 씬 종료 시에 객체가 파괴되는 것을 방지
        }
        // 인스턴스가 이미 존재하면 싱글톤이기떄문에 다른 객체를 삭제함
        else
        {
            DestroyImmediate(this);
        }
    }

    ///  <summary>  
    /// 특정 리스너 객체를 리스너 배열에 추가
    /// </summary> 
    /// <param name="eEVENT_TYPE">이벤트 종류</param>
    /// <param name="Listener">이벤트를 듣고자 하는 갹채</param
    public void AddListener(EVENT_TYPE eEVENT_TYPE, OnEvent Listener)
    {
        // 해당 이벤트의 리스너 리스트
        List<OnEvent> ListenerList = null;
        
        // Listeners는 < 이벤트 종류, 수행할 메소드>를 담고있는 사전
        if (Listeners.TryGetValue(eEVENT_TYPE, out ListenerList))
        {
            ListenerList.Add(Listener);
            return;
        }

        // 사전의 key로 새 리스트 생성
        ListenerList = new List<OnEvent>();
        ListenerList.Add(Listener);
        Listeners.Add(eEVENT_TYPE, ListenerList);    // 리스너 리스트에 추가
    }
    #endregion

    // 튜토리얼 이벤트 대리자
    public delegate void TutorialEvent( TUTORIAL_TYPE num );
    //event TutorialEvent CallTutorialEvent;
}
