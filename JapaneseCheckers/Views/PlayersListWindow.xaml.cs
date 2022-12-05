using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using JapaneseCheckers.ViewModels;

namespace JapaneseCheckers.Views
{
    /// <summary>
    /// Логика взаимодействия для PlayersListWindow.xaml
    /// </summary>
    public partial class PlayersListWindow : Window
    {
        public PlayersListWindow()
        {
            
            InitializeComponent();
        }
        public PlayersListWindow(MainViewModel mvm) : this()
        {
            DataContext = mvm;
        }
    }
}
