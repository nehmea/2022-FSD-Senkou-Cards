using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace SenkouCards
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        bool loggedIn = false;
        public Login()
        {
            InitializeComponent();
        }

        private void Btn_Login_Click(object sender, RoutedEventArgs e)
        {
            string username = Tbx_Username.Text;
            string password = Pbx_Password.Password;

            try
            {
                users user = (from u in Globals.SenkouDbAuto.users where u.username == username select u).FirstOrDefault<users>();
                if(user==null)
                {
                    Lbl_RegistrationMessage.Foreground = new SolidColorBrush(Colors.Red);
                    Lbl_RegistrationMessage.Content = "Username/Password does not exist!";

                }
                else if (user.password == password)//nullreferenceex
                {
                    loggedIn = true;
                    Globals.ActiveUser = user;
                    this.DialogResult = true;
                }
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void Btn_Register_Click(object sender, RoutedEventArgs e)
        {
            //test code, delete later
            //MessageBox.Show(Globals.ActiveUser.username, "Username", MessageBoxButton.OK);
            //this.DialogResult = false;
            Register register = new Register(this);//passing the login window object to make display registration success message from registration dialogue
            register.ShowDialog();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(!loggedIn)
            {
                e.Cancel = true;
               Environment.Exit(0); //exits the whole app
            }    
            
        }

        private void Lbl_CtnOffline_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Globals.ActiveUser = new users { id = -1, username="Local User", password=""};
            loggedIn = true;
            this.DialogResult = true;
        }
    }
}
