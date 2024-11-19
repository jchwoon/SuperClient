# 📑 목차

- [프로젝트 개요](#-프로젝트-개요)
- [트러블 슈팅](#-트러블-슈팅)

# 👋 프로젝트 개요
* 📌 **장르 : 3D MMORPG**
* 📅 **2024. 09 ~ 진행중**
* 🔧 **기술 스택**
  * C#, .Net, Unity, EntityFrameworkCore, SQLServer
* ### [SuperWorld 시연영상](https://www.youtube.com/watch?v=Q42nrx8J2Wo)

# 👋 트러블 슈팅
1. 유니티의 Main Thread가 아닌 다른 Thread가 유니티 API에 접근하지 못하는 문제
* 들어온 패킷을 유니티의 MainThread가 가로채야겠다고 판단
* PacketQueue를 만들어 PacketQueue에 있는 Queue에 패킷을 등록
* NetworkManager의 Update에서 Queue에 패킷이 있으면 처리하여 해결
<br></br>
2. Update마다 이동 패킷을 날리면 부하가 심할 거로 생각하여 일정 주기마다 패킷을 보
내야겠다고 판단
* 그러나 해당 주기 사이에 동기화가 안 되는 문제
* 다음 주기까지의 위치를 얘측해야겠다고 판단
* 위치와 rotY값을 받고 rotY와 단위원을 활용해 방향을 구함
* 오차가 클 경우를 대비해 Threshold 변수를 두어 범위를 벗어나면 실제 위치를
보내어 해결
<br></br>
3. 유니티 2022 이하 버전의 Android 환경에서 Terrain이 금속 재질처럼 빛나는
효과가 발생하는 문제
* 원인: URP의 Terrain Lit 메터리얼의 Shader코드가 ASTC 압축 방식에서
Smoothness값이 Texture의 alpha 값으로 되는 문제가 있음
* 1 .압축 방식 ETC2로 변경, 2. 유니티 버전 업, 3. Shader 코드 수정,
    4. Terrain에 사용되는 Texture의 Format변경, 5. Alpha Source를 From Gray
Scale로 변경
총 5가지의 방법이 있었는데
* 1번은 Terrain하나 때문에 전체 압축 방식을 바꾸는 건 아니라고 생각함
* 2번도 1번과 비슷한 이유
* 3번이 제일 좋은 방법이라 생각했으나 Shader를 아직 다루지 못함
* 4번은 압축이 안 되어 Mobile 환경에서 메모리 문제가 발생
* 위의 이유로 5번 방식을 채택
