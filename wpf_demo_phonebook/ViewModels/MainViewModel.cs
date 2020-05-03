using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Windows;
using wpf_demo_phonebook.ViewModels.Commands;

namespace wpf_demo_phonebook.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        bool newUser = false;
        private ContactModel selectedContact;

        public ContactModel SelectedContact
        {
            get => selectedContact;
            set { 
                selectedContact = value;
                newUser = false;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ContactModel> contacts = new ObservableCollection<ContactModel>();

        public ObservableCollection<ContactModel> Contacts
        {
            get => contacts;
            set
            {
                contacts = value;
                OnPropertyChanged();
            }
        }

        private string criteria;

        public string Criteria
        {
            get { return criteria; }
            set { 
                criteria = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand SearchContactCommand { get; set; }
        public RelayCommand UpdateContactCommand { get; set; }
        public RelayCommand DeleteContactCommand { get; set; }
        public RelayCommand NewContactCommand { get; set; }

        public MainViewModel()
        {
            SearchContactCommand = new RelayCommand(SearchContact);
            UpdateContactCommand = new RelayCommand(UpdateContact);
            DeleteContactCommand = new RelayCommand(DeleteContact);
            NewContactCommand = new RelayCommand(NewContact);

            Contacts = PhoneBookBusiness.LoadData();
            SelectedContact = PhoneBookBusiness.GetContactByID(1);
        }

        private void SearchContact(object parameter)
        {
            string input = parameter as string;
            int output;
            string searchMethod;
            if (!Int32.TryParse(input, out output))
            {
                searchMethod = "name";
            } else
            {
                searchMethod = "id";
            }

            switch (searchMethod)
            {
                case "id":
                    SelectedContact = PhoneBookBusiness.GetContactByID(output);
                    break;
                case "name":
                    Contacts = PhoneBookBusiness.GetxContactsByName(input);
                    SelectedContact = PhoneBookBusiness.GetContactByName(input);
                    break;
                default:
                    MessageBox.Show("Unkonwn search method");
                    Contacts = PhoneBookBusiness.LoadData();
                    break;
            }

            newUser = false;
        }

        private void UpdateContact(object parameter)
        {
            if(newUser == false)
            {
                PhoneBookBusiness.UpdateContact(SelectedContact);
            }
            else
            {
                PhoneBookBusiness.NewContact(SelectedContact);
                newUser = false;
            }
            
            
        }

        private void DeleteContact(object parameter)
        {
            if (newUser == false)
            {
                MessageBoxResult result = MessageBox.Show("Etes vous sure de vouloir supprimer ce contact", "Supprimer", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        PhoneBookBusiness.DeleteContact(SelectedContact);
                        Contacts = PhoneBookBusiness.LoadData();
                        SelectedContact = PhoneBookBusiness.GetContactByID(1);
                        MessageBox.Show("Contact Supprimer");
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
            else
            {
                SelectedContact = PhoneBookBusiness.GetContactByID(1);
                newUser = false;
            }
        }

        private void NewContact(object parameter)
        {
            SelectedContact = new ContactModel();
            newUser = true;
        }

    }
}
