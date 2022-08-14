using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using Newtonsoft.Json;
using TestTask.Models;

namespace TestTask
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            OutputJson();
        }

        private async void OutputJson()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync("https://localhost:44309/api/Card/get-card");
                    response.EnsureSuccessStatusCode();

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        Card.ItemsSource = JsonConvert.DeserializeObject<List<Card>>(json);
                    }
                    else
                    {
                        MessageBox.Show("Error");
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void Reload_OnClick(object sender, RoutedEventArgs e)
        {
            OutputJson();
        }

        private void Add_OnClick(object sender, RoutedEventArgs e)
        {
            AddCardWindow addCardWindow = new AddCardWindow();
            addCardWindow.ShowDialog();
        }

        private async void Update_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    if (Card.SelectedItems != null && Card.SelectedItems.Count == 1)
                    {
                        var cards = Card.SelectedItems.Cast<Card>().ToList();

                        foreach (var card in cards)
                        {
                            var formContent = new IdModel
                            {
                                Id = card.Id
                            };

                            string json = JsonConvert.SerializeObject(formContent);

                            var content = new StringContent(
                                json,
                                System.Text.Encoding.UTF8,
                                "application/json");

                            var response = await client.PostAsync("https://localhost:44309/api/Card/get-card-by-id", content);

                            var returnJsonCard = await response.Content.ReadAsStringAsync();

                            var returnIdCard = JsonConvert.DeserializeObject<Card>(returnJsonCard).Id;
                            var returnNameCard = JsonConvert.DeserializeObject<Card>(returnJsonCard).Name;
                            var returnImgCard = JsonConvert.DeserializeObject<Card>(returnJsonCard).Img;

                            new EditCardWindow(returnIdCard, returnNameCard, returnImgCard).ShowDialog();
                        }
                    }
                    else if (Card.SelectedItems == null)
                    {
                        MessageBox.Show("Выберите карточку для изменения");
                    }
                    else
                    {
                        MessageBox.Show("Выберите одну карточку из таблицы");
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private async void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    if (Card.SelectedItems != null && Card.SelectedItems.Count > 0)
                    {
                        var cards = Card.SelectedItems.Cast<Card>().ToList();

                        foreach (var card in cards)
                        {
                            var formContent = new IdModel
                            {
                                Id = card.Id
                            };

                            string json = JsonConvert.SerializeObject(formContent);

                            var content = new StringContent(
                                json,
                                System.Text.Encoding.UTF8,
                                "application/json");

                            var response = await client.PostAsync("https://localhost:44309/api/Card/delete-card", content);
                            response.EnsureSuccessStatusCode();

                            OutputJson();
                        }
                    }
                    else if (Card.SelectedItems == null)
                    {
                        MessageBox.Show("Выберите карточку, которую хотите удалить");
                    }
                    else
                    {
                        MessageBox.Show("Выберите хотя бы одну карточку из таблицы");
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}
