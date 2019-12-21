using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Gallery
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> myGallery;
        private Image imageControl, imageControl1, imageControl2, imageControl3, imageControl4;
        private int counter, counterMain, counterTimer, timerSpanSpeed;
        private DispatcherTimer dispatcherTimer;
        public MainWindow()
        {
            InitializeComponent();
            //Loaded += new RoutedEventHandler(MainWindow_Loaded);
            dispatcherTimer = new DispatcherTimer();
            imageControl = new Image();
            imageControl1 = new Image();
            imageControl2 = new Image();
            imageControl3 = new Image();
            imageControl4 = new Image();
            counter = 0;
            counterMain = 0;
            counterTimer = 0;
            timerSpanSpeed = 3;//seconds
        }

        private void MenuChooseDir_Click(object sender, RoutedEventArgs e)
        {
            counter = 0;
            try
            {
                using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
                {
                    System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        string dirPath = dialog.SelectedPath;
                        string supportedExtensions = "*.jpg,*.gif,*.png,*.bmp,*.jpe,*.jpeg,*.ico,*.eps,*.tif";
                        myGallery = new List<string>();
                        foreach (string imageFile in Directory.GetFiles(dirPath, "*.*", SearchOption.AllDirectories).Where(s => supportedExtensions.Contains(System.IO.Path.GetExtension(s).ToLower())))
                        {
                            myGallery.Add(imageFile);
                        }
                    }
                }
            }
            catch(Exception ex) { MessageBox.Show("Something goes wrong! Please, try another folder!"); return; }
            var largeImage = new BitmapImage(new Uri(myGallery[0]));
            imageControl.Source = largeImage;
            imageControl.Height = 600;
            imageControl.Stretch = Stretch.Uniform;
            mainImage.Children.Clear();
            mainImage.Children.Add(imageControl);
            
            var Image1 = new BitmapImage(new Uri(myGallery[0]));
            imageControl1.Source = Image1;
            //imageControl1.Height = 300;
            imageControl1.Stretch = Stretch.Uniform;
            bottomImage1.Children.Clear();
            bottomImage1.Children.Add(imageControl1);

            var Image2 = new BitmapImage(new Uri(myGallery[1]));
            imageControl2.Source = Image2;
           // imageControl2.Height = 200;
            imageControl2.Stretch = Stretch.Uniform;
            bottomImage2.Children.Remove(imageControl2);
            bottomImage2.Children.Add(imageControl2);

            var Image3 = new BitmapImage(new Uri(myGallery[2]));
            imageControl3.Source = Image3;
            //imageControl3.Height = 300;
            imageControl3.Stretch = Stretch.Uniform;
            bottomImage3.Children.Remove(imageControl3);
            bottomImage3.Children.Add(imageControl3);

            var Image4 = new BitmapImage(new Uri(myGallery[3]));
            imageControl4.Source = Image4;
            //imageControl4.Height = 400;
            imageControl4.Stretch = Stretch.Uniform;
            bottomImage4.Children.Clear();
            bottomImage4.Children.Add(imageControl4);

        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonRev_Click(object sender, RoutedEventArgs e)
        {
            counter--; counterMain = 0;
            if (counter >= 0)
            {
                var largeImage = new BitmapImage(new Uri(myGallery[counter]));
                imageControl.Source = largeImage;
                mainImage.Children.Clear();
                mainImage.Children.Add(imageControl);

                var Image1 = new BitmapImage(new Uri(myGallery[counter]));
                imageControl1.Source = Image1;
                bottomImage1.Children.Clear();
                bottomImage1.Children.Add(imageControl1);

                var Image2 = new BitmapImage(new Uri(myGallery[counter + 1]));
                imageControl2.Source = Image2;
                bottomImage2.Children.RemoveAt(1);
                bottomImage2.Children.Add(imageControl2);

                var Image3 = new BitmapImage(new Uri(myGallery[counter + 2]));
                imageControl3.Source = Image3;
                bottomImage3.Children.RemoveAt(1);
                bottomImage3.Children.Add(imageControl3);

                var Image4 = new BitmapImage(new Uri(myGallery[counter + 3]));
                imageControl4.Source = Image4;
                bottomImage4.Children.Clear();
                bottomImage4.Children.Add(imageControl4);
            }
        }

        private void ButtonForw_Click(object sender, RoutedEventArgs e)
        {
            ForwardAction();
        }

        
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("The Gallery App is developed by Bichurin S. / Academy Step");
        }



        private void dtTicker(object sender, EventArgs e)
        {
            counterTimer++;
            if (counterTimer + 3 < myGallery.Count)
            {
                var largeImage = new BitmapImage(new Uri(myGallery[counterTimer]));
                imageControl.Source = largeImage;
                mainImage.Children.Clear();
                mainImage.Children.Add(imageControl);
                counter = counterTimer;
                ForwardAction();
            }
            else
            {
                sliderOn.IsChecked = false;
                dispatcherTimer.Stop();
                MessageBox.Show("Turned OFF");
                counter--;
            }

        }

        private void MenuItemSlideOn_Click(object sender, RoutedEventArgs e)
        {
            if (sliderOn.IsChecked == true)
            {
                dispatcherTimer.Tick += new EventHandler(dtTicker);
                dispatcherTimer.Interval = new TimeSpan(0, 0, timerSpanSpeed);
                dispatcherTimer.Start();
                counterTimer = counter;
                MessageBox.Show("Turned ON");
            }
            if (sliderOn.IsChecked == false)
            {
                counterTimer = 0;
                dispatcherTimer.Stop();
                MessageBox.Show("Turned OFF");
            }
        }

        private void MenuIncreaseSp_Click(object sender, RoutedEventArgs e)
        {
            if (timerSpanSpeed - 3 > 0)
            timerSpanSpeed -= 3;
        }

        private void MenuItemDefault_Click(object sender, RoutedEventArgs e)
        {
            Style style = this.FindResource("DefaultTheme") as Style;
            myGrid.Style = style;
            Style styleb = this.FindResource("DefaultButton") as Style;
            leftButton.Style = styleb; rightButton.Style = styleb;
            Style styleborder = this.FindResource("DefaultBorder") as Style;
            mainBorder.Style = styleborder; miniBorder1.Style = styleborder;
            miniBorder2.Style = styleborder; miniBorder3.Style = styleborder; miniBorder4.Style = styleborder;
        }

        private void MenuItemLight_Click(object sender, RoutedEventArgs e)
        {
            Style style = this.FindResource("LightTheme") as Style;
            myGrid.Style = style;
            Style styleb = this.FindResource("LightThemeButton") as Style;
            leftButton.Style = styleb; rightButton.Style = styleb;
            Style styleborder = this.FindResource("LightThemeBorder") as Style;
            mainBorder.Style = styleborder; miniBorder1.Style = styleborder;
            miniBorder2.Style = styleborder; miniBorder3.Style = styleborder; miniBorder4.Style = styleborder;

        }

        private void MenuItemDark_Click(object sender, RoutedEventArgs e)
        {
            Style style = this.FindResource("DarkTheme") as Style;
            myGrid.Style = style;
            Style styleb = this.FindResource("DarkThemeButton") as Style;
            leftButton.Style = styleb; rightButton.Style = styleb;
            Style styleborder = this.FindResource("DarkThemeBorder") as Style;
            mainBorder.Style = styleborder; miniBorder1.Style = styleborder;
            miniBorder2.Style = styleborder; miniBorder3.Style = styleborder; miniBorder4.Style = styleborder;
        }

        private void MenuDecreaseSp_Click(object sender, RoutedEventArgs e)
        {
            timerSpanSpeed += 3;
        }

        private void mainImage_mouseUp(object sender, RoutedEventArgs e)
        {
            counterMain++;
            if (counterMain < 4)
            {
                var largeImage = new BitmapImage(new Uri(myGallery[counter + counterMain]));
                imageControl.Source = largeImage;
                mainImage.Children.Clear();
                mainImage.Children.Add(imageControl);
            }
        }

        private void bottomImage1_mouseUp(object sender, RoutedEventArgs e)
        {
            counterMain = 0;
            var largeImage = new BitmapImage(new Uri(myGallery[counter]));
            imageControl.Source = largeImage;
            mainImage.Children.Clear();
            mainImage.Children.Add(imageControl);
        }
        private void bottomImage2_mouseUp(object sender, RoutedEventArgs e)
        {
            counterMain = 0;
            if (counter + 1 < myGallery.Count)
            {
                var largeImage = new BitmapImage(new Uri(myGallery[counter + 1]));
                imageControl.Source = largeImage;
                mainImage.Children.Clear();
                mainImage.Children.Add(imageControl);
            }
        }
        private void bottomImage3_mouseUp(object sender, RoutedEventArgs e)
        {
            counterMain = 0;
            if (counter + 2 < myGallery.Count)
            {
                var largeImage = new BitmapImage(new Uri(myGallery[counter + 2]));
                imageControl.Source = largeImage;
                mainImage.Children.Clear();
                mainImage.Children.Add(imageControl);
            }
        }

        private void bottomImage4_mouseUp(object sender, RoutedEventArgs e)
        {
            counterMain = 0;
            if (counter + 3 < myGallery.Count)
            {
                var largeImage = new BitmapImage(new Uri(myGallery[counter + 3]));
                imageControl.Source = largeImage;
                mainImage.Children.Clear();
                mainImage.Children.Add(imageControl);
            }
        }

       private void ForwardAction()
        {
            counter++; counterMain = 0;
            if (counter + 3 <= myGallery.Count - 1)
            {
                var largeImage = new BitmapImage(new Uri(myGallery[counter]));
                imageControl.Source = largeImage;
                mainImage.Children.Clear();
                mainImage.Children.Add(imageControl);

                var Image1 = new BitmapImage(new Uri(myGallery[counter]));
                imageControl1.Source = Image1;
                bottomImage1.Children.Clear();
                bottomImage1.Children.Add(imageControl1);

                var Image2 = new BitmapImage(new Uri(myGallery[counter + 1]));
                imageControl2.Source = Image2;
                bottomImage2.Children.RemoveAt(1);
                bottomImage2.Children.Add(imageControl2);

                var Image3 = new BitmapImage(new Uri(myGallery[counter + 2]));
                imageControl3.Source = Image3;
                bottomImage3.Children.RemoveAt(1);
                bottomImage3.Children.Add(imageControl3);

                var Image4 = new BitmapImage(new Uri(myGallery[counter + 3]));
                imageControl4.Source = Image4;
                bottomImage4.Children.Clear();
                bottomImage4.Children.Add(imageControl4);
            }
        }

    }
}
