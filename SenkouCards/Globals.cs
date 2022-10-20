using CsvHelper.Configuration;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace SenkouCards
{
    public class Globals
    {

        //Singleton DB Context
        private static SenkoucardsConfig _SenkouDbContextInternal;


        public static SenkoucardsConfig SenkouDbAuto
        {
            get
            {
                try
                {
                    if (_SenkouDbContextInternal == null)
                    {
                        _SenkouDbContextInternal = new SenkoucardsConfig();
                        return _SenkouDbContextInternal;
                    }
                }
                catch (SqlException ex)
                {
                    Debug.WriteLine("Database conneciton error" + ex.Message);
                }
                return _SenkouDbContextInternal;
            }
        }


        private static users _activeUser;

        public static users ActiveUser
        {
            set
            {
                if (_activeUser == null)
                {
                    _activeUser = value;
                }
            }

            get
            {
                return _activeUser;
            }
        }


        /**
        * Overloaded function that dynamically changes column names based on class properties
        * */
        public static void AddListViewColumns<T>(GridView GvFOO)
        {
            foreach (System.Reflection.PropertyInfo property in typeof(T).GetProperties().Where(p => p.CanWrite)) //loop through the fields of the object
            {
                if (property.Name != "Id" && property.Name != "id") //if you don't want to add the id in the list view
                {
                    GridViewColumn gvc = new GridViewColumn(); //initialize the new column
                    gvc.DisplayMemberBinding = new Binding(property.Name); // bind the column to the field
                    if (property.PropertyType == typeof(DateTime)) { gvc.DisplayMemberBinding.StringFormat = "yyyy-MM-dd"; } //[optional] if you want to display dates only for DateTime data
                    gvc.Header = property.Name; //set header name like the field name
                    GvFOO.Columns.Add(gvc); //add new column to the Gridview
                }
            }
        }

        /**
         * Overloaded function to dynamically change column names based on class properties
         * with an excluded list to exclude from headers
         * */
        public static void AddListViewColumns<T>(GridView GvFOO, List<string> excluded)
        {
            foreach (System.Reflection.PropertyInfo property in typeof(T).GetProperties().Where(p => p.CanWrite)) //loop through the fields of the object
            {
                if (property.Name != "Id" && property.Name != "id" && !excluded.Contains(property.Name)) //if you don't want to add the id in the list view
                {
                    GridViewColumn gvc = new GridViewColumn(); //initialize the new column
                    gvc.DisplayMemberBinding = new Binding(property.Name); // bind the column to the field
                    if (property.PropertyType == typeof(DateTime)) { gvc.DisplayMemberBinding.StringFormat = "yyyy-MM-dd"; } //[optional] if you want to display dates only for DateTime data
                    gvc.Header = property.Name; //set header name like the field name
                    GvFOO.Columns.Add(gvc); //add new column to the Gridview
                }

            }
        }

        /**
         * A function that sorts the current view by clicking on column header
         * */
        public static void LvHeader_Click(object sender, RoutedEventArgs e, GridViewColumnHeader _lastHeaderClicked, ListSortDirection _lastDirection, ListView LvFOO)
        {
            var headerClicked = e.OriginalSource as GridViewColumnHeader; //get the clicked header by the RoutedEventArgs
            ListSortDirection direction;

            if (headerClicked != null && headerClicked.Role != GridViewColumnHeaderRole.Padding) //check if header is clicked
            {
                if (headerClicked != _lastHeaderClicked) //set direction to ascending if new column clicked
                {
                    direction = ListSortDirection.Ascending;
                }
                else
                {
                    if (_lastDirection == ListSortDirection.Ascending) // else set direction to opposite from previous click if smae column clicked
                    {
                        direction = ListSortDirection.Descending;
                    }
                    else
                    {
                        direction = ListSortDirection.Ascending;
                    }
                }
                var columnBinding = headerClicked.Column.DisplayMemberBinding as Binding; // get the binding of the clicked column
                var sortBy = columnBinding?.Path.Path ?? headerClicked.Column.Header as string; // get the first not null value of binding path or column header
                Sort(sortBy, direction, LvFOO); //sort 

                _lastHeaderClicked = headerClicked;
                _lastDirection = direction;
            }
        }

        /**
         * A function that sorts a Listview (LvFOO) by a specific direction based on a specific column (string)
         * */
        public static void Sort(string sortBy, ListSortDirection direction, ListView LvFOO)
        {
            ICollectionView dataView = CollectionViewSource.GetDefaultView(LvFOO.ItemsSource);

            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }

        /**
         * Mapping class to CSV columns
         * */
        public sealed class CardsMap : ClassMap<cards>
        {
            public CardsMap()
            {
                AutoMap(CultureInfo.InvariantCulture);
                Map(m => m.decks).Ignore();
                Map(m => m.responses).Ignore();
            }
        }

        /**
         * Creates a file header with a specific delimiter for any class type, and excluding given list of properties
         * */
        public static string CreateFileHeaderFromProperties<T>(List<string> excluded, string delimiter = ",")
        {
            var headerList = typeof(T).GetProperties()
                .Where(x => (!excluded.Contains(x.Name)))
                .Select(x => x.Name).ToList();
            string headerString = String.Join(delimiter, headerList);
            return headerString;
        }

        /**
         * Creates a delimiter-separated string of propertiers from any object, and excluding given list of properties
         * */
        public static string CreateStringFromProperties<T>(T source, List<string> excluded, string delimiter = ",")
        {
            var propertiesList = typeof(T).GetProperties()
                .Where(x => (!excluded.Contains(x.Name)))
                .Select(x => x.GetValue(source) ?? "").ToList();
            string propertyString = String.Join(delimiter, propertiesList);
            return propertyString;
        }

        /**
         * Writes a list of objects into a file
         * Takes a list of elements (objects) to be written and a list of properties to be excluded from the writing
         * 
         * */
        public static void SaveToCSV<T>(List<string> excluded, List<T> elementsList)
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
            if (saveFileDialog.ShowDialog() == true)
            {
                List<string> lines = new List<string>();
                lines.Add(Globals.CreateFileHeaderFromProperties<T>(excluded, ";"));
                foreach (T element in elementsList)
                {
                    string newline = Globals.CreateStringFromProperties<T>(element, excluded, ";");
                    //Console.WriteLine(newline);
                    lines.Add(newline);
                }
                File.WriteAllLines(saveFileDialog.FileName, lines); // ex IO/Sys
            }
        }

        public static string convertedRtf(string toConvert)
        {

            FlowDocument document = new FlowDocument();

            TextRange textRange = new TextRange(document.ContentStart, document.ContentEnd);
            using (MemoryStream sm = new MemoryStream(Encoding.ASCII.GetBytes(toConvert)))
            {
                textRange.Load(sm, DataFormats.Rtf);
            }
            return textRange.Text;
        }
        /*public static byte[] convertedImage(string toConvert)
        {

        }*/

    }
}
