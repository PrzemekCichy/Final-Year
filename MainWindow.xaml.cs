using System;
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
using Newtonsoft.Json; //For manipulating JSON Data
using System.IO; //Reading file.
using MahApps.Metro.Controls;
using Microsoft.Win32;
using System.Windows.Controls.Primitives;


namespace WpfApplication1
{
    public partial class MainWindow : MetroWindow
    {
        //List of Car objects
        List<Car> carList;

        //Sorted list of refernces to the object in carList
        List<Car> SortedCarList;

        //Used for Copy/Cut/Paste
        Car tempCar;

        string path = @"";

        //For viewing Purposes
        int selectedCar = 0;

        Window window = new Window
        {
            Title = "My User Control Dialog",
            Content = new UserControl(),
            Height = 200,  // just added to have a smaller control (Window)
            Width = 240
        };
        //        window.ShowDialog();


        public MainWindow()
        {
            InitializeComponent();

            //List of Car objects which holds parsed cars from JSON file. Passed parameter is a json string.
            //This string is deserialized and loaded into cars list
            carList = JsonConvert.DeserializeObject< List<Car>>(loadJson() );

            //
            SortedCarList = carList;

            updateContext();
        }

        public void updateContext()
        {
            if (selectedCar < SortedCarList.Count)
            {
                this.DataContext = SortedCarList[selectedCar];
            }
            else if (0 < selectedCar)
            {
                selectedCar--;
                this.DataContext = SortedCarList[selectedCar];
            }
            else if (SortedCarList.Count == 0)
            {
                selectedCar = 0;
                this.DataContext = SortedCarList[selectedCar];
                SortedCarList.Add( new Car() );
            }
            Label_CarNumber.Content = selectedCar + 1 + "/" + SortedCarList.Count;
            displayRadio();
        }

        //returns JSON string
        public string loadJson(string path = "../../database.json")
        {
            //Path to the file defined by user should be passed here, otherwise default location of file 
            //Defined as shown. Needs to be completed by ali/stuart
            using (StreamReader r = new StreamReader(path))
                return r.ReadToEnd();
        }

        //converts a List object into JSON string and saves into file
        //Defined as shown. Needs to be completed by ali
        public void saveJson(ref List<Car> carList, string path = "../../database.json")
        {
            string json = JsonConvert.SerializeObject(carList, Formatting.Indented);
            using (StreamWriter sw = new StreamWriter(path))
                sw.Write(json);
        }

        private void button_Next_Click(object sender, RoutedEventArgs e)
        {
            //Check whether the object in the list is last
            if (carList.Count > selectedCar + 1)
            {
                ++selectedCar;
                updateContext();
            }
        }

        private void button_Previous_Click(object sender, RoutedEventArgs e)
        {
            //Check whether the object in the list is first
            if (0 < selectedCar)
            {
                --selectedCar;
                updateContext();
            }
        }

        //ordering a list of object references, not duplicating the objects themselves. 
        //While this does double the memory used by the list of references it's not as bad as actually duplicating all of the objects themselves
        private void sortByBrand(object sender, RoutedEventArgs e)
        {
            SortedCarList = carList.OrderBy(o => o.Brand).ToList();
            updateContext();
        }


        private void sortByYear(object sender, RoutedEventArgs e)
        {
            SortedCarList = carList.OrderBy(o => o.Year).ToList();
            updateContext();

        }

        private void sortByPrice(object sender, RoutedEventArgs e)
        {
            SortedCarList = carList.OrderBy(o => o.Price).ToList();
            updateContext();

        }

        //Adds String from textbox to information list. 
        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            SortedCarList[selectedCar].Information.Add(TextBox_addInformations.Text);
            //Refresh listbox to reflect the changes
            listBox_Information.Items.Refresh();
            TextBox_addInformations.Text = "";
        }

        //Remove selected item from listbox
        private void Clear_Selected_Informations(object sender, RoutedEventArgs e)
        {
            if (listBox_Information.SelectedIndex >= 0)
            {
                SortedCarList[selectedCar].Information.RemoveAt(listBox_Information.SelectedIndex);
                listBox_Information.Items.Refresh();
            }
        }


        //Creates new car at the end of a list
        private void menu_newCar_Click(object sender, RoutedEventArgs e)
        {
            carList.Add( new Car() );
            selectedCar = carList.Count -1;
            updateContext();
        }

        private void menu_Cut_Click(object sender, RoutedEventArgs e)
        {
            //let tempCar be equal to new instance of a car, with an object passed to a constructor
            tempCar = new Car( SortedCarList[selectedCar] );
            SortedCarList.RemoveAt(selectedCar);
            updateContext();
        }

        private void menu_Copy_Click(object sender, RoutedEventArgs e)
        {
                tempCar = new Car(SortedCarList[selectedCar]);
        }

        private void menu_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (SortedCarList.Count > 0)
            {
                SortedCarList.RemoveAt(selectedCar);
                updateContext();
            }
        }

        private void menu_Paste_Click(object sender, RoutedEventArgs e)
        {
            if(tempCar != null)
            {
                SortedCarList.Insert(++selectedCar, tempCar);
                updateContext();
            }
        }

        //Select correct radiobutton based on property
        public void displayRadio()
        {
            switch (SortedCarList[selectedCar].BodyType)
            {
                case "Hatchback":
                    Hatchback.IsChecked = true;
                    break;
                case "MPV":
                    MPV.IsChecked = true;
                    break;
                case "SUV":
                    SUV.IsChecked = true;
                    break;
                case "Saloon":
                    Saloon.IsChecked = true;
                    break;
                case "Convertible":
                    Convertible.IsChecked = true;
                    break;
                case "Coupe":
                    Coupe.IsChecked = true;
                    break;
                case "Estate":
                    Estate.IsChecked = true;
                    break;
                default:
                    Hatchback.IsChecked = false;
                    MPV.IsChecked = false;
                    SUV.IsChecked = false;
                    Saloon.IsChecked = false;
                    Convertible.IsChecked = false;
                    Coupe.IsChecked = false;
                    Estate.IsChecked = false;
                    break;
            };

            switch (SortedCarList[selectedCar].Gearbox)
            {
                case "Automatic":
                    Automatic.IsChecked = true;
                    break;
                case "Manual":
                    Manual.IsChecked = true;
                    break;
                default:
                    Manual.IsChecked = false;
                    Automatic.IsChecked = false;
                    break;
            }     
        }

        private void radio_Button_Click(object sender, RoutedEventArgs e)
        {
            var radio = sender as RadioButton;
            SortedCarList[selectedCar].BodyType = radio.Name;
        }

        private void radio_GearboxButton_Click(object sender, RoutedEventArgs e)
        {
            var radio = sender as RadioButton;
            SortedCarList[selectedCar].Gearbox = radio.Name;
        }

        private void menu_open_click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = "json";
            openFileDialog.Filter = "Json files (*.json)|*.json|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                //path saved for save function
                path = openFileDialog.FileName;
                carList = JsonConvert.DeserializeObject<List<Car>>( loadJson(openFileDialog.FileName) );
                SortedCarList = carList;
                updateContext();
            }
        }

        private void menu_saveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = "json";
            saveFileDialog.Filter = "Json files (*.json)|*.json|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                //path saved for save function
                path = saveFileDialog.FileName;
                saveJson(ref SortedCarList, saveFileDialog.FileName);
            }
        }

        private void menu_Save_Click(object sender, RoutedEventArgs e)
        {
            if (path != "")
                saveJson(ref SortedCarList, path);
            else
                menu_saveAs_Click(null, null);
        }
    }
}

