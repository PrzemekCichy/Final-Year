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
using MahApps.Metro; //Styling
using MahApps.Metro.Controls; //Styling
using Microsoft.Win32;
using System.Windows.Controls.Primitives;
using System.ComponentModel; // Shutdown App cancelEventArg

namespace FinalProject
{
    public partial class MainWindow : MetroWindow
    {
        //List of Car objects
        List<Car> carList;

        //Sorted list of refernces to the object in carList
        List<Car> SortedCarList;

        //Used for Copy/Cut/Paste
        Car tempCar;

        //Path storing last open/save path for Saving
        private string path = @"";

        //For viewing Purposes
        private int selectedCar = 0;

        public MainWindow()
        {
            InitializeComponent();

            //Load app with default database file
            readJson();

            //Update view
            SortedCarList = carList;
            updateContext();
        }

        //Every time View needs to be updated, this function is called (Eg. SOrting, Searching, Cutting, adding new car etc.)
        public void updateContext()
        {
            //Check how to assign view. Eg. if you remove object 0, your object 1 will become object 0, so just refresh view       
            if (selectedCar < SortedCarList.Count)            
                this.DataContext = SortedCarList[selectedCar];            
            //if we remove last object in list, decrease counter to prevent error
            else if (0 < selectedCar)
            {
                selectedCar--;
                this.DataContext = SortedCarList[selectedCar];
            }
            //If no object in list hide view
            if (SortedCarList.Count == 0)
            {
                car_holder.Visibility = Visibility.Hidden;
                search_Fail.Visibility = Visibility.Visible;
            }
            //Make sure display is visible if there are elements to display
            else
            {
                car_holder.Visibility = Visibility.Visible;
                search_Fail.Visibility = Visibility.Hidden;

                displayRadio();
                //Update label of no. of cars eg 1/11
                Label_CarNumber.Content = selectedCar + 1 + "/" + SortedCarList.Count;
            }
        }

        private void button_Next_Click(object sender, RoutedEventArgs e)
        {
            //Check whether the object in the list is last
            if (SortedCarList.Count > selectedCar + 1)
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
            carList = carList.OrderBy(o => o.Brand).ToList();
            SortedCarList = carList;
            updateContext();
        }

        private void sortByYear(object sender, RoutedEventArgs e)
        {
            carList = carList.OrderBy(o => o.Year).ToList();
            SortedCarList = carList;
            updateContext();
        }

        private void sortByPrice(object sender, RoutedEventArgs e)
        {
            carList = carList.OrderBy(o => o.Price).ToList();
            SortedCarList = carList;
            updateContext();
        }

        //Adds String from textbox to information list. 
        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            if (TextBox_addInformations.Text != "")
            {
                SortedCarList[selectedCar].Information.Add(TextBox_addInformations.Text);
                //Refresh listbox to reflect the changes
                listBox_Information.Items.Refresh();
                TextBox_addInformations.Text = "";
                carList[selectedCar].EditDate = new DateTime(DateTime.Now.Ticks, DateTimeKind.Utc);
            }
        }

        //Remove selected item from listbox
        private void Clear_Selected_Informations(object sender, RoutedEventArgs e)
        {
            if (listBox_Information.SelectedIndex >= 0)
            {
                SortedCarList[selectedCar].Information.RemoveAt(listBox_Information.SelectedIndex);
                listBox_Information.Items.Refresh();
                carList[selectedCar].EditDate = new DateTime(DateTime.Now.Ticks, DateTimeKind.Utc);
            }
        }

        //Creates new car at the end of a list
        private void menu_newCar_Click(object sender, RoutedEventArgs e)
        {
            SortedCarList = carList;

            SortedCarList.Add(new Car());

            selectedCar = SortedCarList.Count - 1;
            updateContext();
        }

        //For removing object from list
        private void remove()
        {
            if (SortedCarList.Count > 0)
            {
                var carToremove = SortedCarList[selectedCar];
                try
                {
                    carList.Remove(carToremove);
                    SortedCarList.Remove(carToremove);
                }
                catch { }
                updateContext();
            }
        }

        private void menu_Cut_Click(object sender, RoutedEventArgs e)
        {
            //let tempCar be equal to new instance of a car, with an object passed to a constructor
            if (SortedCarList.Count > 0)
            {
                tempCar = new Car(SortedCarList[selectedCar]);
                remove();
            }
        }

        private void menu_Copy_Click(object sender, RoutedEventArgs e)
        {
            if (SortedCarList.Count > 0)
                tempCar = new Car(SortedCarList[selectedCar]);            
        }

        private void menu_Delete_Click(object sender, RoutedEventArgs e)
        {
            remove();
        }

        private void menu_Paste_Click(object sender, RoutedEventArgs e)
        {
            if (tempCar != null)
            {
                SortedCarList = carList;
                if (SortedCarList.Count != 0)
                {
                    SortedCarList.Insert(++selectedCar, tempCar);
                }
                else
                {
                    SortedCarList.Add(tempCar);
                }

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

        //Path to the file defined by user should be passed here, otherwise default location of file 
        public void readJson(string path = "database.json")
        {
            using (StreamReader r = new StreamReader(path))
                try
                {
                    //List of Car objects which holds parsed cars from JSON file. Passed parameter is a json string.
                    //This string is deserialized and loaded into cars list
                    carList = JsonConvert.DeserializeObject<List<Car>>(r.ReadToEnd());
                }
                    //If user opened invalid json file, warning will show up
                catch
                {
                    MessageBoxResult result =
                    MessageBox.Show(
                    "Error loading file",
                    "Car Explorer",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                }
        }

        //converts a List object into JSON string and saves into file
        //Defined as shown. Needs to be completed by ali
        public void writeJson(string path = "database.json")
        {
            using (StreamWriter sw = new StreamWriter(path))
                try
                {
                    sw.Write(JsonConvert.SerializeObject(carList, Formatting.Indented));
                }
                catch { }
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
                readJson(openFileDialog.FileName);
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
                writeJson(saveFileDialog.FileName);
            }
        }

        private void menu_Save_Click(object sender, RoutedEventArgs e)
        {
            if (path != "")
                writeJson(path);
            else
                menu_saveAs_Click(null, null);
        }

        private int theme = 1;
        private void menu_Color_Click(object sender, RoutedEventArgs e)
        {
            // < !--BaseLight, Blue, crimson-- >
            string accentColor = "Blue";
            string baseColor = "BaseDark";

            switch (theme)
            {
                case 0:
                    accentColor = "Blue";
                    baseColor = "BaseDark";
                    break;
                case 1:
                    accentColor = "Blue";
                    baseColor = "BaseLight";
                    break;
                case 2:
                    accentColor = "Crimson";
                    baseColor = "BaseDark";
                    break;
                case 3:
                    accentColor = "Crimson";
                    baseColor = "BaseLight";
                    theme = -1;
                    break;
                default:
                    accentColor = "Blue";
                    baseColor = "BaseDark";
                    break;
            };
            theme++;
            ThemeManager.ChangeAppStyle(Application.Current,
                                        ThemeManager.GetAccent(accentColor),
                                        ThemeManager.GetAppTheme(baseColor));
        }

        private void menu_Help_Click(object sender, RoutedEventArgs e)
        {
            new Help().Show();
        }

        private void menu_About_Click(object sender, RoutedEventArgs e)
        {
            new About().ShowDialog();
        }

        private void menu_Search_Click(object sender, RoutedEventArgs e)
        {
            //Try to convert searchbox text into yeear
            try
            {
                selectedCar = 0;
                SortedCarList = carList.FindAll(p => carList.All(tag => p.Year.Equals(Convert.ToInt16(textBox_Search.Text))));
            }
            //If thre is a problem look for Name containing the searchbox text
            catch
            {
                selectedCar = 0;
                SortedCarList = carList.FindAll(p => carList.All(tag => p.Brand.Contains(textBox_Search.Text)));
            }
            updateContext();
        }

        private void textBox_Search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                menu_Search_Click(null, null);
        }

        private void MetroWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
                button_Next_Click(null, null);
            if (e.Key == Key.Left)
                button_Previous_Click(null, null);
        }

        //Change image or add new one
        private void img01_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = "jpg";
            openFileDialog.Filter = "Jpg file (*.jpg)|*.jpg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    SortedCarList[selectedCar].ImgPath = openFileDialog.FileName;
                    //Clear context, and reassign it again to show new picture
                    DataContext = "";
                    DataContext = SortedCarList[selectedCar];
                }
                catch { }
            }
        }

        void Window_Closing(object sender, CancelEventArgs e)
        {
            string msg = "Are you sure you wish to exit?";
            MessageBoxResult result =
              MessageBox.Show(
                msg,
                "Car Explorer",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);
            if (result == MessageBoxResult.No)
            {
                // If user doesn't want to close, cancel closure
                e.Cancel = true;
            }
        }

        private void menu_exit(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}