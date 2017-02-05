using CocktailMaker;
using DAL;
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
    /// Interaction logic for CocktailsResult.xaml
    /// </summary>
    public partial class CocktailsResult : Window
    {
        List<Cocktail> cocktails;
        List<Buttle> buttles;
        MainWindow window;
        public CocktailsResult(List<Cocktail> _cocktails, List<Buttle> _buttles,MainWindow _window)
        {
            InitializeComponent();
            buttles = _buttles;
            cocktails = _cocktails;
            window = _window;
            BuildCocktailList();
            buildButtleList();
            string fullPath = System.IO.Path.GetFullPath("../../Resources/no_image.jpg"); //TODO: replace it with a normal thing
            imgPhoto.Source = new BitmapImage(new Uri(fullPath));
        }

        private void buildButtleList()
        {
            string buttlesForList = "";
            foreach (Buttle buttle in buttles)
            {
                string type = buttle.Type.ToString();
                buttlesForList = buttlesForList + buttle.Name + " - " + type + "\n";
            }
            this.buttleList.Text = buttlesForList;
        }

        private void BuildCocktailList()
        {
            //Build the item list
            List<string> items = new List<string>();
            foreach (Cocktail cocktail in cocktails)
            {
                items.Add(cocktail.name);
            }
            //Populate the ComboBox from the item list
            cocktailList.ItemsSource = items;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.window.showWindow();
            this.Close();
        }

        private void cocktailList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string cock = cocktailList.SelectedItem.ToString();
            Cocktail cocktail = findCocktail(cock);
            if (cocktail == null) return;
            string fullPath = System.IO.Path.GetFullPath("../../Resources/cocktails"); //TODO: replace it with a normal thing
            fullPath = fullPath + "/" + cocktail.pic;
            imgPhoto.Source = new BitmapImage(new Uri(fullPath));
        }

        private Cocktail findCocktail(string cock)
        {
            foreach (Cocktail cocktail in cocktails)
            {
                if (cocktail.name.Equals(cock))
                {
                    return cocktail;
                }
            }
            return null;
        }
    }
}
