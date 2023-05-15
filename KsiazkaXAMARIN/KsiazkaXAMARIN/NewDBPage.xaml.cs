using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static System.Net.Mime.MediaTypeNames;

namespace KsiazkaXAMARIN
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewDBPage : ContentPage
	{
		public NewDBPage ()
		{
			InitializeComponent ();
		}

        private async void AddButton_Clicked(object sender, EventArgs e)
        {
            string name = eName.Text;

            Regex regex = new Regex("^[A-Za-z0-9_]+$");

            if (!regex.IsMatch(name))
            {
                eName.BackgroundColor = Color.HotPink;
            }
            else
            {
                Functions.AddTable(name);
                await Navigation.PopAsync();
            }
        }

        private void eName_TextChanged(object sender, TextChangedEventArgs e)
        {
            eName.BackgroundColor = Color.White;
        }
    }
}