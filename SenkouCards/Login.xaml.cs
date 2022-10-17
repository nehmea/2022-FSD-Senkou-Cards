using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login(string registerSuccess="")
        {
            InitializeComponent();
            Lbl_RegistrationMessage.Content = registerSuccess;
        }


        private void Btn_Login_Click(object sender, RoutedEventArgs e)
        {
            string username=Tbx_Username.Text;
            string password = Pbx_Password.Password;

            try
            {
                users user = (from u in Globals.SenkouDbAuto.users where u.username == username select u).FirstOrDefault<users>();
                if(user.password==password)
                {
                    Globals.ActiveUser = user;
                    this.DialogResult = true;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void Btn_Register_Click(object sender, RoutedEventArgs e)
        {
            //test code, delete later
            //MessageBox.Show(Globals.ActiveUser.username, "Username", MessageBoxButton.OK);
            this.DialogResult = false;
            Register register = new Register();
            register.ShowDialog();
        }
    }
}
