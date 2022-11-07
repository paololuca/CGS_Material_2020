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
            
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (!_connectionAvailable)
            {
                new MessageBoxCustom("Unavailable DB Connection", MessageType.Error, MessageButtons.Ok).ShowDialog();
                return;
            }

            if (txtBoxUsername.Text == "")
                new MessageBoxCustom("User Name cannot be empty", MessageType.Warning, MessageButtons.Ok).ShowDialog();
            else if (txtBoxPassword.Password == "")
                new MessageBoxCustom("Password cannot be empty", MessageType.Warning, MessageButtons.Ok).ShowDialog();
            else
            {
                StringBuilder sb = EncryptionHelper.GetEncryptedPassword(txtBoxPassword.Password);

                var currentUser = SqlDal_MasterDB.CheckLogin(txtBoxUsername.Text);

                if (currentUser == null)
                    new MessageBoxCustom("Username not valid", MessageType.Error, MessageButtons.Ok).ShowDialog();
                else if (sb.ToString().ToUpper() != currentUser.Password)
                    new MessageBoxCustom("Uncorrect Password", MessageType.Error, MessageButtons.Ok).ShowDialog();
                else if (!currentUser.IsEnabled)
                {
                    new MessageBoxCustom("The user '"+ txtBoxUsername.Text + "' is not active", MessageType.Warning, MessageButtons.Ok).ShowDialog();
                }
                else if(currentUser.Type == ProfileType.None)
                {
                    new MessageBoxCustom("The user '" + txtBoxUsername.Text + "' is not allowed due its privileges", MessageType.Warning, MessageButtons.Ok).ShowDialog();
                }
                else
                {

                    //var f = HemaRatingsHelper.SyncFigthersAsync();
                    //f.Wait();
                    //var c = HemaRatingsHelper.SyncClubsAsync();
                    //c.Wait();
                    //HemaRatingsHelper.SyncFightersBetweenDBs();

                    var main = new MainWindow(currentUser);
                    main.Show();

                    this.Close();
                }
            }
        }

        
    }

}
