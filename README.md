# 🐉 Drago

<p align="center">
  <img src="Drago/Dragoico.ico" alt="Drago Icon" width="128" height="128" />
</p>

<h3 align="center">Drago: In-place Text Translator</h3>

<p align="center">
  <img src="https://img.shields.io/badge/Platform-Windows-0078D4?style=flat-square&logo=windows&logoColor=white" alt="Platform" />
  <img src="https://img.shields.io/badge/Language-C%23-239120?style=flat-square&logo=c-sharp&logoColor=white" alt="Language" />
  <img src="https://img.shields.io/badge/Framework-.NET%208.0%20%2F%20WPF-512BD4?style=flat-square&logo=dotnet&logoColor=white" alt="Framework" />
  <img src="https://img.shields.io/badge/License-Custom-red?style=flat-square" alt="License" />
</p>

<p align="center">
  <b>Drago</b>는 작업 흐름을 깨지 않는 스마트 인플레이스(In-place) 번역기입니다.<br />
  원하는 문장을 드래그하고 단축키를 누르면, 번역 창을 따로 띄울 필요 없이 <b>그 자리에서 즉시</b> 번역된 텍스트로 치환됩니다.
</p>

---

## ⚡ Key Features

* **In-place Replacement** : 별도의 팝업창이나 레이오버 없이, 선택한 원문 자체가 지정한 언어로 마법처럼 교체됩니다.
* **Global Hotkey Framework** : 윈도우 OS 수준에서 작동하는 글로벌 훅을 통해 `F9`, `F10`, `F11`, `ScrollLock` 중 원하는 키로 실시간 매핑이 가능합니다.
* **Clipboard Protection** : 가상 키보드 이벤트를 전송하기 전 사용자의 기존 클립보드 데이터를 안전하게 백업하고, 작업 완료 후 즉시 복원하여 데이터 유실을 방지합니다.
* **No API Key Required** : 복잡한 클라우드 결제나 API 발급 과정 없이, 실행 즉시 무료로 실시간 다국어 번역을 이용할 수 있습니다.

---

## 🚀 Workflow
1. 웹브라우저, IDE, 메모장 등 어디서나 번역할 텍스트를 마우스로 **블록 지정**합니다.
2. 지정한 **글로벌 단축키**를 누릅니다.
3. 드래그된 텍스트 영역이 선택한 목표 언어(KO, EN, JA, ES)로 **즉시 치환**됩니다.

---

## 🛠️ Tech Stack & Architecture

* **UI Framework**: Windows Presentation Foundation (WPF)
* **Runtime**: .NET 8.0 SDK (Windows Desktop Core)
* **Interoperability**: Windows Native API (`user32.dll` - RegisterHotKey, keybd_event)
* **Translation Engine**: Google Translation Architecture (GTX REST Interface)

---

## 📦 Requirements

* Windows 10 / 11 (64-bit)
* [.NET Desktop Runtime 8.0](https://dotnet.microsoft.com/download/dotnet/8.0) 이상

---

## ⚖️ License

Copyright © 2026 **micio**. All rights reserved.  
본 프로젝트는 개인적/상업적 이용이 가능하나, 소스 코드 및 바이너리의 무단 복제, 수정 후 재배포 및 재판매는 엄격히 금지됩니다. 자세한 내용은 `LICENSE` 파일을 참조하세요.
