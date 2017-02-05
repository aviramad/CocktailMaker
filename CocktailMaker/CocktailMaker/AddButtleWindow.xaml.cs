using BL;
using DAL;
using CocktailMaker;
using Microsoft.Win32;
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

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for AddButtleWindow.xaml
    /// </summary>
    public partial class AddButtleWindow : Window
    {
        string source;
        IBL logic;
        MainWindow window;

        public AddButtleWindow(MainWindow _window, IBL _logic)
        {
            InitializeComponent();
            init(_logic, _window);
        }

        private void init(IBL _logic, MainWindow _window)
        {
            BuildComboBox();
            this.drinkName.Text = "";
            this.drinkType.Text = "";
            this.window = _window;
            this.logic = _logic;
            source = "";
            error_pic.Visibility = System.Windows.Visibility.Hidden;
            string fullPath = System.IO.Path.GetFullPath("../../Resources/no_image.jpg");
            imgPhoto.Source = new BitmapImage(new Uri(fullPath));
        }

        private void BuildComboBox()
        {
            //Build the item list
            List<string> items = new List<string>();
            items.Add(ButtleType.Vodka.ToString());
            items.Add(ButtleType.Gin.ToString());
            items.Add(ButtleType.Rum.ToString());
            items.Add(ButtleType.Taquilla.ToString());
            items.Add(ButtleType.Wiskey.ToString());
            items.Add(ButtleType.Vermouth.ToString());
            items.Add(ButtleType.Liquor.ToString());
            //Populate the ComboBox from the item list
            drinkType.ItemsSource = items;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.window.showWindow();
            this.Close();
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                source = op.FileName;
                imgPhoto.Source = new BitmapImage(new Uri(source));
            }
        }

        private bool checkInput()
        {
            error_pic.Visibility = System.Windows.Visibility.Hidden;
            bool ans = true;
            if (this.source.Equals("") || this.drinkName.Text.Equals("") || this.drinkType.Text.Equals(""))
            {
                ans = false;
                error_pic.Visibility = System.Windows.Visibility.Visible;
            } else {
                if (logic.isButtleExist(this.drinkName.Text))
                {
                    ans = false;
                    MessageBox.Show("Bottle is already exists");
                }
                else {
                    if (this.drinkName.Text.Length > DB.MAX_NAME_LENGTH)
                    {
                        ans = false;
                        MessageBox.Show("Please choose shorter name");
                    }
                }
            }
            return ans;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (checkInput())
            {
                this.logic.AddButtle(this.drinkType.Text, this.drinkName.Text, this.source);
                MessageBox.Show("The Bottle '" + this.drinkName.Text + "' has been added successfully!");
                this.window.showWindow();
                this.Close();
            }
        }
    }
}
