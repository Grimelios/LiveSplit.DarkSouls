using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Testing.Tas;
using System.Windows.Forms;
using DarkSoulsMemory;
using WindowsInput;
using WindowsInput.Native;

namespace Testing.Tas
{
    internal class ToolAssistant
    {
        private Process _process;
        private DarkSouls _darkSouls;
        private GameType _gameType;
        private InputSimulator _inputSimulator;

        public ToolAssistant(DarkSouls darkSouls, GameType gameType)
        {
            _darkSouls = darkSouls;

            var process = Process.GetProcesses().FirstOrDefault(i => i.ProcessName.ToLower().StartsWith("darksouls"));
            if (process != null)
            {
                _process = process;
            }

            _inputSimulator = new InputSimulator();

            _gameType = gameType;
        }

        private void Focus()
        {
            User32.SetForegroundWindow(_process.MainWindowHandle);
        }

        #region Movement =================================================================================================================================

        private bool _sprint = false;
        public void Sprint()
        {
            Focus();
            _sprint = true;
            User32.SendKeyDown(User32.KeyCode.SPACE_BAR, User32.ScanCode.Space);
        }

        public void Roll()
        {
            Focus();
            User32.SendKeyPress(User32.KeyCode.SPACE_BAR, User32.ScanCode.Space);
        }
        
        private bool _forward = false;
        public void Forward()
        {

            Focus();
            _forward = true;
            User32.SendKeyDown(User32.KeyCode.KEY_W, User32.ScanCode.W);
        }

        private bool _backward = false;
        public void Backward()
        {
            Focus();
            _backward = true;
            User32.SendKeyDown(User32.KeyCode.KEY_S, User32.ScanCode.S);
        }

        private bool _left = false;
        public void Left()
        {
            Focus();
            _left = true;
            User32.SendKeyDown(User32.KeyCode.KEY_A, User32.ScanCode.A);
        }


        private bool _right = false;
        public void Right()
        {
            Focus();
            _right = true;
            User32.SendKeyDown(User32.KeyCode.KEY_D, User32.ScanCode.D);
        }

        public void Stop()
        {
            if (_forward)
            {
                User32.SendKeyUp(User32.KeyCode.KEY_W, User32.ScanCode.W);
            }

            if (_forward)
            {
                User32.SendKeyUp(User32.KeyCode.KEY_S, User32.ScanCode.S);
            }

            if (_left)
            {
                User32.SendKeyUp(User32.KeyCode.KEY_A, User32.ScanCode.A);
            }

            if (_right)
            {
                User32.SendKeyUp(User32.KeyCode.KEY_D, User32.ScanCode.D);
            }

            if (_sprint)
            {
                User32.SendKeyUp(User32.KeyCode.SPACE_BAR, User32.ScanCode.Space);
            }
        }

        public void Punch()
        {
            Focus();
            Keypress(User32.KeyCode.KEY_P, User32.ScanCode.P);
        }

        public void LightAttack()
        {
            Focus();
            Keypress(User32.KeyCode.KEY_I, User32.ScanCode.I);
        }

        public void Parry()
        {
            Focus();
            Keypress(User32.KeyCode.KEY_O, User32.ScanCode.O);
        }

        public void Menu()
        {
            Focus();
            Keypress(User32.KeyCode.ESC, User32.ScanCode.ESC);
        }
        #endregion

        #region items =================================================================================================================================

        public void UseItem()
        {
            Focus();
            Keypress(User32.KeyCode.KEY_R, User32.ScanCode.R);
        }

        #endregion

        #region Camera =================================================================================================================================
        public void MoveCamera(int x, int y, uint time = 0)
        {
            Focus();
            User32.SendMouseMove(x, y, time);
        }
        #endregion

        #region Menuing =================================================================================================================================

        public void SkipCutscene()
        {
            Focus();
            Keypress(User32.KeyCode.ESC, User32.ScanCode.ESC);
        }
        
        public void MainMenuContinue()
        {
            Focus();
            Keypress(User32.KeyCode.KEY_E, User32.ScanCode.E, 200);

            //Ptde doesn't have "continue" in the main menu. Needs to click load and then click a savefile
            if (_gameType != GameType.DarkSoulsRemastered)
            {
                Thread.Sleep(3000);
                Keypress(User32.KeyCode.KEY_E, User32.ScanCode.E, 200);
            }
            
            while (!_darkSouls.IsPlayerLoaded())
            {
                Thread.Sleep(100);
            }
            Thread.Sleep(3800);
        }


        public void SaveQuit()
        {
            Focus();

            var keyPressDelay = _gameType == GameType.DarkSoulsRemastered ? 32 : 200;
            var sleepTime = _gameType == GameType.DarkSoulsRemastered ? 50 : 100;

            Keypress(User32.KeyCode.ESC, User32.ScanCode.ESC, keyPressDelay);
            Thread.Sleep(200);

            Keypress(User32.KeyCode.PAGEDOWN, User32.ScanCode.PageDown, keyPressDelay);
            Thread.Sleep(sleepTime);

            Keypress(User32.KeyCode.KEY_E, User32.ScanCode.E, keyPressDelay);
            Thread.Sleep(sleepTime);

            Keypress(User32.KeyCode.UP, User32.ScanCode.Up, keyPressDelay);
            Thread.Sleep(sleepTime);

            Keypress(User32.KeyCode.KEY_E, User32.ScanCode.E, keyPressDelay);
            Thread.Sleep(sleepTime);

            Keypress(User32.KeyCode.LEFT, User32.ScanCode.Left, keyPressDelay);
            Thread.Sleep(sleepTime);

            Keypress(User32.KeyCode.KEY_E, User32.ScanCode.E, keyPressDelay);

            //Press any key
            Thread.Sleep(1500);
            Keypress(User32.KeyCode.KEY_E, User32.ScanCode.E, keyPressDelay);

            if (_gameType == GameType.DarkSoulsRemastered)
            {
                //Clear offline message
                Thread.Sleep(1500);
                Keypress(User32.KeyCode.KEY_E, User32.ScanCode.E, keyPressDelay);
            }
        }

        #endregion

        public void EquipArtoriasRing()
        {
            Focus();

            var keyPressDelay = _gameType == GameType.DarkSoulsRemastered ? 32 : 200;
            var sleepTime = _gameType == GameType.DarkSoulsRemastered ? 50 : 100;

            Keypress(User32.KeyCode.ESC, User32.ScanCode.ESC, keyPressDelay);
            Thread.Sleep(200);

            Keypress(User32.KeyCode.RIGHT, User32.ScanCode.Right, keyPressDelay);
            Thread.Sleep(sleepTime);

            Keypress(User32.KeyCode.KEY_E, User32.ScanCode.E, keyPressDelay);
            Thread.Sleep(sleepTime);

            Keypress(User32.KeyCode.LEFT, User32.ScanCode.Left, keyPressDelay);
            Thread.Sleep(sleepTime);
            
            for (int i = 0; i < 2; i++)
            {
                Keypress(User32.KeyCode.DOWN, User32.ScanCode.Down, keyPressDelay);
                Thread.Sleep(sleepTime);
            }

            for (int i = 0; i < 2; i++)
            {
                Keypress(User32.KeyCode.KEY_E, User32.ScanCode.E, keyPressDelay);
                Thread.Sleep(sleepTime);
            }

            Keypress(User32.KeyCode.ESC, User32.ScanCode.ESC, keyPressDelay);
        }
        
        public void Interact()
        {
            Focus();
            Keypress(User32.KeyCode.KEY_E, User32.ScanCode.E);
        }
        
        private void Keypress(User32.KeyCode keyCode, User32.ScanCode scanCode, int keyPressDelay = 32)
        {
            User32.SendKeyDown(keyCode, scanCode);
            Thread.Sleep(keyPressDelay);
            User32.SendKeyUp(keyCode, scanCode);
        }

    }
}
