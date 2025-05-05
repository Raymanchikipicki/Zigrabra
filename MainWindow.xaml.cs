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

namespace CompanyPartApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ComponyPartEntities db = new ComponyPartEntities();
        
        public MainWindow()
        {
            InitializeComponent();
            TakePartners();
        }

        private int Calculate(double Num)
        {
            if (Num > 300000)
                return 15;
            else if (Num > 50000)
                return 10;
            else if (Num > 10000)
                return 5;
            else
                return 0;
        }

        private void TakePartners()
        {
            var InfoParts = from p in db.Partners_
                            join t in db.PartnerType on p.Тип_партнера equals t.id
                            select new
                            {
                                id = p.id,
                                name = p.Наименование_партнера,
                                type = t.Наименование,
                                FIO = p.Фамилия + " " + p.Имя + " " + p.Отчество,
                                phone = p.Телефон_партнера,
                                rate = p.Рейтинг
                            };
            foreach (var part in InfoParts)
            {
                WrapPanel wp = new WrapPanel();
                wp.Width = 500;


                TextBlock type = new TextBlock
                {
                    Text = part.type + " | " + part.name,
                    FontSize = 16,
                    Margin = new Thickness(10, 0, 0, 0),
                    Width = wp.Width - 250
                };

                List<Order> orders = db.Order.Where(x => x.Партнер == part.id).ToList();
                double sum = 0;
                for (int i = 0; i < orders.Count; i++)
                {
                    sum += Convert.ToDouble(orders[i].Стоймость);
                }
                int Skidka = Calculate(sum);

                TextBlock discountTxt = new TextBlock
                {
                    Text = Skidka + "%",
                    FontSize = 16,
                    Margin = new Thickness(0, 0, 0, 0),
                    TextAlignment = TextAlignment.Right,
                    Width = wp.Width - 50,
                    HorizontalAlignment = HorizontalAlignment.Right
                };

                TextBlock role = new TextBlock
                {
                    Text = part.FIO,
                    FontSize = 12,
                    Margin = new Thickness(10, 0, 0, 0),
                    Width = wp.Width

                };

                TextBlock phone = new TextBlock
                {
                    Text = part.phone,
                    FontSize = 12,
                    Margin = new Thickness(10, 0, 0, 0),
                };

                TextBlock rating = new TextBlock
                {
                    Text = "Рейтинг: " + part.rate,
                    FontSize = 12,
                    Margin = new Thickness(10, 0, 0, 0),
                    Width = wp.Width
                };

                wp.Children.Add(type);
                wp.Children.Add(discountTxt);
                wp.Children.Add(role);
                wp.Children.Add(phone);
                wp.Children.Add(rating);

                listParts.Items.Add(wp);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)//Добавить партнера
        {
            RedactPartner rp = new RedactPartner();
            rp.Show();
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)//История
        {
            History h = new History();
            h.Show();
            Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)//Расчет
        {
            CalculateCount cc = new CalculateCount();
            cc.Show();
            Close();
        }
    }
}
