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

namespace Testing.Tas
{
    internal class ToolAssistant
    {
        private Process _process;
        private DarkSouls _darkSouls;

        public ToolAssistant(DarkSouls darkSouls)
        {
            _darkSouls = darkSouls;

            var process = Process.GetProcesses().FirstOrDefault(i => i.ProcessName.ToLower().StartsWith("darksouls"));
            if (process != null)
            {
                _process = process;
            }
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
            User32.SendKeyDown(User32.KeyCode.SPACE_BAR);
        }

        public void Roll()
        {
            Focus();
            User32.SendKeyPress(User32.KeyCode.SPACE_BAR);
        }
        
        private bool _forward = false;
        public void Forward()
        {
            Focus();
            _forward = true;
            User32.SendKeyDown(User32.KeyCode.KEY_W);
        }

        private bool _backward = false;
        public void Backward()
        {
            Focus();
            _backward = true;
            User32.SendKeyDown(User32.KeyCode.KEY_S);
        }

        private bool _left = false;
        public void Left()
        {
            Focus();
            _left = true;
            User32.SendKeyDown(User32.KeyCode.KEY_A);
        }


        private bool _right = false;
        public void Right()
        {
            Focus();
            _right = true;
            User32.SendKeyDown(User32.KeyCode.KEY_D);
        }

        public void Stop()
        {
            if (_forward)
            {
                User32.SendKeyUp(User32.KeyCode.KEY_W);
            }

            if (_forward)
            {
                User32.SendKeyUp(User32.KeyCode.KEY_S);
            }

            if (_left)
            {
                User32.SendKeyUp(User32.KeyCode.KEY_A);
            }

            if (_right)
            {
                User32.SendKeyUp(User32.KeyCode.KEY_D);
            }

            if (_sprint)
            {
                User32.SendKeyUp(User32.KeyCode.SPACE_BAR);
            }
        }

        public void Attack()
        {
            Focus();
            User32.SendKeyPress(User32.KeyCode.KEY_P);
        }
        #endregion

        #region items =================================================================================================================================

        public void UseItem()
        {
            Focus();
            User32.SendKeyPress(User32.KeyCode.KEY_R);
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
            User32.SendKeyPress(User32.KeyCode.ESC);
        }
        
        public void MainMenuContinue()
        {
            Interact();
            while (!_darkSouls.IsPlayerLoaded())
            {
                Thread.Sleep(100);
            }
            Thread.Sleep(3800);
        }


        public void SaveQuit()
        {
            Focus();
            User32.SendKeyPress(User32.KeyCode.ESC);
            Thread.Sleep(200);

            User32.SendKeyPress(User32.KeyCode.HOME);
            Thread.Sleep(50);

            User32.SendKeyPress(User32.KeyCode.KEY_E);
            Thread.Sleep(50);
            
            User32.SendKeyPress(User32.KeyCode.UP);
            Thread.Sleep(50);

            User32.SendKeyPress(User32.KeyCode.KEY_E);
            Thread.Sleep(50);

            User32.SendKeyPress(User32.KeyCode.LEFT);
            Thread.Sleep(50);

            User32.SendKeyPress(User32.KeyCode.KEY_E);

            //Press any key
            Thread.Sleep(1500);
            User32.SendKeyPress(User32.KeyCode.KEY_E);
            
            //Clear offline message
            Thread.Sleep(1500);
            User32.SendKeyPress(User32.KeyCode.KEY_E);
        }

        #endregion


        public void Interact()
        {
            Focus();
            User32.SendKeyPress(User32.KeyCode.KEY_E);
        }
    }
}
