using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace TZ_Nurzhan_WPF
{
    public partial class MainWindow : Window
    {
        private static string connectionString = @"Data Source=DESKTOP-DKM570K\MYDBSERVER;Initial Catalog=TZ_Nurzhan;Integrated Security=True";

        public MainWindow()
        {
            InitializeComponent();

            cityComboBox.ItemsSource = GetDataFromDatabase("SELECT ID, Name FROM Cities");
            teamComboBox.ItemsSource = GetDataFromDatabase("SELECT ID, Name FROM Teams");
            shiftComboBox.ItemsSource = GetDataFromDatabase("SELECT ID, Name FROM Shifts");
        }

        private void cityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedCity = cityComboBox.SelectedItem as MyData;
            if (selectedCity != null)
            {
                workshopComboBox.ItemsSource = GetDataFromDatabase($"SELECT ID, Name FROM Workshops WHERE CityID = {selectedCity.ID}");
            }
            else
            {
                workshopComboBox.ItemsSource = null;
            }
        }



        private void workshopComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedWorkshop = workshopComboBox.SelectedItem as MyData;
            if (selectedWorkshop != null)
            {
                employeeComboBox.ItemsSource = GetDataFromDatabase($"SELECT ID, Name FROM Employees WHERE WorkshopID = {selectedWorkshop.ID}");
            }
            else
            {
                employeeComboBox.ItemsSource = null;
            }
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            var formData = new
            {
                cityId = (cityComboBox.SelectedItem as MyData)?.ID,
                workshopId = (workshopComboBox.SelectedItem as MyData)?.ID,
                employeeId = (employeeComboBox.SelectedItem as MyData)?.ID,
                teamId = (teamComboBox.SelectedItem as MyData)?.ID,
                shiftId = (shiftComboBox.SelectedItem as MyData)?.ID
            };

            System.IO.File.WriteAllText("formdata.json", Newtonsoft.Json.JsonConvert.SerializeObject(formData));
            System.Windows.MessageBox.Show("Файл успешно сохранен!\nПроверьте путь TZ_Nurzhan_WPF\\TZ_Nurzhan_WPF\\bin\\Debug\\net5.0-windows\\formdata.json");
        }

        public List<MyData> GetDataFromDatabase(string query)
        {
            var data = new List<MyData>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            data.Add(new MyData
                            {
                                ID = (int)reader["ID"],
                                Name = reader["Name"].ToString()
                            });
                        }
                    }
                }
            }

            return data;
        }
    }

    public class MyData
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
