using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SillyControlCenter_WPF.daima
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MianbanMaster : ContentPage
    {
        public ListView ListView;

        public MianbanMaster()
        {
            InitializeComponent();

            BindingContext = new MianbanMasterViewModel();
            ListView = MenuItemsListView;
        }

        class MianbanMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MianbanMasterMenuItem> MenuItems { get; set; }

            public MianbanMasterViewModel()
            {
                MenuItems = new ObservableCollection<MianbanMasterMenuItem>(new[]
                {
                    new MianbanMasterMenuItem { Id = 0, Title = "Page 1" },
                    new MianbanMasterMenuItem { Id = 1, Title = "Page 2" },
                    new MianbanMasterMenuItem { Id = 2, Title = "Page 3" },
                    new MianbanMasterMenuItem { Id = 3, Title = "Page 4" },
                    new MianbanMasterMenuItem { Id = 4, Title = "Page 5" },
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}