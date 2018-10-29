using BGayet.GIA.Models;
using BGayet.GIA.ViewModels;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace BGayet.GIA.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvInscrits.ItemsSource);
            PropertyGroupDescription groupDescription1 = new PropertyGroupDescription("NumGroupeArbitre");
            PropertyGroupDescription groupDescription2 = new PropertyGroupDescription("Classement");
            view.GroupDescriptions.Add(groupDescription1);
            view.GroupDescriptions.Add(groupDescription2);

            Closing += (s, e) => ViewModelLocator.Cleanup();
        }
    }
}