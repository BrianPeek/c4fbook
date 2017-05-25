using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InnerTube
{
    /// <summary>
    /// Interaction logic for RatingUserControl.xaml
    /// </summary>

    public partial class RatingUserControl : System.Windows.Controls.UserControl
    {
        public RatingUserControl()
        {
            InitializeComponent();            
        }


        public double StarRating
        {
            get { return (double)GetValue(StarRatingProperty); }
            set { 
                //SetValue(StarRatingProperty, value);                
            } //can't write custom logic in a Dependency Property setter
        }

        // Using a DependencyProperty as the backing store for StarRating.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StarRatingProperty =
            DependencyProperty.Register("StarRating",
                                        typeof(double),
                                        typeof(RatingUserControl),
                                        new UIPropertyMetadata(0.0,
                                        RatingUserControl.RatingPropertyChanged));


       private static void RatingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
       {
            //static method so must get object
        	RatingUserControl rating = (RatingUserControl)d;

            double newRating;            
            bool test = double.TryParse(e.NewValue.ToString(), out newRating);

            if (test)
			{
                //You can only have a zero through 5 rating
                if (newRating <= 5.0 && newRating >= 0.0)
                {
                    //set values
                    rating.StarToolTip.Content = newRating.ToString();
                    rating.SetStars(newRating);                       
                }
			}
        }



       private void SetStars(double newRating)
        {
            int StarredIndex = CalculateIndex(newRating);     
      
            //loop through all the star halves
            for (int i = 0; i < StarStackPanel.Children.Count; i++)
            {
                Path star = (Path)StarStackPanel.Children[i];
                
                if (i <= StarredIndex)
                {
                    SetStyle(star, i, true);
                }
                else
                {
                    SetStyle(star, i, false);
                }
            }
            
        }

        private void SetStyle(Path star, int i, bool Selected)
        {
            //even numbers use "SelectedLeft" style
            if ((i % 2) == 0)
            {
                if (Selected)
                {
                    star.Style = (Style)(this.Resources["SelectedLeft"]);
                }
                else
                {
                    star.Style = (Style)(this.Resources["EmptyLeft"]);
                }

            }
            else
            {
                if (Selected)
                {
                    star.Style = (Style)(this.Resources["SelectedRight"]);
                }
                else
                {
                    star.Style = (Style)(this.Resources["EmptyRight"]);
                }
                
            }
            
        }



        private int CalculateIndex(double value)
        {
            //Get the ".yy" part of  "x.yy" (3.14)
            var actual = value - Math.Floor(value);

            //compare the newRating and choose to round 
            //to either: 0, 0.5, or 1
            if (actual < .25)
            {
                actual = 0;
            }
            else if (actual >= .25 && actual < .75)
            {
                actual = .5;
            }
            else if (actual >= .75)
            {
                actual = 1;
            }

            //add the base number to the rounded number and double
            //whole num + rounded value, then * 2 to get a valid index, 
            //then subtract by 1 (zero-based index)
            double newNum = ((Math.Floor(value) + actual) * 2) - 1 ;

            return (int)newNum;       
        }


    }
}