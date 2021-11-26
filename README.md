# <img src="https://user-images.githubusercontent.com/37385962/143446027-a7c0d743-a02f-42be-ad2a-c608f7045076.png" width="32" height="32"/> 헬스 다이어터(Hell's DIEter)
<img src="https://user-images.githubusercontent.com/37385962/143446270-dc6978cb-f321-4992-b462-0e726b4df734.png" width="854" height="480"/>
          
## 홍익대학교 게임제작프로젝트(2)_졸업작품: 헬스 다이어터(Hell's DIEter)
### 게임 실행 파일 배포 Build File 
##### https://drive.google.com/file/d/19QHdJjUbcHs1Pz4jYNfbloC-N_rFA7EV/view?usp=sharing
------------

### 0. 제작
* 기획 / 개발 / 3D 모델링(일부) : 홍익대학교 게임학부 게임소프트웨어전공(공학계) B893248 정해빈
> #### - email : bean9194@naver.com
> #### - portfolio : https://drive.google.com/file/d/1dc3YYt8FcClYOhHHp1U3nUgkhmxw0vRZ/view
* UI / 2D아트 : 홍익대학교 게임학부 게임그래픽디자인전공(미술계) B878037 이동건
> #### - email :  dk000484@naver.com
> #### - instagram : @dk000484
------------

### 1. 개요 IntroDuction
#### (1) 소개
###### 먹보 우주인이 지옥에서 아이템과 체중 증감을 이용하여 퍼즐을 풀고 잠긴 문을 열며 이승으로 가는 출구를 찾는 게임
#### (2) 장르
###### Adventure, Puzzle
#### (3) 플랫폼
###### PC, (Mobile - Android / IOS 준비중)
#### (4) 타겟 플레이어
###### 퍼즐게임을 좋아하는 여성 라이트 게이머

------------

### 2. 기획 및 기술적 특징 Specifics
#### (1) 기획적 특징
##### * 타겟 플레이어
> ##### - 여성 게이머 : 전반적으로 사용 모델들은 Low-Polygon이고 아기자기한 느낌이 나는 것으로 사용
> ##### - 라이트 게이머 : 플레이타임이 짧은 편에 속하는 게임으로 기획
##### * 시험의 방(스테이지 종류)
> ##### - 패널 퍼즐 : 간단한 규칙을 가졌지만 해결하기 위해서는 고민이 필요한 패널 뒤집기 퍼즐 스테이지
> ##### - 무게 저울 : 제한시간 내에 맵을 돌아다니는 몬스터를 잡아 저울 위에 올리는 액션 스테이지
#### (2) 기술적 특징
##### * 사용 알고리즘
> ##### - 길찾기(using A*) : 몬스터가 랜덤으로 순찰해야 하는 포지션을 배정받으면 현위에서 해당위치까지 장애물을 피해 이동하는 알고리즘
> ##### - 비동기 씬 전환 : 일정 시간동안 혹은 다음 씬의 오브젝트가 모두 생성이 될 때까지 플레이 TIP을 보여준 후 다음 씬으로 전환하는 시스템
> ##### - Quest System(using delegate) : Observer가 이벤트를 감지하면 다음 퀘스트를 제안해주는 시스템
> ##### - Improved TPS Camera : 카메라와 플레이어 사이에 장애물이 감지되면 해당 장애물 앞으로 카메라가 이동되어 플레이어 시야가 가려지는 불편함을 해소한 카메라

------------

### 3. 관련 문서 및 영상 Docs & Videos
#### (1) 기획서
###### [졸업작품 게임기획서-B893248 정해빈.pdf](https://github.com/Haebny/Hongik-PP2-HellsDIEter/files/7371135/-B893248.pdf)
#### (2) 제작 보고서
###### [졸업작품 제작보고서-B893248 정해빈.pdf](https://github.com/Haebny/Hongik-PP2-HellsDIEter/files/7371141/-B893248.pdf)
#### (3) Link
###### YouTube Link : https://youtu.be/FdspcRUEO_g

------------

### 4.CCL
(1) Sounds  
> [Story]
> * Song : SY9162 - novel
> * Follow Artist : https://bit.ly/3eIA3CP
> * Music promoted by DayDreamSound : https://youtu.be/Q0I-XFCycZ0

> [Tutorial]
> * Song : Purple Planet Music - Nice Surprise
> * Music: https://www.purple-planet.com
> * Music promoted by DayDreamSound : https://youtu.be/UyqVmG-NmW4

> [Main]
> * Song : 50meru - Deneb
> * Follow Artist : http://bit.ly/2ZiGvdH
> * Music promoted by DayDreamSound : https://youtu.be/nLzzDqMFnhE

> [ETC]
> * Effect Sounds : https://freesound.org/

(2) Fonts
> * © NEXON MAPLESTORY FONT (BOLD & LIGHT) : http://levelup.nexon.com/font/index.aspx?page=5 
