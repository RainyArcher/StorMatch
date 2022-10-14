using StorMatch.Views;

namespace StorMatch.Pages;

public partial class ConditionsPage : ContentPage
{
	public ConditionsPage(ConditionsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}