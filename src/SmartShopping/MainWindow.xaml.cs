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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using SmartShoppingLibrary;

namespace SmartShopping
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        String[] basketarray = new string[400];
        BindingList<string> basketList = new BindingList<string>();
        decimal totalprice = 0;
        SmartShoppingData ssd;

        public MainWindow()
        {
            //string filename = @"C:\Users\troels\troe3159\SC\src\SmartShoppingMerge\savefiles\databaseTest.ssd";
            string filename = @"..\..\..\savefiles\databaseTest.ssd";
            ssd = SmartShoppingData.Load(filename);
            InitializeComponent();
            //basketListBox.ItemsSource = basketarray;
            basketListBox.ItemsSource = basketList;
            string url = ssd.Shops[0].Products[0].CanonicalProduct.ImageURL;
            for (int i = 0; i < 20; i++)
            {
                foreach (Product product in ssd.Shops[0].Products.Values) 
                {
                    addProductToGui(product.CanonicalProduct.Uid, product.Price, @"ProductPictures\" + product.CanonicalProduct.ImageURL);    
                }
                
            }
            foreach (Product product in ssd.Shops[0].Products.Values) 
            {
                addProductToGui(product.CanonicalProduct.Uid, product.Price, @"ProductPictures\" + product.CanonicalProduct.ImageURL);    
            }
        }

        public void addProductToGui(int uid, decimal price, string imageUrl)
        {
            //Main window
            StackPanel productWindow = new StackPanel();
            productWindow.Height = 135;
            productWindow.Width = 135;
            productWindow.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            productWindow.Margin = new Thickness(15, 15, 15, 0);
            productWindow.Background = Brushes.Goldenrod;

            //bitmapPicture.UriSource = new Uri(//@"C:\Users\ubereski\Documents\Visual Studio 2012\Projects\SmartShopping\SmartShopping\produktbilleder\guleroedder.jpg", UriKind.Absolute);
            //Product picture 
            Image productPicture = new Image();
            BitmapImage bitmapPicture = new BitmapImage();
            bitmapPicture.BeginInit();
            bitmapPicture.UriSource = new Uri(imageUrl, UriKind.Relative);
            bitmapPicture.EndInit();
            productPicture.Height = 109;
            productPicture.Width = 125;
            productPicture.Stretch = Stretch.Fill;
            productPicture.Source = bitmapPicture;
            productPicture.MouseLeftButtonDown += addToBasket;
            productPicture.Name = "_" + uid.ToString();


            //Price-panel for combobox, price, dataPrice, currency
            DockPanel pricePanel = new DockPanel();
            pricePanel.Width = 135;
            pricePanel.Height = 26;
            pricePanel.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            pricePanel.Background = Brushes.Green;

            //Quantity selecter
            ComboBox productQuantity = new ComboBox();
            productQuantity.Width = 31;
            DockPanel.SetDock(productQuantity, Dock.Left);
            //Dropdown options - #1
            ComboBoxItem productQuantitySelect1 = new ComboBoxItem();
            productQuantitySelect1.Content = 1;
            productQuantitySelect1.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            productQuantitySelect1.Width = 30;
            // #2
            ComboBoxItem productQuantitySelect2 = new ComboBoxItem();
            productQuantitySelect2.Content = 2;
            productQuantitySelect2.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            productQuantitySelect2.Width = 30;
            // #3
            ComboBoxItem productQuantitySelect3 = new ComboBoxItem();
            productQuantitySelect3.Content = 3;
            productQuantitySelect3.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            productQuantitySelect3.Width = 30;
            // #4
            ComboBoxItem productQuantitySelect4 = new ComboBoxItem();
            productQuantitySelect4.Content = 4;
            productQuantitySelect4.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            productQuantitySelect4.Width = 30;
            // #5
            ComboBoxItem productQuantitySelect5 = new ComboBoxItem();
            productQuantitySelect5.Content = 5;
            productQuantitySelect5.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            productQuantitySelect5.Width = 30;

            productQuantity.SelectedItem = productQuantitySelect1;

            //Label with the text "Pris:"
            Label productPriceText = new Label();
            productPriceText.Content = "Pris:";
            productPriceText.Width = 31;
            DockPanel.SetDock(productPriceText, Dock.Left);

            //Label with the text "kr,-"
            Label productCurrency = new Label();
            productCurrency.Content = "kr,-";
            productCurrency.Width = 27;
            DockPanel.SetDock(productCurrency, Dock.Right);

            //Label with the price taken from the database
            Label productPriceVar = new Label();
            productPriceVar.Content = price.ToString("n2");
            productPriceVar.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            DockPanel.SetDock(productPriceVar, Dock.Right);

            //Build-a-product            
            productWrapPanel.Children.Add(productWindow);
            productWindow.Children.Add(productPicture);
            productWindow.Children.Add(pricePanel);
            pricePanel.Children.Add(productQuantity);
            productQuantity.Items.Add(productQuantitySelect1);
            productQuantity.Items.Add(productQuantitySelect2);
            productQuantity.Items.Add(productQuantitySelect3);
            productQuantity.Items.Add(productQuantitySelect4);
            productQuantity.Items.Add(productQuantitySelect5);
            pricePanel.Children.Add(productPriceText);
            pricePanel.Children.Add(productCurrency);
            pricePanel.Children.Add(productPriceVar);
        }

        private void addToBasket(object sender, MouseButtonEventArgs e)
        {
            
            Image clickedImage = sender as Image;
            int uid = int.Parse(clickedImage.Name.Substring(1));
            Product product = ssd.Shops[0].Products[uid];
            StackPanel clickedStackPanel = clickedImage.Parent as StackPanel;
            DockPanel clickedDockPanel = (DockPanel)VisualTreeHelper.GetChild(clickedStackPanel, 1);
            ComboBox clickedComboBox = (ComboBox)VisualTreeHelper.GetChild(clickedDockPanel, 0);
            int quantity = int.Parse(clickedComboBox.Text);
            basketList.Add(quantity + " " + product.CanonicalProduct.Name + " á " + product.Price + " kr");
            totalprice += quantity * product.Price;
            totalPriceLabel.Content = totalprice;


        }
    }
}
