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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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
         * A function that sorts the current view by clicking on clumn header
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

        public static void Sort(string sortBy, ListSortDirection direction, ListView LvFOO)
        {
            ICollectionView dataView = CollectionViewSource.GetDefaultView(LvFOO.ItemsSource);

            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }

        public sealed class CardsMap : ClassMap<cards>
        {
            public CardsMap()
            {
                AutoMap(CultureInfo.InvariantCulture);
                Map(m => m.decks).Ignore();
                Map(m => m.responses).Ignore();
            }
        }

        public static string CreateFileHeaderFromProperties<T>(List<string> excluded)
        {
            var headerList = typeof(T).GetProperties()
                .Where(x => (!excluded.Contains(x.Name)))
                .Select(x => x.Name).ToList();
            string headerString = String.Join(",", headerList);
            return headerString;
        }

        public static string CreateStringFromProperties<T>(T source, List<string> excluded)
        {
            var propertiesList = typeof(T).GetProperties()
                .Where(x => (!excluded.Contains(x.Name)))
                .Select(x => x.GetValue(source) ?? "").ToList();
            string propertyString = String.Join(",", propertiesList);
            return propertyString;
        }

        public static void SaveToCSV<T>(List<string> excluded, List<T> elementsList)
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
            if (saveFileDialog.ShowDialog() == true)
            {
                List<string> lines = new List<string>();
                lines.Add(Globals.CreateFileHeaderFromProperties<T>(excluded));
                foreach (T element in elementsList)
                {
                    string newline = Globals.CreateStringFromProperties<T>(element, excluded);
                    //Console.WriteLine(newline);
                    lines.Add(newline);
                }
                File.WriteAllLines(saveFileDialog.FileName, lines); // ex IO/Sys
            }
        }
    }
}
