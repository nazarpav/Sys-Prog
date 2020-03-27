using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tank_Description;

namespace _17_03_2020
{
    public partial class MainWindow : Window
    {
        private Stack<Tank_Description.Tank_Description> _tanksRedTeam = null;
        private Stack<Tank_Description.Tank_Description> _tanksGreenTeam = null;
        const uint BattleCount = 5;
        SoundPlayer sp = null;
        public MainWindow()
        {
            InitializeComponent();
            _tanksRedTeam = new Stack<Tank_Description.Tank_Description>();
            _tanksGreenTeam = new Stack<Tank_Description.Tank_Description>();
            sp = new SoundPlayer();
            sp.SoundLocation = "Shot1.wav";
            sp.Load();
        }
        private void StartBattle()
        {
            uint countRedWin = 0;
            uint countGreenWin = 0;
            FillBattleground();
            for (int i = 0; i < BattleCount; i++)
            {
                Dispatcher.Invoke(() => RedTeam.Items.Add(i.ToString() + " | " + _tanksRedTeam.Peek()));
                Dispatcher.Invoke(() => GreenTeam.Items.Add(i.ToString() + " | " + _tanksGreenTeam.Peek()));
                for (int k = 0; k < 3; k++)
                {
                    sp.Play();
                    Thread.Sleep(330);
                }
                Thread.Sleep(700);
                if (_tanksRedTeam.Pop() ^ _tanksGreenTeam.Pop())
                {
                    countRedWin++;
                    Dispatcher.Invoke(() => Battleground.Items.Add(i.ToString() + " | T_34 is winer"));
                }
                else
                {
                    countGreenWin++;
                    Dispatcher.Invoke(() => Battleground.Items.Add(i.ToString() + " | Pantera is winer"));
                }
                Thread.Sleep(1000);
            }

        }
        private void FillBattleground()
        {
            _tanksRedTeam.Clear();
            _tanksGreenTeam.Clear();
            for (int i = 0; i < BattleCount; i++)
            {
                _tanksRedTeam.Push(new Tank_Description.Tank_Description("T_34"));
                _tanksGreenTeam.Push(new Tank_Description.Tank_Description("Pantera"));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(StartBattle);
        }
    }
}
