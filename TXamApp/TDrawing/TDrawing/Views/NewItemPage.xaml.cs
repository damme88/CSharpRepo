using System;
using System.Collections.Generic;
using System.ComponentModel;
using TDrawing.Models;
using TDrawing.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TDrawing.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }
    }
}