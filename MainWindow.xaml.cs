using Microsoft.Win32;
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

namespace SVPP_CS_WPF_Lab2_task2_Product_Card
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Product product = new Product();

        public MainWindow()
        {
            InitializeComponent();
            init_otherComponents();
        }

        /// <summary>
        /// Инициализация ComboBox категорий.
        /// </summary>
        private void init_ComboBoxCategories(ref ComboBox box)
        {
            foreach(Categories category in Enum.GetValues(typeof(Categories)))
            { 
                ComboBoxItem item = new ComboBoxItem();
                item.Content = category;
                if(category == Categories.Другое) item.IsSelected= true;
                box.Items.Add(item);
            }
        }
        /// <summary>
        /// Инициализация DatePicker даты принятия по умолчанию.
        /// </summary>
        private void init_DatePickerAdoptionDate(DateTime? defaultDate=null)
        {
            if (defaultDate is not null)
                DatePicker_AdoptionDate.SelectedDate = ((DateTime)defaultDate).Date;
            else DatePicker_AdoptionDate.SelectedDate = DateTime.Now.Date;
        }
        /// <summary>
        /// Инициализация дополнительных данных
        /// </summary>
        private void init_otherComponents()
        {
            init_ComboBoxCategories( ref ComboBox_Categories);
            init_DatePickerAdoptionDate();
        }

        /// <summary>
        /// Открывает окно загрузки фото после нажатия левой кнопки мыше по элементу изображения
        /// </summary>

        private void Image_ProductPhoto_MouseLeft_Click(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Photo (.png,jpg)|*.png;*jpg";
            dlg.FileName = "";

            bool? result = dlg.ShowDialog();

            if (result == true)
            {

                Image_ProductPhoto.Source = new BitmapImage(new Uri(dlg.FileName, UriKind.RelativeOrAbsolute));
            }
            else { return; }


        }

        /// <summary>
        /// Обработчик кнопки 'Сохранить в файл'. Считывает введенные данные в объект, сериализует объект в файл json.
        /// </summary>
        private void Button_SaveInFile_Click(object sender, RoutedEventArgs e)
        {
            // Обработка ввода дробного числа через точку или запятую в поле Цена.
            if (TextBox_Price.Text.Contains("."))
            {
                TextBox_Price.Text = TextBox_Price.Text.Replace(".", ",");
            }
            //Обработка jотсувствия ввода даты или б/у опции (null значения).
            DateTime? adoptionDate = DatePicker_AdoptionDate.SelectedDate;
            bool? bu = CheckBox_BU.IsChecked;

            try
            {
                product.ArticleId = Int32.Parse(TextBox_Article.Text);
                product.Name = TextBox_Name.Text;
                product.Manufacturer = TextBox_Manufacturer.Text;
                product.AdoptionDate = adoptionDate is not null? 
                    (DateTime) adoptionDate : DateTime.MinValue;
                product.BU = bu is not null? (bool)bu : false;
                product.Price = Double.Parse(TextBox_Price.Text);
                product.Description = TextBox_Description.Text;
                product.Category = (Categories)((ComboBoxItem)ComboBox_Categories.SelectedItem).Content;
                product.PhotoPath = Image_ProductPhoto.Source.ToString();
            } catch (Exception)
            {
                MessageBox.Show("Введенные данные некорректны.", "Ошибка");
                return;
            }

            string path  = product.inJson();
            MessageBox.Show($"Файл {path} сохранен", "Сохранение");

        }

        /// <summary>
        /// Обработчик кнопки Очистить. Очищает поля ввода и объект.
        /// </summary>
        private void Button_Clear_Click(object sender, RoutedEventArgs e)
        {
            product = new Product();
            TextBox_Article.Text = string.Empty;
            TextBox_Name.Text = string.Empty;
            TextBox_Manufacturer.Text = string.Empty;
            DatePicker_AdoptionDate.SelectedDate = DateTime.Now.Date;
            CheckBox_BU.IsChecked = false;
            TextBox_Price.Text = string.Empty;
            TextBox_Description.Text = string.Empty;
            Image_ProductPhoto.Source = new BitmapImage(new Uri("/images/no_image.jpg", UriKind.RelativeOrAbsolute));

            foreach(ComboBoxItem category in ComboBox_Categories.Items)
            {
                category.IsSelected = true;
            }
        }

        /// <summary>
        /// Обработчик кнопки 'Загрузить из файла'. Загружает JSON файл, создает объект и заполняет 
        /// поля ввода данными из файла.
        /// </summary>
        private void Button_ReadFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dg = new OpenFileDialog();
            dg.Filter = "Product JSON (.JSON)|*.JSON";
            dg.FileName = "";

            bool? result = dg.ShowDialog();

            if (result is null || result == false) return;
            try
            {
                product = Product.CreateFromJSON(dg.FileName);
            }
            catch(Exception)
            {
                MessageBox.Show("Неверная конфигурация или файл поврежден", "Ошибка");
                return;
            }
            
            TextBox_Article.Text = product.ArticleId.ToString();
            TextBox_Name.Text = product.Name;
            TextBox_Manufacturer.Text = product.Manufacturer;
            DatePicker_AdoptionDate.SelectedDate = product.AdoptionDate;
            CheckBox_BU.IsChecked = product.BU;
            TextBox_Price.Text = product.Price.ToString();
            TextBox_Description.Text = product.Description;
            Image_ProductPhoto.Source = new BitmapImage(new Uri(product.PhotoPath, UriKind.RelativeOrAbsolute));
            
            foreach(ComboBoxItem item in ComboBox_Categories.Items)
            {
                if(((Categories)item.Content) == product.Category)
                {
                    item.IsSelected = true;
                }
            }

        }
    }
}