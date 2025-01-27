using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using WpfApp1.Command;
using WpfApp1.Model;

namespace WpfApp1.ViewModel
{
    /// <summary>
    /// Represents the ViewModel for managing Person entities in a WPF MVVM application.
    /// Implements INotifyPropertyChanged to support data binding and property change notifications.
    /// </summary>
    public class PersonViewModel : INotifyPropertyChanged
    {
        private Person _person;

        /// <summary>
        /// Gets or sets the current person object
        /// </summary>
        public Person Person
        {
            get { return _person; }
            set
            {
                _person = value;
                NotifyPropertyChanged( "Person" );

                // Notify commands to re-evaluate their CanExecute logic
                CommandManager.InvalidateRequerySuggested( );
                SelectedPerson = null;
            }
        }

        private ObservableCollection< Person > _persons;

        /// <summary>
        /// Gets or sets the collection of Person objects.
        /// </summary>
        public ObservableCollection< Person > Persons
        {
            get { return _persons; }
            set
            {
                _persons = value;
                NotifyPropertyChanged( "Persons" );
            }
        }

        /// <summary>
        /// Initializes a new instance of the PersonViewModel class.
        /// </summary>
        public PersonViewModel()
        {
            Person = new Person();
            Persons = new ObservableCollection< Person >();
            Persons.Add( new Person { Name = "Walter White", Age = 42 } );
            Persons.Add( new Person { Name = "George II", Age = 68 } );
            Persons.Add( new Person { Name = "Isaac Newton", Age = 39 } );
        }

        private ICommand _addPersonCommand;

        /// <summary>
        /// Command for adding a new Person to the Persons collection.
        /// </summary>
        public ICommand AddPersonCommand
        {
            get
            {
                if ( _addPersonCommand == null )
                {
                    _addPersonCommand = new RelayCommand( AddExecute, CanAddExecute, true );
                }

                return _addPersonCommand;
            }
        }

        /// <summary>
        /// Adds the current Person to the Persons collection if it does not already exist.
        /// </summary>
        /// <param name="parameter">Optional command parameter (unused).</param>
        private void AddExecute( object parameter )
        {
            // Check if the person already exists in the collection
            if( Persons.Any( p => p.Name.Trim() == Person.Name.Trim() ))
            {
                // notify the user that the person already exists
                System.Windows.MessageBox.Show( "This person already exists." , "Duplicate Record" , System.Windows.MessageBoxButton.OK , System.Windows.MessageBoxImage.Warning );
                return;
            }

            Persons.Add( Person );
            NotifyPropertyChanged( nameof(Persons.Count) );
            Person = new Person();
        }

        /// <summary>
        /// Determines whether the AddPersonCommand can execute.
        /// </summary>
        /// <param name="parameter">Optional command parameter (unused).</param>
        /// <returns>True if the Person object is valid for addition; otherwise, false.</returns>
        private bool CanAddExecute( object parameter )
        {
            if (!string.IsNullOrEmpty( Person.Name ) && !string.IsNullOrWhiteSpace( Person.Name ) && Person.Age > 0 )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private ICommand _deletePersonCommand;

        /// <summary>
        /// Command for deleting the currently selected Person from the Persons collection.
        /// </summary>
        public ICommand DeletePersonCommand
        {
            get
            {
                if ( _deletePersonCommand == null )
                {
                    _deletePersonCommand = new RelayCommand( DeleteExecute, CanDeletePerson, true );
                }

                return _deletePersonCommand;
            }
        }

        /// <summary>
        /// Deletes the currently selected Person from the Persons collection.
        /// </summary>
        /// <param name="parameter">Optional command parameter (unused).</param>
        private void DeleteExecute( object parameter )
        {
            Persons.Remove( SelectedPerson );
            NotifyPropertyChanged( nameof(Persons.Count) );
            SelectedPerson = null;
        }

        /// <summary>
        /// Determines whether the selected person can be deleted.
        /// </summary>
        /// <param name="obj">Optional command parameter (unused).</param>
        /// <returns>True if a person is selected.</returns>
        private bool CanDeletePerson( object obj )
        {
            return SelectedPerson != null;
        }

        /// <summary>
        /// Currently selected person in the UI.
        /// </summary>
        public Person SelectedPerson
        {
            get { return _selectedPerson; }
            set
            {
                _selectedPerson = value;
                NotifyPropertyChanged( nameof(Persons.Count) );
                // Notify commands to re-evaluate their CanExecute logic
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private Person _selectedPerson;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event to notify the UI of property changes.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        private void NotifyPropertyChanged( string propertyName )
        {
            if ( PropertyChanged != null )
            {
                PropertyChanged( this, new PropertyChangedEventArgs( propertyName ) );
            }
        }
    }
}
 