using NoteApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.Storage;
using System.Diagnostics;
using System.IO;
using DataRepositoryProject;
using Windows.UI.Xaml.Controls;

namespace NoteApp.ViewModels
{
    public class NoteViewModel : INotifyPropertyChanged
    {
        //Event
        public event PropertyChangedEventHandler PropertyChanged;

        #region "Properties" 
            // Command instances for view model
            public AcceptCommand AcceptCommand { get; }
            public EditCommand EditCommand { get;  }
            public AddCommand AddCommand { get; }
            public DeleteCommand DeleteCommand { get; }
            // Note collection and filename collection
            public ObservableCollection<NoteModel> Notes { get; set; }
            public ObservableCollection<string> NoteFiles { get; set; }
            // data of currently selected note
            public string CurrentNoteTitle { get; set; }
            public string CurrentNoteText { get; set; }
            
            private bool _readflag = true; // read only mode bool

            public string cmdBarState = "Collapsed";

            public bool ReadFlag // read only mode bool with value change invocation for command checking
            {
                get => _readflag; 
                set
                {
                    _readflag = value;

                    // when readflag changed check to close cmd bar for RTF
                    if (_readflag)
                    {
                        cmdBarState = "Collapsed";
                    }
                    else
                    {
                        cmdBarState = "Visible";
                    }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ReadFlag)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(cmdBarState)));
                    AcceptCommand.FireTheCanExecuteChanged();
                    EditCommand.FireTheCanExecuteChanged();
                    AddCommand.FireTheCanExecuteChanged();
                }
            }


        private string _filter; // filter term

            public string Filter // on filter change filter list
            {
                get { return _filter; }
                set
                {
                    if(value == _filter)
                    { return; }
                    _filter = value;
                    NoteFilter();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Filter));
                }
            }

            public List<NoteModel> _allNotes = new List<NoteModel>(); // all note collection to hold different vals from filtered list

            private NoteModel _selectedNote; // currently selected note

            public NoteModel SelectedNote // on selected note val change 
            {
                get { return _selectedNote; }
                set
                {
                    _selectedNote = value;

                    if(value == null) // set default vals if null
                    {
                        SelectedNote = new NoteModel(); // default is unselected note
                        ReadFlag = false; // can edit text if nothing is selected

                        CurrentNoteTitle = SelectedNote.Title; // current vals
                        CurrentNoteText = SelectedNote.NoteText;
                    }
                    else // set vals if not null
                    {
                        CurrentNoteTitle = value.Title;
                        CurrentNoteText = value.NoteText;
                        ReadFlag = true;
                    }

                    // invoke property changed events
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentNoteTitle"));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentNoteText"));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Notes"));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ReadFlag"));

                    // check if can execute relevant commands
                    AcceptCommand.FireTheCanExecuteChanged();
                    EditCommand.FireTheCanExecuteChanged();
                    AddCommand.FireTheCanExecuteChanged();
                    DeleteCommand.FireTheCanExecuteChanged();
                }
            }

            #endregion

            //Constructor
            public NoteViewModel()
            {
                // create cmd instances
                AcceptCommand = new AcceptCommand(this);

                EditCommand = new EditCommand(this);

                AddCommand = new AddCommand(this);

                DeleteCommand = new DeleteCommand(this);

                InitViewModel(); // init data for note list
            }

            public void InitViewModel()
            {
                //Instantiate note list
                Notes = new ObservableCollection<NoteModel>();

                // instantiate note filename list
                NoteFiles = new ObservableCollection<string>();

                List<Note> notes = DataRepo.GetData();

                for (int i = 0; i < notes.Count; i++)
                {
                    _allNotes.Add(new NoteModel(notes[i].title, notes[i].text));
                    NoteFilter();
                }

                ReadFlag = false; // not read only mode by default

            }

            private void NoteFilter() // filter note results
            {
                if (_filter == null)
                {
                    _filter = ""; // default filter is empty string
                }
                //If _filter has a value (ie. user entered something in Filter textbox)
                //Lower-case and trim string
                var lowerCaseFilter = Filter.ToLowerInvariant().Trim();

                //Use LINQ query to get all note names that match filter text, as a list
                var result =
                    _allNotes.Where(d => d.Title.ToLowerInvariant()
                    .Contains(lowerCaseFilter))
                    .ToList();

                //Get list of values in current filtered list that we want to remove
                //(ie. don't meet new filter criteria)
                var toRemove = Notes.Except(result).ToList();

                //Loop to remove items that fail filter
                foreach (var x in toRemove)
                {
                    Notes.Remove(x);
                }

                var resultCount = result.Count;
                // Add back in correct order.
                for (int i = 0; i < resultCount; i++)
                {
                    var resultItem = result[i];
                    if (i + 1 > Notes.Count || !Notes[i].Equals(resultItem))
                    {
                        Notes.Insert(i, resultItem);
                    }
                }
            }
            public void AddNote(Models.NoteModel newNote)
            {
                _allNotes.Add(newNote);
            }
            public void RemoveNote(Models.NoteModel newNote)
            {
                _allNotes.Remove(newNote);
            }
    }
}
