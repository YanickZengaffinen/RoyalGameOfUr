using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static RoyalGameOfUr.Player;

namespace RoyalGameOfUr.Client.Game
{
    public class Field : Button
    {
        static Field()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Field), new FrameworkPropertyMetadata(typeof(Field)));
        }


        public static readonly DependencyProperty FieldIdProperty = DependencyProperty.Register(nameof(FieldId), typeof(string), typeof(Field));
        public string FieldId
        {
            get { return (string)GetValue(FieldIdProperty); }
            set { SetValue(FieldIdProperty, value); }
        }

        public int Index
            => int.Parse(FieldId.Substring(1));

        public static readonly DependencyProperty BoardProperty = DependencyProperty.Register(nameof(Board), typeof(Board), typeof(Field), new PropertyMetadata(new PropertyChangedCallback(OnBoardChanged)));
        public Board Board
        {
            get { return (Board)GetValue(BoardProperty); }
            set { SetValue(BoardProperty, value); }
        }

        public PlayerId PlayerId
        {
            get { return Board[FieldId]; }
        }

        public static void OnBoardChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            var field = sender as Field;

            if (args.OldValue is Board oldBoard)
            {
                oldBoard.Changed -= field.Repaint;
            }

            if (args.NewValue is Board newBoard)
            {
                newBoard.Changed += field.Repaint;
            }

            field.Repaint(null, EventArgs.Empty);
        }

        public void Repaint(object sender, EventArgs args)
        {
            if(Index >= 0)
            {
                switch (PlayerId)
                {
                    case PlayerId.A:
                        this.Background = Brushes.Blue;
                        break;
                    case PlayerId.B:
                        this.Background = Brushes.Red;
                        break;
                    case PlayerId.None:
                        this.Background = Brushes.Gray;
                        break;
                    default:
                        break;
                }

                if(Board.IsRosette(Index))
                {
                    this.Content = "Rosette";
                }
            }
        }

    }
}
