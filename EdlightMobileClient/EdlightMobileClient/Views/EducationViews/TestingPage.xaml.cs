using Xamarin.Forms;

namespace EdlightMobileClient.Views.EducationViews
{
    public partial class TestingPage : CarouselPage
    {
        public TestingPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, System.EventArgs e) => this.SelectedItem = 0;
    }
}
