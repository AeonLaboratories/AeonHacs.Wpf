using HACS.Core;
using HACS.WPF.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace HACS.WPF.ViewModels
{
	[TypeConverter(typeof(ViewModelConverter))]
	public class ViewModel : BindableObject, INamedObject
	{
		public static Dictionary<string, Type> ViewModelTypes;
		static ViewModel()
		{
			ViewModelTypes = AppDomain.CurrentDomain.GetAssemblies().Where(
				assembly => !assembly.IsDynamic).SelectMany(
				assembly => assembly.GetExportedTypes()).Where(
				type => typeof(ViewModel).IsAssignableFrom(type)).ToDictionary(
				type => type.Name, type => type);
		}

		public static Dictionary<string, ViewModel> Collection = new Dictionary<string, ViewModel>();


		/// <summary>
		/// Finds the ViewModel with the specified key, creating one
		/// if needed, based on the model (NamedObject) having the
		/// name implied by the key.
		/// The key can be
		///		an existing ViewModel's Key,
		///		a NamedObject's Name (e.g., "pVacuumSystem"), or
		///		a NamedObject's Name prepended with its type and a dot
		///			e.g., "Section.VTT"
		///	In the first case, the existing ViewModel is returned;
		///	otherwise, a new ViewModel of the type corresponding to the given 
		///	NamedObject's type is created and returned.
		///	If anything goes wrong (e.g., the key is invalid or the NamedObject
		///	doesn't exist), null is returned.
		/// </summary>
		/// <param name="viewModelName"></param>
		/// <returns></returns>
		public static ViewModel GetFromKey(string key)
		{
			if (key.IsBlank()) return null;

			if (Collection.TryGetValue(key, out ViewModel existingViewModel))
				return existingViewModel;

			INamedObject model = Core.NamedObject.Find<INamedObject>(key);

			if (model == null)
			{
				var dot = key.IndexOf('.');
				if (dot < 1 || dot > key.Length - 2) return null;
				var modelTypeName = key.Substring(0, dot);
				var modelName = key[(dot + 1)..];

				var list = Core.NamedObject.FindAll(modelName);
				var type =
					Type.GetType("HACS.Components." + modelTypeName + ", HACS") ??
					Type.GetType("HACS.Core." + modelTypeName + ", HACS");
				if (type != null)
					model = 
						list.FirstOrDefault(x => x.GetType() == type) ??
						list.FirstOrDefault(x => type.IsAssignableFrom(x.GetType()));
			}
			return NewViewModel(model);
		}

		/// <summary>
		/// Finds the ViewModel for the specified INotifyPropertyChanged, creating one
		/// if needed. Returns null if this isn't possible.
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public static ViewModel GetFromModel(INotifyPropertyChanged model)
		{
			if (model == null) return null;
			if (model is ViewModel viewModel) return viewModel;

			// Every unnamed model produces a new unnamed ViewModel,
			// which doesn't get added to the collection, so it can't be 
			// found by name, but it can still be accessed via its owner.
			if (!(model is INamedObject namedObject) || namedObject.Name.IsBlank())
				return NewViewModel(model);

			return GetFromKey(GenerateKey(namedObject));
		}

		/// <summary>
		/// Note: The generated key depends on whether the NamedObject dictionary contains
		/// multiple objects with the same name as the model.
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public static string GenerateKey(INamedObject model)
		{
			if (model.Name.IsBlank())
				return null;

			var list = Core.NamedObject.FindAll(model.Name);

			if (list.Count == 0) return null;
			if (list.Count == 1 && list[0] == model)
				return model.Name;

			return model.GetType().Name + "." + model.Name;
		}

		/// <summary>
		/// Returns the ViewModel type that corresponds to the 
		/// specified model. Presently, that means the Name property
		/// of the ViewModel's type matches the model's type Name (which 
		/// requires the two types to be defined in different 
		/// namespaces).
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public static Type ViewModelType(INotifyPropertyChanged model)
		{
			var typeName = model.GetType().Name;
			if (ViewModelTypes.TryGetValue(typeName, out Type viewModelType))
				return viewModelType;
			return null;
		}

		public static ViewModel NewViewModel(INotifyPropertyChanged model)
		{
			if (model == null) return null;
			var viewModelType = ViewModelType(model);
			if (viewModelType == null)
				return null;
			var viewModel = (ViewModel)Activator.CreateInstance(viewModelType);
			viewModel.Component = model;
			return viewModel;
		}

		/// <summary>
		/// This method only looks and returns null if the model isn't found.
		/// Use GetFromModel() if automatic creation is needed.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="model"></param>
		/// <returns></returns>
		public static ViewModel FindFromModel(INamedObject model)
		{
			if (model == null) return null;
			if (model.Name.IsBlank()) return null;
			return Collection.TryGetValue(GenerateKey(model), out ViewModel viewModel) ? viewModel : null;
		}

		public static Window SettingsWindow { get; set; }

		[Browsable(false)]
		public INotifyPropertyChanged Component
		{
			get => component;
			protected set
			{
				if (value == null) throw new NullReferenceException();
				if (component != null)
				{
					component.PropertyChanged -= OnPropertyChanged;
					Key = null;
				}
				component = value;
				if (component is INamedObject model)
				{
					Name = model.Name;
					Key = GenerateKey(model);
				}
				StartContext();
				component.PropertyChanged += OnPropertyChanged;
				ComponentChanged();
			}
		}
		INotifyPropertyChanged component;

		[Browsable(false)]
		public virtual string Name { get; set; }

		[Browsable(false)]
		public virtual string Key
		{
			get => key;
			set
			{
				if (key == value) return;
				if (key != null && Collection.ContainsKey(key))
					Collection.Remove(key);
				key = value;
				if (key != null)
					Collection[key] = this;
			}
		}
		string key;
		
		protected virtual void ComponentChanged() { }

		/// <summary>
		/// The default list of Context Menu items.
		/// </summary>
		protected List<Context> ContextStart { get; set; } = new List<Context>();

		/// <summary>
		/// Builds the initial/default list of Context menu items.
		/// Override this property to construct a Context menu that
		/// remains constant for all instances and states of the ViewModel.
		/// </summary>
		protected virtual void StartContext() { }

		/// <summary>
		/// The list of Context menu items presented when the Context menu opens.
		/// Override this property to dynamically generate the Context menu
		/// upon opening.
		/// </summary>
		/// <returns></returns>
		public virtual List<Context> Context() => ContextStart;

		public virtual void Run(string command = "") { }

		/// <summary>
		/// True if Run("") does something.
		/// </summary>
		[Browsable(false)]
		public bool RunHasDefault { get; set; } = false;

		public void Dispatch(string command = "") =>
			Task.Run(() => Run(command));

		protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e) =>
			NotifyPropertyChanged(this, e);

		public override string ToString() =>
			Component?.ToString() ?? "<None>";
	}
}
