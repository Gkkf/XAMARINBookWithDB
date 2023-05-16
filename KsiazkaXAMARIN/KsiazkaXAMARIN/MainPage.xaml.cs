using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KsiazkaXAMARIN
{
    public partial class MainPage : ContentPage
    {
        public int page = 0;
        public int pageSize = 0;
        public string searchBarText = "";

        public MainPage()
        {
            InitializeComponent();

            Functions.Open();
            Functions.refresh = Load;
            Load();
        }

        public void Load()
        {
            if(Functions.GetPersons(page, searchBarText).Count <= 0)
            {
                page--;
                pageCount.Text = (page + 1).ToString();
            }

            var count = Functions.MaxPage(searchBarText);
            if(count == 0)
            {
                count = 1;
                page = 0;
                pageCount.Text = (page+1).ToString();
            }

            pageSize = Functions.MaxPage();
            
            listView.ItemsSource = Functions.GetPersons(page, searchBarText);
            listView.SelectedItem = null;
        }


        //dodawanie nowej bazy
        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            NewDBPage newDB = new NewDBPage();
            await Navigation.PushAsync(newDB);

            newDB.Disappearing += NewDB_Disappearing;
        }

        //wczytywanie bazy
        private async void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {
            NewDBPage newDB = new NewDBPage();
            await Navigation.PushAsync(newDB);

            newDB.Disappearing += NewDB_Disappearing;
        }

        private void NewDB_Disappearing(object sender, EventArgs e)
        {
            Load();
        }

        //dodawanie do bazy
        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            OptionsPage page = new OptionsPage();
            page.toEdit = false;
            await Navigation.PushAsync(page);
        }

        //edycja po przytrzymaniu
        private async void MenuItem_Clicked_1(object sender, EventArgs e)
        {
            var selected = (Person)((MenuItem)sender).BindingContext;

            if(selected != null)
            {
                OptionsPage optionsPage = new OptionsPage(selected.Name, selected.Surname, selected.Number, selected.Email);
                optionsPage.toEdit  = true;
                optionsPage.person = selected;
                await Navigation.PushAsync(optionsPage);
            }
        }

        //usuwanie po przytrzymaniu
        private void MenuItem_Clicked_2(object sender, EventArgs e)
        {
            var selected = (Person)((MenuItem)sender).BindingContext;

            Functions.DeletePerson(selected);
            Load();
        }

        //stronnicowanie przód
        private void next_Clicked(object sender, EventArgs e)
        {
            if (page >= pageSize)
            {
                return;
            }

            ++page;

            if (progress.Progress < Functions.MaxPage())
            {
                progress.Progress += 1.0/Functions.MaxPage();
            }

            pageCount.Text = (page + 1).ToString();
            Load();
        }

        //stronnicowanie tył
        private void back_Clicked(object sender, EventArgs e)
        {
            if (page <= 0)
            {
                return;
            }

            --page;

            if (progress.Progress > 0)
            {
                progress.Progress -= 1.0 / Functions.MaxPage();
            }

            pageCount.Text = (page + 1).ToString();
            Load(); 
        }

        //searchbar
        private async void MenuItem_Clicked_3(object sender, EventArgs e)
        {
            var result = await DisplayPromptAsync("", "Wpisz wyszukiwany tekst", "OK", "Anuluj");

            if(result != null)
            {
                searchBarText = result;
            }
            else
            {
                searchBarText = "";
            }
            Load();
        }
    }
}
