using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이벤트 종류
public enum EVENT_TYPE
{
    HP_CHANGE,
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
    MOVEMENTS = 1,
    DECREASE,
    INCREASE,
    WEIGHTPUZZLE
}

// 객체가 특정 이벤트를 받을 수 있도록 하는 클래스
public class EventManager : MonoBehaviour
{
    // [싱글톤]이벤트 매니저
    private static EventManager instance = null;
    public static EventManager Instance
    {
        get { return instance; }
        set { }
    }

    // 이벤트에 대한 delegate 타입 정의
    public delegate void OnEvent(TUTORIAL_TYPE eventType, bool flag);

    // 모든 오브젝트는 이벤트를 듣기 위해 listener 객체 사전에 등록
    private Dictionary<TUTORIAL_TYPE, List<OnEvent>> Listeners = new Dictionary<TUTORIAL_TYPE, List<OnEvent>>();

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
        else { DestroyImmediate(this); }
    }

    /////  <summary>  
    ///// 특정 리스너 객체를 리스너 배열에 추가
    ///// </summary> 
    ///// <param name="eEVENT_TYPE">이벤트 종류</param>
    ///// <param name="Listener">이벤트를 듣고자 하는 갹채</param
    //public void AddListener(EVENT_TYPE eventType, OnEvent Listener)
    //{
    //    // 해당 이벤트의 리스너 리스트
    //    List<OnEvent> ListenerList = null;
        
    //    // 리스트가 이미 있는 경우 Listeners는 < 이벤트 종류, 수행할 메소드>를 담고있는 사전
    //    if (Listeners.TryGetValue(eventType, out ListenerList))
    //    {
    //        ListenerList.Add(Listener);
    //        return;
    //    }

    //    // 리스트가 없는 경우 사전의 key로 새 리스트 생성
    //    ListenerList = new List<OnEvent>();
    //    ListenerList.Add(Listener);
    //    Listeners.Add(eventType, ListenerList);    // 리스너 리스트에 추가
    //}

    /// <summary>
    /// 튜토리얼 리스너 객체를 리스너 배열에 추가
    /// </summary>
    /// <param name="tutorialType"></param>
    /// <param name="Listener"></param>
    public void AddListener(TUTORIAL_TYPE tutorialType, OnEvent Listener)
    {
        // 해당 이벤트의 리스너 리스트
        List<OnEvent> ListenerList = null;

        // 리스트가 이미 있는 경우 Listeners는 < 이벤트 종류, 수행할 메소드>를 담고있는 사전
        if (Listeners.TryGetValue(tutorialType, out ListenerList))
        {
            ListenerList.Add(Listener);
            return;
        }

        // 리스트가 없는 경우 사전의 key로 새 리스트 생성
        ListenerList = new List<OnEvent>();
        ListenerList.Add(Listener);
        Listeners.Add(tutorialType, ListenerList);    // 리스너 리스트에 추가
    }

    public void PostNotification(TUTORIAL_TYPE tutorialType, bool flag)
    {
        List<OnEvent> ListenerList = null;

        if (Listeners.TryGetValue(tutorialType, out ListenerList))
            return;

        for(int i=0; i<ListenerList.Count; i++)
        {
            // 객체가 null이 아니면 인터페이스를 통해 메세지 전달
            if (!ListenerList[i].Equals(null))
                ListenerList[i](tutorialType, flag);
        }
    }

    #endregion
}
