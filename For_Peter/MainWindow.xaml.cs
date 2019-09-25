using System;
using System.IO;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Data;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace For_Peter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string json;
        public MainWindow()
        {
            InitializeComponent();
            
        }

       

        private void fillTable()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                json = File.ReadAllText(openFileDialog.FileName, Encoding.UTF8);
                DataTable data = Tabulate(json);

                dataGrid.ItemsSource = data.DefaultView;
            }
        }
            public static DataTable Tabulate(string json)
        {
            var jsonLinq = JObject.Parse(json);
            var srcArray = jsonLinq.Descendants().Where(d => d is JArray).First();
            var trgArray = new JArray();
            foreach (JObject row in srcArray.Children<JObject>())
            {
                var cleanRow = new JObject();
                foreach (JProperty column in row.Properties())
                {
                    if (column.Value is JValue)
                    {
                        cleanRow.Add(column.Name, column.Value);
                    }
                    else {
                     
                            foreach (JProperty c in column.Value)
                            {
                            if (c.Value is JValue)
                            {
                                if (!c.Name.Contains(column.Name))
                                    cleanRow.Add(column.Name + "-" + c.Name, c.Value);
                                else
                                    cleanRow.Add(c.Name, c.Value);
                            }
                           

                            }
                       
                        
                    }
                }

                trgArray.Add(cleanRow);
            }

            return JsonConvert.DeserializeObject<DataTable>(trgArray.ToString());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.fillTable();
        }
        


    }
}
