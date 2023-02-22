using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Essentials;
using SQLite;
using Microsoft.Data.Sqlite;
using System;
//using Xamarin.Forms;

namespace MyLanguageLearningApp
{
    [Activity(Label = "My Language Learning App", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private ListView wordListView;
        private EditText wordEditText;
        private EditText translationEditText;
        private Button addButton;

        private List<Word> wordList = new List<Word>();
        private SQLiteConnection dbConn;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get UI controls
            wordListView = FindViewById<ListView>(Resource.Id.word_list_view);
            wordEditText = FindViewById<EditText>(Resource.Id.word_edit_text);
            translationEditText = FindViewById<EditText>(Resource.Id.translation_edit_text);
            addButton = FindViewById<Button>(Resource.Id.add_button);

            // Set up Add button click event
            addButton.Click += AddButton_Click;

            // Get database connection
            var dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "mylanguagelearningapp.db");
            dbConn = new SQLiteConnection(dbPath);

            // Create Word table if it doesn't exist
            dbConn.CreateTable<Word>();

            // Refresh word list view
            RefreshWordList();
        }

        private void SetContentView(object main)
        {
            throw new NotImplementedException();
        }

        private T FindViewById<T>(object word_list_view)
        {
            throw new NotImplementedException();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            // Check for necessary permissions
            var status = Permissions.CheckStatusAsync<Permissions.StorageWrite>();
            if (status != PermissionStatus.Granted)
            {
                // Request necessary permission
                Permissions.RequestAsync<Permissions.StorageWrite>();
                return;
            }

            // Create new Word object
            var newWord = new Word
            {
                WordText = wordEditText.Text,
                TranslationText = translationEditText.Text
            };

            // Add Word object to database
            dbConn.Insert(newWord);

            // Clear text fields
            wordEditText.Text = "";
            translationEditText.Text = "";

            // Refresh word list view
            RefreshWordList();
        }

        private void RefreshWordList()
        {
            // Get list of all words in database
            var words = dbConn.Table<Word>().ToList();

            // Clear word list
            wordList.Clear();

            // Add words to word list
            foreach (var word in words)
            {
                wordList.Add(word);
            }

            // Set word list adapter
            wordListView.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, wordList);
        }
    }

    [Table("Word")]
    public class Word
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string WordText { get; set; }
        public string TranslationText { get; set; }

        public override string ToString()
        {
            return WordText + " - " + TranslationText;
        }
    }
}