using BookShop.CRM.ViewModels.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace BookShop.CRM.Wrappers.Base
{
    public abstract class ModelWrapper<TModel> : BaseViewModel, INotifyDataErrorInfo
    {
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public TModel Model { get; }

        public bool HasErrors => Errors.Any();

        public ModelWrapper(TModel model, bool validate_properties_on_start = true)
        {
            Model = model;
            if (!validate_properties_on_start)
                return;
            ValidateAll();
        }

        public void ValidateAll()
        {
            foreach (PropertyInfo property in typeof(TModel).GetProperties())
            {
                PropertyValidation(property.Name);
            }
        }

        protected virtual Dictionary<string, List<string>> Errors { get; set; } = new();

        protected virtual void OnErrorChanged([CallerMemberName] string propertyName = default)
        {
            ErrorsChanged?.Invoke(this, new(propertyName));
            OnPropertyChanged(nameof(HasErrors));
        }

        protected virtual TValue GetValue<TValue>([CallerMemberName] string propertyName = default)
        {
            return (TValue)typeof(TModel).GetProperty(propertyName).GetValue(Model);
        }

        protected virtual void SetValue<TValue>(TValue value, [CallerMemberName] string propertyName = default)
        {
            typeof(TModel).GetProperty(propertyName).SetValue(Model, value);
            OnPropertyChanged(propertyName);
            PropertyValidation(propertyName);
        }

        private void PropertyValidation(string propertyName)
        {
            ClearErrors(propertyName);
            IEnumerable<string>  errors = ValidateProperties(propertyName);
            if(errors != null)
            {
                AddErrors(propertyName, errors.ToArray());
            }
        }

        protected abstract IEnumerable<string> ValidateProperties(string propertyName);

        protected virtual void AddError(string propertyName, string error)
        {
            if (!Errors.ContainsKey(propertyName))
            {
                Errors.Add(propertyName, new());
            }
            if (!Errors[propertyName].Contains(error))
            {
                Errors[propertyName].Add(error);
                OnErrorChanged(propertyName);
            }
        }

        protected virtual void AddErrors(string propertyName, params string[] errors)
        {
            foreach (string error in errors)
            {
                AddError(propertyName, error);
            }
        }

        protected virtual void ClearErrors(string propertyName)
        {
            if (Errors.ContainsKey(propertyName))
            {
                Errors.Remove(propertyName);
                OnErrorChanged(propertyName);
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (Errors.ContainsKey(propertyName))
                return Errors[propertyName];
            return null;
        }
    }
}
