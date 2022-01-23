using DarkSoulsMemory;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Testing.Tas;

namespace Testing
{
    [TestFixture]
    internal class DarkSoulsRemastered
    {
        public enum GameType
        {
            DarkSoulsRemastered,
            DarkSoulsPtde,
            DarkSoulsCracked,
        }

        private static ToolAssistant _assistant;
        private static DarkSouls _darkSouls;


        public static string SaveFileName = "DRAKS0005.sl2";
        public static string SaveFileLocation;
        public static string ReplacementSave;


        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var game = GameType.DarkSoulsRemastered;

            switch (game)
            {
                case GameType.DarkSoulsRemastered:
                    SaveFileLocation = @"C:\Users\Frank\Documents\NBGI\DARK SOULS REMASTERED\72957510";
                    ReplacementSave = "remastered";
                    break;

                case GameType.DarkSoulsPtde:
                    SaveFileLocation = @"C:\Users\Frank\Documents\NBGI\DarkSouls\GroupMink920669";
                    ReplacementSave = "ptde";
                    break;

                case GameType.DarkSoulsCracked:
                    SaveFileLocation = @"C:\Users\Frank\Documents\NBGI\DarkSouls";
                    ReplacementSave = "cracked";
                    break;
            }


            //Rename current save
            if (File.Exists(SaveFileLocation + "\\" + SaveFileName))
            {
                if (File.Exists(SaveFileLocation + "\\automated_backup"))
                {
                    File.Delete(SaveFileLocation + "\\automated_backup");
                }
                File.Move(SaveFileLocation + "\\" + SaveFileName, SaveFileLocation + "\\automated_backup");
            }
            //Replace with test save
            File.Move(Environment.CurrentDirectory + $@"\saves\{ReplacementSave}\UndeadAsylum", SaveFileLocation + "\\" + SaveFileName);

            _darkSouls = new DarkSouls();
            _darkSouls.SetCheat(CheatType.PlayerExterminate     , true);
            _darkSouls.SetCheat(CheatType.PlayerNoDead          , true);
            _darkSouls.SetCheat(CheatType.AllNoStaminaConsume   , true);
            _darkSouls.SetCheat(CheatType.AllNoUpdateAI         , true);
            
            _assistant = new ToolAssistant(_darkSouls);
            _assistant.MainMenuContinue();
            Thread.Sleep(1000);


            //Make sure the game is running
            if (!_darkSouls.IsGameAttached)
            {
                throw new Exception();
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _assistant.SaveQuit();
        
            //Bring the game back into a non-cheated state
            _darkSouls.SetCheat(CheatType.PlayerExterminate     , false);
            _darkSouls.SetCheat(CheatType.PlayerNoDead          , false);
            _darkSouls.SetCheat(CheatType.AllNoStaminaConsume   , false);
            _darkSouls.SetCheat(CheatType.AllNoUpdateAI         , false);
        
            //restore savefile
            File.Delete(SaveFileLocation + "\\" + SaveFileName);
            File.Move(SaveFileLocation + "\\automated_backup", SaveFileLocation + "\\" + SaveFileName);
        }

        #region Boss kills

        public delegate void KillBossMethod();

        public static List<(BossType, KillBossMethod)> BossKills = new List<(BossType, KillBossMethod)>()
        {
            (BossType.AsylumDemon, KillAsylumDemon),
            (BossType.TaurusDemon, KillTaurusDemon),
            (BossType.Gargoyles, KillGargoyles),
            (BossType.CapraDemon, KillCapraDemon),
            (BossType.GapingDragon, KillGapingDragon),
            (BossType.Quelaag, KillQuelaag),
            (BossType.CeaselessDischarge, KillCeaselessDischarge),
            (BossType.IronGolem, KillIronGolem),
            (BossType.OrnsteinAndSmough, KillOrnsteinAndSmough),
            //gwyndolin
            //priscilla
        };

        [TestCaseSource(nameof(BossKills))]
        public void BossKill((BossType, KillBossMethod) param)
        {
            var bossType = param.Item1;
            var func = param.Item2;

            var stateBefore = _darkSouls.IsBossDefeated(bossType);
            func();
            var stateAfter = _darkSouls.IsBossDefeated(bossType);

            Assert.AreEqual(false, stateBefore);
            Assert.AreEqual(true, stateAfter);
        }



        private static void KillAsylumDemon()
        {
            _darkSouls.BonfireWarp(WarpType.NorthernUndeadAsylumBonfire1);
            Thread.Sleep(3000);

            _darkSouls.Teleport(new Vector3f(3.0f, 198.0f, -25.0f), 180);
            Thread.Sleep(3000);

            _assistant.Attack();
            Thread.Sleep(5500);
        }

        private static void KillGargoyles()
        {
            _darkSouls.BonfireWarp(WarpType.UndeadParishNearCell);
            Thread.Sleep(3000);

            //The bossfight won't trigger unless we pass through the foggate
            _darkSouls.Teleport(new Vector3f(17.0f, 48.0f, 74.0f), 0);
            Thread.Sleep(3000);
            _assistant.Interact();
            Thread.Sleep(3300);


            _darkSouls.Teleport(new Vector3f(15.0f, 48.0f, 90.0f), 0);
            Thread.Sleep(1000);
            _assistant.SkipCutscene();
            Thread.Sleep(100);

            _darkSouls.Teleport(new Vector3f(11.0f, 49.0f, 103.0f), 0);
            Thread.Sleep(500);
            _assistant.Attack();
            Thread.Sleep(1000);

            _darkSouls.Teleport(new Vector3f(11.0f, 49.0f, 122.0f), 0);
            Thread.Sleep(4500);
            _assistant.Attack();

            Thread.Sleep(5500);
        }


        private static void KillTaurusDemon()
        {
            _darkSouls.BonfireWarp(WarpType.UndeadBurgBonfire);
            Thread.Sleep(3000);

            //Trigger Taurus to jump down

            _darkSouls.Teleport(new Vector3f(23.0f, 16.0f, -120.0f), 270);
            Thread.Sleep(1000);

            _darkSouls.Teleport(new Vector3f(3.0f, 16.0f, -115.0f), 270);
            Thread.Sleep(2000);

            _assistant.Attack();
            Thread.Sleep(5500);
        }

        private static void KillCapraDemon()
        {

        }

        private static void KillGapingDragon()
        {
            _darkSouls.BonfireWarp(WarpType.DepthsGapingDragonsRoom);
            Thread.Sleep(3000);

            //Trigger cutscene & boss spawn
            _darkSouls.Teleport(new Vector3f(-167.0f, -100.0f, -10.0f), 0);
            Thread.Sleep(1000);
            _assistant.SkipCutscene();
            Thread.Sleep(100);

            //Teleport to boss & kill
            _darkSouls.Teleport(new Vector3f(-170.0f, -100.0f, 25.0f), 0);
            Thread.Sleep(2000);
            _assistant.Attack();
            Thread.Sleep(7000);
        }

        private static void KillQuelaag()
        {
            _darkSouls.BonfireWarp(WarpType.BlighttownSwampBonfire);
            Thread.Sleep(3000);
            
            _darkSouls.Teleport(new Vector3f(16.0f, -237.0f, 113.0f), 0);
            Thread.Sleep(2000);
            _assistant.SkipCutscene();
            Thread.Sleep(100);

            _darkSouls.Teleport(new Vector3f(74.5f, -238.7f, 125.0f), 90);
            Thread.Sleep(2000);
            _assistant.Attack();
            Thread.Sleep(6000);
        }

        private static void KillCeaselessDischarge()
        {

        }

        private static void KillIronGolem()
        {

        }

        private static void KillOrnsteinAndSmough()
        {
            _darkSouls.BonfireWarp(WarpType.AnorLondoGwynevereBonfire);
            Thread.Sleep(3000);

            //Trigger fight
            _darkSouls.Teleport(new Vector3f(538.0f, 143.0f, 255.0f), 0);
            Thread.Sleep(1000);
            _assistant.SkipCutscene();
            Thread.Sleep(100);

            //kill o, skip wait and skip cutscene
            _darkSouls.Teleport(new Vector3f(577.0f, 143.0f, 253.0f), 90);
            Thread.Sleep(1500);
            _assistant.Attack();
            Thread.Sleep(4500);
            _assistant.SkipCutscene();

            //kill s, skip wait and skip cutscene
            _darkSouls.Teleport(new Vector3f(571.0f, 143.0f, 255.0f), 90);
            Thread.Sleep(1500);
            _assistant.Attack();

            Thread.Sleep(5500);
        }

        #endregion

        public void Sleep(int millis)
        {
            Thread.Sleep(millis);
        }
    }
}
