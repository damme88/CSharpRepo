using System.ComponentModel;
using TDrawing.ViewModels;
using Xamarin.Forms;

namespace TDrawing.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}