using System;
using CocktailMaker;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BL;
using DAL;
using Microsoft.Win32;
using WpfApplication2;
using System.Threading;
using System.ComponentModel;

namespace CocktailMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string source;
        IBL logic;
        DB db;
        List<Buttle> result;
        List<Cocktail> cocktails;
        BackgroundWorker bg;
        public MainWindow()
        {
            InitializeComponent();
            DB _db = new DB();
            IBL _logic = new CBL(_db);
            result = new List<Buttle>();
            cocktails = new List<Cocktail>();
            init(_logic, _db);
        }

        public MainWindow(IBL _logic, DB _db)
        {
            InitializeComponent();
            init(_logic, _db);
        }

        private void init(IBL _logic, DB _db)
        {
            this.db = _db;
            this.logic = _logic;
            source = "";
            process_msg.Visibility = System.Windows.Visibility.Hidden;
            error_pic.Visibility = System.Windows.Visibility.Hidden;
            string fullPath = System.IO.Path.GetFullPath("../../Resources/no_image.jpg"); //TODO: replace it with a normal thing
            imgPhoto.Source = new BitmapImage(new Uri(fullPath));
            createWorker();
        }

        private void createWorker()
        {
            bg = new BackgroundWorker();
            bg.DoWork += new DoWorkEventHandler(bg_findBottles);
            bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bg_findBottlesCompleted);
        }

        private void bg_findBottlesCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            cocktails = logic.findCoctails(result);
            showCocktails(cocktails, result);
        }

        private void bg_findBottles(object sender, DoWorkEventArgs e)
        {
            result = logic.findButtles(source);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.logic.killMatlab();
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

        private void cocktail_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = checkInput();
            if (isValid)
            {
                cocktails = new List<Cocktail>();
                result = new List<Buttle>();
                createWaitingMessage();
                bg.RunWorkerAsync();
            }
        }

        private void createWaitingMessage()
        {
            this.process_msg.Visibility = System.Windows.Visibility.Visible;
            this.cocktail_btn.Content = "PLEASE WAIT...";
        }

        private void showCocktails(List<Cocktail> cocktails, List<Buttle> buttles)
        {
            CocktailsResult window = new CocktailsResult(cocktails, buttles, this);
            window.Show();
            this.Hide();
        }

        private bool checkInput()
        {
            error_pic.Visibility = System.Windows.Visibility.Hidden;
            bool ans = true;
            if (source.Equals(""))
            {
                ans = false;
                error_pic.Visibility = System.Windows.Visibility.Visible;
            }
            return ans;
        }

        private void showMessage(List<string> message)
        {
            string res = "Drinks:\n";
            foreach (string s in message)
            {
                res = res + s + "\n";
            }
            MessageBox.Show(res);
        }

        private void addButtle_Click(object sender, RoutedEventArgs e)
        {
            AddButtleWindow window = new AddButtleWindow(this, this.logic);
            window.Show();
            this.Hide();
        }

        public void showWindow()
        {
            this.Show();
            source = "";
            process_msg.Visibility = System.Windows.Visibility.Hidden;
            cocktail_btn.Content = "FIND A COCKTAIL!";
            error_pic.Visibility = System.Windows.Visibility.Hidden;
            string fullPath = System.IO.Path.GetFullPath("../../Resources/no_image.jpg"); //TODO: replace it with a normal thing
            imgPhoto.Source = new BitmapImage(new Uri(fullPath));
            result = new List<Buttle>();
            cocktails = new List<Cocktail>();
        }
    }
}
