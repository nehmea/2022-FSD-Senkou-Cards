using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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
            
            try
            {   
                
                SenkoucardsConfig Sc = new SenkoucardsConfig();
                //Does user already exist?
                users dbUser = (from u in Sc.users where u.username == username select u).FirstOrDefault<users>();
                if(dbUser != null)
                {
                    throw new ArgumentException("User Already Exists!");
                }

                users user = new users { username = username, password = password, confirmPassword = confirmPassword, score = 0 };
                
                Sc.users.Add(user);
                Sc.SaveChanges();

                this.DialogResult = true;
                Login login = new Login("Successfully Registered!");
                login.ShowDialog();
            }
            catch (ArgumentException ex)
            {
                Lbl_ValidationErr.Content = ex.Message;

            }
            catch (DbEntityValidationException ex)
            {
                foreach (var errors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in errors.ValidationErrors)
                    {
                        Lbl_ValidationErr.Content = validationError.ErrorMessage;
                    }
                }
                
            }
                
        }

        private void Btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Login login = new Login();
            login.ShowDialog();
        }
    }
}
