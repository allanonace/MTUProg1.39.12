using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aclara_meters.view
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class viewBottomBar : Grid
    {
        public viewBottomBar()
        {
            InitializeComponent();
        }
        public TapGestureRecognizer GetTGRElement(string buttonName)
        {
            TapGestureRecognizer element = (TapGestureRecognizer)this.FindByName(buttonName);

            Console.WriteLine ( "----------> " + buttonName + ": " + ( element == null ) );

            return element;
        }
        public Label GetLabelElement(string labelName)
        {
            Label element = (Label)this.FindByName(labelName);
            return element;
        }
        public Image GetImageElement(string imageName)
        {
            Image element = (Image)this.FindByName(imageName);
            return element;
        }
        public ImageButton GetImageButtonElement(string imageName)
        {
            ImageButton element = (ImageButton)this.FindByName(imageName);

            Console.WriteLine ( "----------> " + imageName + ": " + ( element == null ) );
            element.BackgroundColor = Color.Red;

            return element;
        }

        public StackLayout GetLabelStack ( string stackName )
        {
            return (StackLayout)this.FindByName(stackName);
        }
    }
}