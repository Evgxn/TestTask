using System;
using System.IO;
using System.Net.Http;
using System.Windows;
using Microsoft.Win32;
using Newtonsoft.Json;
using TestTask.Models;

namespace TestTask
{
    public partial class AddCardWindow : Window
    {
        private string _copyImgName;

        public AddCardWindow()
        {
            InitializeComponent();
        }

        private void BtnOpenFile_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();

                openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";

                openFileDialog.ShowDialog();

                _copyImgName = openFileDialog.FileName;

                MessageBox.Show("Картинка добавлена успешно");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private async void AddMethod()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TextBoxName.Text))
                {
                    MessageBox.Show("Введите название карточки");
                }
                else
                {
                    if (_copyImgName == null)
                    {
                        MessageBox.Show("Выберите картинку");
                    }
                    else
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            var formContent = new Card
                            {
                                Name = TextBoxName.Text,
                                Img = _copyImgName
                            };

                            string json = JsonConvert.SerializeObject(formContent);

                            var content = new StringContent(
                                json,
                                System.Text.Encoding.UTF8,
                                "application/json");

                            var response = await client.PostAsync("https://localhost:44309/api/Card/create-card", content);
                            response.EnsureSuccessStatusCode();

                            if (response.IsSuccessStatusCode)
                            {
                                MessageBox.Show("Карта успешно добавлена");
                                this.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void AddCard_OnClick(object sender, RoutedEventArgs e)
        {
            AddMethod();
        }
    }
}
