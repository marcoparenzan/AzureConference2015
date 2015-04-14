using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

using Call4Pizza.CashRegister.ViewModels.Base;

namespace Call4Pizza.CashRegister.ViewModels
{
	#region Class CashRegisterViewModel

	public partial class CashRegisterViewModel: BaseViewModel
    {
			
			#region Local Command AddPizza
			
			public partial class AddPizzaArgs: BaseArgs
			{
			}
			
			public AddPizzaArgs AddPizzaCommandArgs
			{
				get
				{
					AddPizzaArgs args = new AddPizzaArgs {
					};
					return args;
				}
			}

			partial void OnAddPizza(AddPizzaArgs args);

			partial void OnAddPizzaEnable(Action<bool> enableHandler);

			private RelayCommand<AddPizzaArgs> _addPizza; // ICommand

			public RelayCommand<AddPizzaArgs> AddPizza // ICommand
			{
				get
				{
					if (_addPizza == null)
					{
						_addPizza = new RelayCommand<AddPizzaArgs>(new Action<AddPizzaArgs>(
							_ =>
							{
								try
								{
									OnAddPizza(_);
								}
								catch(Exception ex)
								{
									OnException(ex);
								}
							}
						),
						_ => {
							bool enabled = true;
							OnAddPizzaEnable(enable => {
								enabled = enable;
							});
							return enabled;
						});
					}
					return _addPizza;
				}
			}
			
			#endregion		
				
			#region Local Command AddIngredient
			
			public partial class AddIngredientArgs: BaseArgs
			{
			}
			
			public AddIngredientArgs AddIngredientCommandArgs
			{
				get
				{
					AddIngredientArgs args = new AddIngredientArgs {
					};
					return args;
				}
			}

			partial void OnAddIngredient(AddIngredientArgs args);

			partial void OnAddIngredientEnable(Action<bool> enableHandler);

			private RelayCommand<AddIngredientArgs> _addIngredient; // ICommand

			public RelayCommand<AddIngredientArgs> AddIngredient // ICommand
			{
				get
				{
					if (_addIngredient == null)
					{
						_addIngredient = new RelayCommand<AddIngredientArgs>(new Action<AddIngredientArgs>(
							_ =>
							{
								try
								{
									OnAddIngredient(_);
								}
								catch(Exception ex)
								{
									OnException(ex);
								}
							}
						),
						_ => {
							bool enabled = true;
							OnAddIngredientEnable(enable => {
								enabled = enable;
							});
							return enabled;
						});
					}
					return _addIngredient;
				}
			}
			
			#endregion		
				
			#region Local Command SendOrder
			
			public partial class CreateOrderArgs: BaseArgs
			{
			}
			
			public CreateOrderArgs SendOrderCommandArgs
			{
				get
				{
					CreateOrderArgs args = new CreateOrderArgs {
					};
					return args;
				}
			}

			partial void OnCreateOrder(CreateOrderArgs args);

			partial void OnSendOrderEnable(Action<bool> enableHandler);

			private RelayCommand<CreateOrderArgs> _sendOrder; // ICommand

			public RelayCommand<CreateOrderArgs> SendOrder // ICommand
			{
				get
				{
					if (_sendOrder == null)
					{
						_sendOrder = new RelayCommand<CreateOrderArgs>(new Action<CreateOrderArgs>(
							_ =>
							{
								try
								{
									OnCreateOrder(_);
								}
								catch(Exception ex)
								{
									OnException(ex);
								}
							}
						),
						_ => {
							bool enabled = true;
							OnSendOrderEnable(enable => {
								enabled = enable;
							});
							return enabled;
						});
					}
					return _sendOrder;
				}
			}
			
			#endregion		
			
		partial void OnException(Exception ex);

					#region Total Property
			
			private decimal _total;
			
			partial void OnSetTotal(decimal lastTotal, Action<decimal> handleSet);
			
			private void SetTotal(decimal alternateValue)
			{
				_total = alternateValue;
			}

			public decimal Total
			{
				get
				{
					return _total;
				}
				
				set
				{
					if (value == _total) return;
					var lastTotal = _total;
					_total = value;
					OnSetTotal(
						lastTotal
						, new Action<decimal>(SetTotal));
					Notify("Total");
				}
			}
			
			#endregion
				}
	
	#endregion
}
