using Nulah.Shortcuts.Models.Interfaces;

namespace Nulah.Shortcuts.ViewModels.RecycleBin;

public interface IRecycleBinViewModel : IViewModelInterface
{
}

public class RecycleBinViewModel: ViewModelBase<IRecycleBinViewModel>, IRecycleBinViewModel
{
	
}