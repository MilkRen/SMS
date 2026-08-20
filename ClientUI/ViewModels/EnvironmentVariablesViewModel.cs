using WpfApp.Services.Interfaces;

namespace WpfApp.ViewModels
{
    /// <summary>
    /// VM для переменных окружений
    /// </summary>
    public class EnvironmentVariablesViewModel
    {
        #region Services

        private readonly IJsonConfigurationServices _jsonConfigurationServices;

        #endregion

        #region Ctor

        public EnvironmentVariablesViewModel(IJsonConfigurationServices jsonConfigurationServices)
        {
            _jsonConfigurationServices = jsonConfigurationServices;
        }

        #endregion

        #region Binding

        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set => _name = value;
        }


        private string _value = string.Empty;
        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                _jsonConfigurationServices.SaveEnvironmentVariable(this);
            }
        }

        private string _comment = string.Empty;
        public string Comment
        {
            get => _comment;
            set
            {
                _comment = value;
                _jsonConfigurationServices.SaveComments(this);
            }
        }

        #endregion
    }
}
