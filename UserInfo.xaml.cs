using System;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.Maui.Controls;

namespace fuzzy
{
    public partial class UserInfo : ContentPage
    {

        private string username;

        public string Username
        {
            get => username;
            set
            {
                username = Uri.UnescapeDataString(value);
                // Use username in Dashboard, e.g. show welcome message
            }
        }

        public UserInfo()
        {
            InitializeComponent();
        }

        private async void OnSubmitClicked(object sender, EventArgs e)
        {
            ResultLabel.IsVisible = false;

            // Validate Organization ID (optional)
            int? orgId = null;
            if (!string.IsNullOrWhiteSpace(OrganizationIdEntry.Text))
            {
                if (int.TryParse(OrganizationIdEntry.Text, out int parsedOrgId))
                    orgId = parsedOrgId;
                else
                {
                    await DisplayAlert("Error", "Organization ID must be a number.", "OK");
                    return;
                }
            }

            // Validate Role
            if (RolePicker.SelectedIndex == -1)
            {
                await DisplayAlert("Error", "Please select a role.", "OK");
                return;
            }
            string role = RolePicker.Items[RolePicker.SelectedIndex];

            // Validate Birthdate (required)
            DateTime birthdate = BirthdatePicker.Date;
            if (birthdate > DateTime.Today)
            {
                await DisplayAlert("Error", "Birthdate cannot be in the future.", "OK");
                return;
            }

            // Full name (optional)
            string fullName = FullNameEntry.Text?.Trim();

            // Phone (optional)
            string phone = PhoneEntry.Text?.Trim();

            var userInfoData = new
            {
                organization_id = orgId,
                role = role,
                birthdate = birthdate.ToString("yyyy-MM-dd"),
                full_name = fullName,
                phone = phone
            };

            // TODO: send userInfoData to server here via HttpClient

            ResultLabel.Text = "User info submitted (locally)";
            ResultLabel.IsVisible = true;
        }
    }
}
