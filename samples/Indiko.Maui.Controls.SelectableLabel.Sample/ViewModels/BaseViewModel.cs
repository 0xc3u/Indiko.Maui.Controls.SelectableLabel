using CommunityToolkit.Mvvm.ComponentModel;
using Indiko.Maui.Controls.SelectableLabel.Sample.Interfaces;

namespace Indiko.Maui.Controls.SelectableLabel.Sample.ViewModels;

public partial class BaseViewModel : ObservableObject, IViewModel
{
	[ObservableProperty]
	bool isBusy;

	public virtual void OnAppearing(object param) { }

	public virtual Task RefreshAsync()
	{
		return Task.CompletedTask;
	}
}
