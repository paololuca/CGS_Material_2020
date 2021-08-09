using System.Text;
using System.Windows;
using Resources;
using BusinessEntity.Entity;
using BusinessEntity.Type;

namespace HEMATournamentSystem
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private readonly bool _connectionAvailable;


        public Login()
        {
            InitializeComponent();
            _connectionAvailable = Helper.TestConnectionString();
            //TODOPL da spostare var f = new FighterStats();
            //TODOPL da spostare f.Show();

            var currentUser = SqlDal_MasterDB.CheckLogin("admin");
            var main = new MainWindow(currentUser);
            main.Show();

            this.Close();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (!_connectionAvailable)
            {
                PopUpBoxes.ShowPopup("Unavailable DB Connection");
                return;
            }

            if (txtBoxUsername.Text == "")
                PopUpBoxes.ShowPopup("User Name cannot be empty");
            else if (txtBoxPassword.Password == "")
                PopUpBoxes.ShowPopup("Password cannot be empty");
            else
            {
                StringBuilder sb = EncryptionHelper.GetEncryptedPassword(txtBoxPassword.Password);

                var currentUser = SqlDal_MasterDB.CheckLogin(txtBoxUsername.Text);

                if (currentUser == null)
                    PopUpBoxes.ShowPopup("Username not valid");
                else if (sb.ToString().ToUpper() != currentUser.Password)
                    PopUpBoxes.ShowPopup("Uncorrect Password");
                else if (!currentUser.IsEnabled)
                {
                    PopUpBoxes.ShowPopup("The user '"+ txtBoxUsername.Text + "' is not active");
                }
                else if(currentUser.Type == ProfileType.None)
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
