using System;
using System.IO;
using System.Net.Http;
using System.Windows;
using Microsoft.Win32;
using Newtonsoft.Json;
using TestTask.Models;

namespace TestTask
{
    public partial class EditCardWindow : Window
    {
        private string _copyImgName;

        public EditCardWindow(int id, string name,string img)
        {
            InitializeComponent();
            labelId.Content = id;
            TextBoxName.Text = name;
            lableImg.Content = img;
        }

        private async void Edit_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TextBoxName.Text))
                {
                    MessageBox.Show("Название картинки не может быть пустым");
                }
                else
                {
                    using (HttpClient client = new HttpClient())
                    {
                        var formContent = new Card()
                        {
                            Id = Convert.ToInt32(labelId.Content.ToString()),
                            Name = TextBoxName.Text,
                            Img = _copyImgName != null ? _copyImgName : lableImg.Content != null ? lableImg.Content.ToString() : " ",
                        };

                        string json = JsonConvert.SerializeObject(formContent);

                        var content = new StringContent(
                            json,
                            System.Text.Encoding.UTF8,
                            "application/json");

                        var result = await client.PostAsync("https://localhost:44309/api/Card/update-card", content);
                        result.EnsureSuccessStatusCode();

                        if (result.IsSuccessStatusCode)
                        {
                            MessageBox.Show("Картка успешно изменена");
                            this.Close();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void BtnOpenFile_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();

                openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";

                openFileDialog.ShowDialog();

                var fileNamePath = openFileDialog.FileName;

                var fileName = Guid.NewGuid();

                File.Copy(fileNamePath, "..\\..\\..\\Images\\" + fileName + ".png", true);

                _copyImgName = "D:\\proj\\TestTask\\TestTask\\Images\\" + fileName + ".png";
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}
