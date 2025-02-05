# 🎯 Object Switch Tool - Unity
![GitHub release](https://img.shields.io/github/v/release/ArgonautJH/ObjectSwitchTool)
![GitHub stars](https://img.shields.io/github/stars/ArgonautJH/ObjectSwitchTool)
![GitHub forks](https://img.shields.io/github/forks/ArgonautJH/ObjectSwitchTool)

Unity에서 오브젝트의 위치를 저장하고 쉽게 전환할 수 있는 도구입니다.

![프로젝트 대표 이미지](https://github.com/user-attachments/assets/f5605d5b-fda4-4a7b-8c8c-377f7aefb615)



# 🛠 설치 및 사용 방법

## 📦 1️⃣ Package Manager로 설치 (추천)
이 방법은 최신 버전을 쉽게 유지할 수 있습니다.

1. Unity에서 **Window > Package Manager**를 엽니다.
2. 왼쪽 상단의 `+` 버튼을 클릭하고 **"Add package from git URL..."** 선택합니다.
3. 아래 URL을 입력한 후 **`Add`** 버튼을 클릭하세요.
```
https://github.com/ArgonautJH/ObjectSwitchTool.git
```
4. 설치가 완료되면 **Tools > Object Switch Tool**을 실행하여 사용할 수 있습니다.

> 📌 **특정 버전 설치하기**  
> 최신 버전이 아닌 특정 버전을 사용하려면 Git 태그를 추가하세요.
> ```
> https://github.com/ArgonautJH/ObjectSwitchTool.git#v1.0.0
> ```
> 위처럼 `#버전태그`를 추가하면 해당 버전이 설치됩니다.

---
## 📂 2️⃣ `.unitypackage`로 설치
이 방법은 Package Manager 없이 간단히 사용할 수 있습니다.

1. **[GitHub Releases](https://github.com/ArgonautJH/ObjectSwitchTool/releases)** 페이지에서 `.unitypackage` 파일을 다운로드하세요.
2. Unity에서 **Assets > Import Package > Custom Package...**를 선택합니다.
3. 다운로드한 `.unitypackage` 파일을 선택하고 `Import` 버튼을 클릭합니다.

---

## 🎮 3️⃣ 사용 방법
- Unity 메뉴에서 **`Tools > Object Switch Tool`**을 실행합니다.

---

## ❓ FAQ
### Q1. 패키지를 업데이트하려면 어떻게 하나요?
- **Package Manager 사용 시**: `Window > Package Manager`에서 패키지를 선택하고 **Update** 버튼을 클릭하세요.
- **GitHub에서 직접 가져온 경우**: `manifest.json`을 열어 최신 버전으로 변경하세요.
```json
{
 "dependencies": {
   "com.argonautjh.objectswitchtool": "https://github.com/ArgonautJH/ObjectSwitchTool.git#v1.1.0"
 }
}
```
### Q2. GitHub 저장소에서 직접 클론하여 설치할 수도 있나요?
- 아래 명령어를 사용하면 `Packages/` 폴더에서 직접 클론해주세요.
```
cd Packages
git clone https://github.com/ArgonautJH/ObjectSwitchTool.git
```

💡 추가 정보
문제 해결: Issues에서 문제를 신고해주세요.
기여하기: Pull Request를 환영합니다! 😊

## 📝 추가 설명
더 자세한 설명은 블로그에서 확인할 수 있습니다!  
👉 [[Unity] EditorWindow로 오브젝트 이동 툴 만들기](https://velog.io/@argonaut/Unity-EditorWindow%EB%A1%9C-%EC%98%A4%EB%B8%8C%EC%A0%9D%ED%8A%B8-%EC%9D%B4%EB%8F%99-%ED%88%B4-%EB%A7%8C%EB%93%A4%EA%B8%B0)
