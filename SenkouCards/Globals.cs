using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace SenkouCards
{
    public class Globals
    {

        public static int userId = 2;

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

    }
}
