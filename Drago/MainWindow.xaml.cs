using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;

namespace Drago
{
    public partial class MainWindow : Window
    {
        private const int HOTKEY_ID = 7777;
        private const uint WM_HOTKEY = 0x0312;

        // Windows API 선언
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        // 가상 키코드 상수
        private const byte VK_CONTROL = 0x11;
        private const byte VK_C = 0x43;
        private const byte VK_V = 0x56;
        private const uint KEYEVENTF_KEYUP = 0x0002;

        public MainWindow()
        {
            InitializeComponent();
        }

        // 창 핸들이 생성될 때 단축키와 메시지 훅을 최초 등록
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            IntPtr handle = new WindowInteropHelper(this).Handle;
            HwndSource source = HwndSource.FromHwnd(handle);
            source.AddHook(HwndHook);

            UpdateHotkey();
        }

        // 사용자가 UI에서 단축키를 변경할 때 동작
        private void ComboHotkey_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsLoaded)
            {
                UpdateHotkey();
            }
        }

        // 단축키 업데이트 및 재등록
        private void UpdateHotkey()
        {
            IntPtr handle = new WindowInteropHelper(this).Handle;
            UnregisterHotKey(handle, HOTKEY_ID);

            uint targetKey = 0x78; // 기본값 F9 (0x78)
            if (ComboHotkey.SelectedIndex == 1) targetKey = 0x79; // F10
            if (ComboHotkey.SelectedIndex == 2) targetKey = 0x7A; // F11
            if (ComboHotkey.SelectedIndex == 3) targetKey = 0x91; // ScrollLock

            RegisterHotKey(handle, HOTKEY_ID, 0x0000, targetKey);
        }

        // 백그라운드 단축키 입력 감지 훅
        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_HOTKEY && wParam.ToInt32() == HOTKEY_ID)
            {
                ExecuteDragoReplaceTranslation();
                handled = true;
            }
            return IntPtr.Zero;
        }

        // 🐉 Drago 핵심 원문 자동 치환 알고리즘
        private async void ExecuteDragoReplaceTranslation()
        {
            try
            {
                // 1. 기존 클립보드 데이터 백업
                string originalClipboard = Clipboard.ContainsText() ? Clipboard.GetText() : string.Empty;

                // 2. 강제 Ctrl + C 입력으로 선택된 텍스트 가로채기
                keybd_event(VK_CONTROL, 0, 0, 0);
                keybd_event(VK_C, 0, 0, 0);
                await Task.Delay(50);
                keybd_event(VK_C, 0, KEYEVENTF_KEYUP, 0);
                keybd_event(VK_CONTROL, 0, KEYEVENTF_KEYUP, 0);
                await Task.Delay(50);

                string originalText = Clipboard.GetText();

                if (!string.IsNullOrWhiteSpace(originalText))
                {
                    // 3. UI 설정 언어 가져오기
                    string targetLanguage = "KO";
                    if (ComboLanguage.SelectedIndex == 1) targetLanguage = "EN";
                    if (ComboLanguage.SelectedIndex == 2) targetLanguage = "JA";
                    if (ComboLanguage.SelectedIndex == 3) targetLanguage = "ES";

                    // 4. 구글 실시간 무료 API 번역 작동
                    string translatedText = await TranslateEngineAsync(originalText.Trim(), targetLanguage);

                    // 5. 번역된 텍스트를 클립보드에 일시 저장
                    Clipboard.SetText(translatedText);

                    // 6. 강제 Ctrl + V 입력으로 기존 드래그 영역에 그대로 덮어쓰기
                    keybd_event(VK_CONTROL, 0, 0, 0);
                    keybd_event(VK_V, 0, 0, 0);
                    await Task.Delay(50);
                    keybd_event(VK_V, 0, KEYEVENTF_KEYUP, 0);
                    keybd_event(VK_CONTROL, 0, KEYEVENTF_KEYUP, 0);
                    await Task.Delay(50);
                }

                // 7. 작업 종료 후 사용자의 원래 클립보드 데이터 복구
                if (!string.IsNullOrEmpty(originalClipboard))
                {
                    Clipboard.SetText(originalClipboard);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Drago 실행 오류: {ex.Message}");
            }
        }

        // 구글 번역 API 파싱 연동부
        private async Task<string> TranslateEngineAsync(string text, string lang)
        {
            try
            {
                string targetLang = lang.ToLower();
                string url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl=auto&tl={targetLang}&dt=t&q={Uri.EscapeDataString(text)}";

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
                    string response = await client.GetStringAsync(url);

                    using (JsonDocument doc = JsonDocument.Parse(response))
                    {
                        var root = doc.RootElement;
                        if (root.ValueKind == JsonValueKind.Array && root.GetArrayLength() > 0)
                        {
                            var firstArray = root[0];
                            if (firstArray.ValueKind == JsonValueKind.Array)
                            {
                                string result = "";
                                foreach (var item in firstArray.EnumerateArray())
                                {
                                    if (item.ValueKind == JsonValueKind.Array && item.GetArrayLength() > 0)
                                    {
                                        result += item[0].GetString();
                                    }
                                }
                                return !string.IsNullOrEmpty(result) ? result : text;
                            }
                        }
                    }
                }
            }
            catch
            {
                // 예외 발생 시 원문 보호를 위해 원본 반환
            }
            return text;
        }

        protected override void OnClosed(EventArgs e)
        {
            IntPtr handle = new WindowInteropHelper(this).Handle;
            UnregisterHotKey(handle, HOTKEY_ID);
            base.OnClosed(e);
        }
    }
}