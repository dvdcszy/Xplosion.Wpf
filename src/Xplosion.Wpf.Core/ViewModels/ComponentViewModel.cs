using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Xplosion.Core.FrameworkLibrary;

namespace Xplosion.Wpf.Core
{
    public class ComponentViewModel : BaseViewModel
    {

        #region Private services
        /// <summary>
        /// Component creator service invoked on the ML formula itself
        /// </summary>
        private IComponentBuilder _componentBuilder;
        #endregion

        #region Public members

        public ICommand ChangeComponentsViewCommand { get; set; }

        public ICommand HideSideMenuCommand { get; set; }
        #region Private memebers

        private ObservableCollection<XplodedFormulaChildModel> _componentCollection;

        private string _currentFormula;

        private string _formulaDescription;

        private double _formulaValue;

        private double _formulaCost;

        //Visible by default
        private Visibility _sideMenuVisibility = Visibility.Visible;
        #endregion

        public Visibility SideMenuVisibility
        {
            get => _sideMenuVisibility;
            set 
            {
                _sideMenuVisibility = value;
                OnPropertyChanged(nameof(SideMenuVisibility));
            }
        }


        /// <summary>
        /// Collection of <see cref="XplodedFormulaChildModel"/> available for UI list bindings
        /// </summary>
        public ObservableCollection<XplodedFormulaChildModel> ComponentCollection
        {
            get => _componentCollection;
            set
            {

                //if (_componentCollection != null)
                //    _componentCollection.Clear();
                _componentCollection = value;
                OnPropertyChanged(nameof(ComponentCollection));
            }
        }

        /// <summary>
        /// Collection of <see cref="XplodedFormulaParentModel"/> available for UI list bindings
        /// </summary>
        public ObservableCollection<XplodedFormulaParentModel> PartsList { get; set; }

        /// <summary>
        /// Current Formula in use
        /// </summary>
        public string CurrentFormula 
        {
            get => _currentFormula;
            set
            {
                _currentFormula = value;
                OnPropertyChanged(nameof(CurrentFormula));
            }
        }


        /// <summary>
        /// Description of the Formula
        /// </summary>
        public string FormulaDescription
        {
            get => _formulaDescription;
            set
            {
                _formulaDescription = value;
                OnPropertyChanged(nameof(FormulaDescription));
            }
        }

        /// <summary>
        /// Hrs value of the Formula (final result)
        /// </summary>


        public double FormulaValue
        {
            get => _formulaValue; 
            
            set 
            { 
                _formulaValue = value;
                OnPropertyChanged(nameof(FormulaValue));
            }
        }

        
        /// <summary>
        /// $ value of the Formula (final result) 
        /// </summary>
        /// TO DO: Add appropriate labor rates
        public double FormulaCost 
        {
            get => _formulaCost;
            set
            {
               _formulaCost =  value * 28.2;
                OnPropertyChanged(nameof(FormulaCost));
            }
        }

        private double _sumOfComponenets;

        public double SumOfComponenets
        {
            get => _sumOfComponenets;
            set 
            { 
                _sumOfComponenets = value;
                OnPropertyChanged(nameof(SumOfComponenets));
            }
        }


        #endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="_compBuilder">Component building service DI</param>
        public ComponentViewModel(IComponentBuilder _compBuilder)
        {
            _componentBuilder = _compBuilder;

            ChangeComponentsViewCommand = new RelayParameterizedCommand((parameter) => ChangeComponents(parameter));

            HideSideMenuCommand = new RelayCommand(HideSideMenu);

            PartsList = new ObservableCollection<XplodedFormulaParentModel>(_componentBuilder.RunXplosion());

        }




        #region Command helpers
        private void ChangeComponents(object param)
        {
            if (param is XplodedFormulaParentModel)
            {
                XplodedFormulaParentModel item = (XplodedFormulaParentModel)param;

                //if (ComponentCollection != null)
                //    ComponentCollection.Clear();
                ComponentCollection = new ObservableCollection<XplodedFormulaChildModel>(item.ChildrenCollection);

                CurrentFormula = item.FormulaCode;

                FormulaDescription = item.FormulaDescription;

                FormulaValue = item.FormulaValue;

                FormulaCost = item.FormulaValue;

                SumOfComponenets = item.ChildrenCollection.Sum(x => x.OperationResult);
            }
                
        }

        private void HideSideMenu()
        {
            SideMenuVisibility = Visibility.Collapsed;
        }
        #endregion
    }
}
