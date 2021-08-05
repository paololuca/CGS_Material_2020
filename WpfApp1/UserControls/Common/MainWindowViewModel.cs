using MaterialDesignThemes.Wpf;
using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using BusinessEntity.Entity;
using BusinessEntity.Type;

namespace HEMATournamentSystem
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _searchKeyword;
        private readonly LoginUser user;
        private ObservableCollection<WindowsItem> _allItems;
        private ObservableCollection<WindowsItem> _demoItems;
        private WindowsItem _selectedItem;

        public MainWindowViewModel(LoginUser user, ISnackbarMessageQueue snackbarMessageQueue)
        {
            this.user = user;
            _allItems = GenerateDemoItems(snackbarMessageQueue);
            FilterItems(null);
        }

        public string SearchKeyword
        {
            get { return _searchKeyword; }
            set
            {
                _searchKeyword = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WindowsItems)));
                FilterItems(_searchKeyword);
            }
        }

        public ObservableCollection<WindowsItem> WindowsItems
        {
            get { return _demoItems; }
            set
            {
                _demoItems = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WindowsItems)));
            }
        }

        public WindowsItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (value == null || value.Equals(_selectedItem)) return;

                _selectedItem = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedItem)));
            }
        }


        private ObservableCollection<WindowsItem> GenerateDemoItems(ISnackbarMessageQueue snackbarMessageQueue)
        {
            if (snackbarMessageQueue == null)
                throw new ArgumentNullException(nameof(snackbarMessageQueue));

            var items = new ObservableCollection<WindowsItem>();

            items.Add(new WindowsItem("Home", new Home(), null));

            LoadUserMenu(items);

            return items;
        }

        private void LoadUserMenu(ObservableCollection<WindowsItem> items)
        {
            if (user.Type != ProfileType.None)
            {
                //if (user.IsAdmin)
                    //create tournament (con controllo se già non è stato creato)

                items.Add(new WindowsItem("Play Tournament", new Pools(user), null));

                if (user.IsAdmin)
                {
                    items.Add(new WindowsItem("Manage Associations", new Associations(user), null));
                    items.Add(new WindowsItem("Manage Fighters", new Fighters(user), null));
                    items.Add(new WindowsItem("Manage Tournaments", new Tournaments(user), null));
                    items.Add(new WindowsItem("System Settings", new Settings(user), null));
                }

                //report
            }
        }

        private void FilterItems(string keyword)
        {
            var filteredItems =
                string.IsNullOrWhiteSpace(keyword) ?
                _allItems :
                _allItems.Where(i => i.Name.ToLower().Contains(keyword.ToLower()));

            WindowsItems = new ObservableCollection<WindowsItem>(filteredItems);
        }
    }
}