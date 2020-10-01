using MaterialDesignThemes.Wpf;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using Resources;
using BusinessEntity.Entity;
using FormsManagement;

namespace HEMATournamentSystem
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();

            if (!Helper.TestConnectionString())
                PopUpBoxes.ShowPopup("Unavailable DB Connection");

            var f = new FighterStats();
            f.Show();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (txtBoxUsername.Text == "")
                PopUpBoxes.ShowPopup("User Name cannot be empty");
            else if (txtBoxPassword.Password == "")
                PopUpBoxes.ShowPopup("Password cannot be empty");
            else
            {
                StringBuilder sb = EncryptionHelper.GetEncryptedPassword(txtBoxPassword.Password);

                var currentUser = HelperMasterDB.CheckLogin(txtBoxUsername.Text);

                if (currentUser == null)
                    PopUpBoxes.ShowPopup("Username not valid");
                else if (sb.ToString().ToUpper() != currentUser.Password)
                    PopUpBoxes.ShowPopup("Uncorrect Password");
                else if (!currentUser.IsEnabled)
                {
                    PopUpBoxes.ShowPopup("The user '"+ txtBoxUsername.Text + "' is not active");
                }
                else if(currentUser.Type == LoginProfile.None)
                {
                    PopUpBoxes.ShowPopup("The user '" + txtBoxUsername.Text + "' is not allowed to access due its privileges");
                }
                else
                {
                    var main = new MainWindow(currentUser);
                    main.Show();

                    this.Close();
                }
            }
        }         
    }

}
