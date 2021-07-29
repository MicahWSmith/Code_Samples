using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace DataRepositoryProject
{
    public class Note
    {
        public string title { get; set; }
        public string text { get; set; }
        public Note(string newTitle, string newText) {
            title = newTitle;
            text = newText;
        }
    }
    public static class DataRepo
    {
        public static void InitializeDatabase()
        {
            using (SqliteConnection db = new SqliteConnection("Filename=Notes.db"))
            {
                db.Open();
                String tableCommand = "CREATE TABLE IF NOT EXISTS NoteTable (NoteID integer PrimaryKey, NoteTitle NVARCHAR(100), NoteText TEXT);";

                SqliteCommand createTable = new SqliteCommand(tableCommand, db);
                createTable.ExecuteReader();
            }
        }

        public static void AddData(string title, string text)
        {
            using (SqliteConnection db = new SqliteConnection("Filename=Notes.db"))
            {
                db.Open();
                String insertStatement = "INSERT INTO NoteTable VALUES (NULL, @title, @text);";

                SqliteCommand insertCommand = new SqliteCommand(insertStatement, db);
                insertCommand.Parameters.AddWithValue("@title", title);
                insertCommand.Parameters.AddWithValue("@text", text);
                insertCommand.ExecuteReader();
                db.Close();
            }
        }

        public static void EditData(string title, string text)
        {
            using (SqliteConnection db = new SqliteConnection("Filename=Notes.db"))
            {
                db.Open();
                String updateStatement = "UPDATE NoteTable SET NoteText = @text WHERE NoteTitle = @title;";

                SqliteCommand updateCommand = new SqliteCommand(updateStatement, db);
                updateCommand.Parameters.AddWithValue("@title", title);
                updateCommand.Parameters.AddWithValue("@text", text);
                updateCommand.ExecuteReader();
                db.Close();
            }
        }

        public static void DeleteData(string title)
        {
            using (SqliteConnection db = new SqliteConnection("Filename=Notes.db"))
            {
                db.Open();
                String deleteStatement = "DELETE FROM NoteTable WHERE NoteTitle = @title;";

                SqliteCommand deleteCommand = new SqliteCommand(deleteStatement, db);
                deleteCommand.Parameters.AddWithValue("@title", title);
                deleteCommand.ExecuteReader();
                db.Close();
            }
        }

        public static List<Note> GetData()
        {
            // notes list to return
            List<Note> notes = new List<Note>();

            using (SqliteConnection db = new SqliteConnection("Filename=Notes.db"))
            {
                db.Open();
                String selectStatement = "SELECT NoteTitle, NoteText FROM NoteTable;";

                SqliteCommand selectCommand = new SqliteCommand(selectStatement, db);
                SqliteDataReader queryResults = selectCommand.ExecuteReader();
                while (queryResults.Read())
                {
                    notes.Add(new Note(queryResults.GetString(0), queryResults.GetString(1)));
                }
                db.Close();
            }
            return notes;
        }

    }
}
