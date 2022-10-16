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

namespace SenkouCards
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        public Register()
        {
            InitializeComponent();
        }

        private void Btn_Register_Click(object sender, RoutedEventArgs e)
        {
            string username = Tbx_Username.Text;
            string password = Pbx_Password.Password;
            string confirmPassword=Pbx_ConfirmPass.Password;

            if(password!=confirmPassword)
            {
                MessageBox.Show("Passwords must match","Invalid Entry",MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }
            Globals.SenkouDbAuto.users.Add(new users { username=username, password=password, score=0});
            Globals.SenkouDbAuto.SaveChanges();
        }
    }
}
