﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using ModernWpf.Controls;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ItemsRepeaterTestApp.Samples.Selection
{
    public sealed partial class FlatSample : Page
    {
        private SelectionModel selectionModel;
        private RecyclingElementFactory elementFactory;

        ObservableCollection<string> _data = new ObservableCollection<string>(Enumerable.Range(0, 1000).Select(x => x.ToString()));
        public FlatSample()
        {
            this.InitializeComponent();
            selectionModel = (SelectionModel)Resources[nameof(selectionModel)];
            elementFactory = (RecyclingElementFactory)Resources[nameof(elementFactory)];
            repeater.ItemTemplate = elementFactory;
            repeater.ItemsSource = _data;
            selectionModel.Source = _data;
            repeater.ElementPrepared += Repeater_ElementPrepared;
            repeater.ElementIndexChanged += Repeater_ElementIndexChanged;
            selectionModel.PropertyChanged += SelectionModel_PropertyChanged;
        }

        private void SelectionModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var manager = sender as SelectionModel;
            if (e.PropertyName == "SelectedItem")
            {
                Debug.WriteLine("SelectedItem changed");
                Debug.WriteLine(manager.SelectedItem);
            }
            else if (e.PropertyName == "SelectedIndices")
            {
                Debug.WriteLine("SelectedIndices changed");
                for (int i = 0; i < manager.SelectedIndices.Count; i++)
                {
                    Debug.WriteLine(string.Format("{0}:{1}", manager.SelectedIndices[i], manager.SelectedItems[i].ToString()));
                }
            }
            else if (e.PropertyName == "SelectedItems")
            {
                Debug.WriteLine("SelectedItems changed");
                for (int i = 0; i < manager.SelectedIndices.Count; i++)
                {
                    Debug.WriteLine(string.Format("{0}:{1}", manager.SelectedIndices[i], manager.SelectedItems[i].ToString()));
                }
            }
            else if (e.PropertyName == "SelectedIndex")
            {
                Debug.WriteLine("SelectedIndex changed");
                Debug.WriteLine(manager.SelectedIndex);
            }

            //var icpp = (ICustomPropertyProvider)selectionModel;
            //var selectedItemProperty = icpp.GetCustomProperty("SelectedItem");
            //var canRead = selectedItemProperty.CanRead;
            //var selectedItem =  selectedItemProperty.GetValue(selectionModel);
        }

        private void Repeater_ElementIndexChanged(ItemsRepeater sender, ItemsRepeaterElementIndexChangedEventArgs args)
        {
            (args.Element as RepeaterItem).RepeatedIndex = args.NewIndex;
        }

        private void Repeater_ElementPrepared(ItemsRepeater sender, ItemsRepeaterElementPreparedEventArgs args)
        {
            (args.Element as RepeaterItem).RepeatedIndex = args.Index;
        }

        private void OnMultipleSelectionClicked(object sender, RoutedEventArgs e)
        {
            selectionModel.SingleSelect = multipleSelection.IsChecked.Value ? false : true;
        }

        private void OnGroupedClicked(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new GroupedSample());
        }

        private void OnBackClicked(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void insert_Click(object sender, RoutedEventArgs e)
        {
            var index = int.Parse(indexPath.Text);
            _data.Insert(index, "Insert:" + index.ToString());
        }

        private void remove_Click(object sender, RoutedEventArgs e)
        {
            var index = int.Parse(indexPath.Text);
            _data.RemoveAt(index);
        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            _data.Move(0, 2);
            _data.Clear();
        }
    }
}
