using System;
using System.Collections.ObjectModel;
using System.Linq;
using JapaneseCheckers.Models;
using JapaneseCheckers.Views;

namespace JapaneseCheckers.ViewModels
{
    public class MainViewModel : MvvmBase, IDisposable
    {
        public static AccountsData AccountsData {get;set; } = new();
        public static PlayersData PlayersData {get;set;} = new();
        public static BotsData BotsData {get;set;} = new();
        public static PlayedGamesData PlayedGamesData {get;set;} = new();
        public static ObservableCollection<Account> LoggedAccounts {get;set;} = new();

        public MainViewModel()
        {
            Login = new RelayCommand(LoginClick);
            Registration = new RelayCommand(RegistrationClick);
            DisplayGamesList = new RelayCommand(DisplayGamesListClick);
            DisplayPlayersList = new RelayCommand(DisplayPlayersListClick);
            StartNewCommand = new RelayCommand(StartNewClick, (_) => LoggedAccounts.Count > 0);

            AddBots();
        }

        private void DisplayPlayersListClick(object obj)
        {
            new PlayersListWindow(this).ShowDialog();
        }

        private void DisplayGamesListClick(object obj)
        {
            new GamesListWindow(this).ShowDialog();
        }

        public RelayCommand Login { get; }
        public RelayCommand Registration { get; }
        public RelayCommand StartNewCommand { get; }
        public RelayCommand DisplayPlayersList { get; }
        public RelayCommand DisplayGamesList { get; }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        private static void AddBots()
        {
            Cell GetRandomCell(CellBoard cellBoard)
            {
                var cells = cellBoard.Where(cell => cell.IsFree);
                var cell1 = cells.ElementAt(new Random().Next(cells.Count()));
                return cell1;
            }

            if (BotsData.Collection.Count == 0)
            {
                BotsData.Collection.Add(new Bot("Random Bot"));
                BotsData.Collection.Add(new Bot("Smarter Bot"));
            }
            (BotsData.Collection[0] as Bot).CalculateMove =(board) =>
            {
                var cell = GetRandomCell(board);
                return (cell.Row, cell.Col);
            };
            var bot = BotsData.Collection[1] as Bot;
            bot.CalculateMove =(board) =>
            {
                var move = (0, 0);
                var qual = 0;
                int[][] directions =
                {
                    new[] { 1, 0 },
                    new[] { 1, -1 },
                    new[] { 1, 1 },
                    new[] { 0, 1 }
                };
                foreach (var direction in directions)
                {
                    var dx = direction[0];
                    var dy = direction[1];

                    for (var x = 0; x < CellBoard.BoardSize; x++)
                    {
                        for (var y = 0; y < CellBoard.BoardSize; y++)
                        {
                            try
                            {
                                var w = board[x, y].Color;
                                if (w == Color.None) continue;
                                if (w == board[x + dx, y + dy].Color)
                                {
                                    if (w == board[x + 2 * dx, y + 2 * dy].Color)
                                    {
                                        if (w == board[x + 3 * dx, y + 3 * dy].Color)
                                        {
                                            if (board[x + 4 * dx, y + 4 * dy].IsFree)
                                            {
                                                if (qual < 4)
                                                {
                                                    qual = 4;
                                                    move = (x + 4 * dx, y + 4 * dy);
                                                }
                                            }

                                            if (board[x - dx, y - dy].IsFree)
                                            {
                                                if (qual < 4)
                                                {
                                                    qual = 4;
                                                    move = (x - dx, y - dy);
                                                }
                                            }
                                        }

                                        if (board[x + 3 * dx, y + 3 * dy].IsFree)
                                        {
                                            if (qual < 3)
                                            {
                                                qual = 3;
                                                move = (x + 3 * dx, y + 3 * dy);
                                            }
                                        }

                                        if (board[x - dx, y - dy].IsFree)
                                        {
                                            if (qual < 3)
                                            {
                                                qual = 3;
                                                move = (x - dx, y - dy);
                                            }
                                        }
                                    }

                                    if (board[x + 2 * dx, y + 2 * dy].IsFree)
                                    {
                                        if (qual < 2)
                                        {
                                            qual = 2;
                                            move = (x + 2 * dx, y + 2 * dy);
                                        }
                                    }

                                    if (board[x - dx, y - dy].IsFree)
                                    {
                                        if (qual < 2)
                                        {
                                            qual = 2;
                                            move = (x - dx, y - dy);
                                        }
                                    }
                                }

                                if (board[x + 1 * dx, y + 1 * dy].IsFree)
                                {
                                    if (qual < 1)
                                    {
                                        qual = 1;
                                        move = (x + 1 * dx, y + 1 * dy);
                                    }
                                }

                                if (board[x - dx, y - dy].IsFree)
                                {
                                    if (qual < 1)
                                    {
                                        qual = 1;
                                        move = (x - dx, y - dy);
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                //
                            }
                        }
                    }
                }

                if (qual != 0) return move;
                var cell = GetRandomCell(board);
                return (cell.Row, cell.Col);
            };
        }

        private void StartNewClick(object obj)
        {
            var result = new SelectPlayers();
            if (result.ShowDialog() == false) return;

            var data = result.DataContext as SelectPlayersViewModel;
            var first = data.FirstPlayer;
            var second = data.SecondPlayer;
            var onRating = data.OnRating;
            var game = new GameWindow();
            var gameData = game.DataContext as CheckersViewModel;
            gameData.FirstPlayer = first;
            gameData.SecondPlayer = second;
            gameData.PlayedOnRating = onRating;
            gameData.Update();
            game.ShowDialog();
        }

        private void LoginClick(object sender)
        {
            new Login().ShowDialog();
        }

        private void RegistrationClick(object sender)
        {
            new Registration().ShowDialog();
        }

        private void ReleaseUnmanagedResources()
        {
            AccountsData.Dispose();
            PlayersData.Dispose();
            PlayedGamesData.Dispose();
            BotsData.Dispose();
        }

        ~MainViewModel()
        {
            ReleaseUnmanagedResources();
        }
    }
}