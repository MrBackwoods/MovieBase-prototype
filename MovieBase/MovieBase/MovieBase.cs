﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MovieBase
{
    public partial class MovieBase : Form
    {
        DatabaseHandler DB = new DatabaseHandler();
        List<Movie> movieList = new List<Movie>();

        public MovieBase()
        {
            InitializeComponent();
            MovieListView.View = View.Details;
            MovieListView.Columns.Add("ID", 40, HorizontalAlignment.Left);
            MovieListView.Columns.Add("Name", 110, HorizontalAlignment.Left);
            MovieListView.Columns.Add("Release Year", 80, HorizontalAlignment.Left);
            MovieListView.Columns.Add("Director", 120, HorizontalAlignment.Left);
            MovieListView.Columns.Add("Note", 380, HorizontalAlignment.Left);
            MovieListView.Columns.Add("Review", 67, HorizontalAlignment.Left);
        }

        private void AddNewButton_Click(object sender, EventArgs e)
        {
            Movie movie = new Movie();
            DB.AddMovie();
            refreshMovieList();
            clearTextBoxes();
            DisableTextBoxes();


        }
        private void EnableTextBoxes()
        {
            nameBox.Enabled = true;
            yearBox.Enabled = true;
            directorBox.Enabled = true;
            noteBox.Enabled = true;
            reviewBox.Enabled = true;
            nameBox.BackColor = System.Drawing.Color.White;
            yearBox.BackColor = System.Drawing.Color.White;
            directorBox.BackColor = System.Drawing.Color.White;
            noteBox.BackColor = System.Drawing.Color.White;
            reviewBox.BackColor = System.Drawing.Color.White;
        }
        public void DisableTextBoxes()
        {

            nameBox.Enabled = false;
            yearBox.Enabled = false;
            directorBox.Enabled = false;
            noteBox.Enabled = false;
            reviewBox.Enabled = false;
            nameBox.BackColor = System.Drawing.Color.DimGray;
            yearBox.BackColor = System.Drawing.Color.DimGray;
            directorBox.BackColor = System.Drawing.Color.DimGray;
            noteBox.BackColor = System.Drawing.Color.DimGray;
            reviewBox.BackColor = System.Drawing.Color.DimGray;
        }



        private void MovieBase_Load(object sender, EventArgs e)
        {
            refreshMovieList();
        }

        public void refreshMovieList() {
            movieList = DB.GetMovies();
            MovieListView.Items.Clear();

            int i = 0;
            foreach (Movie movie in movieList)
            {
                MovieListView.Items.Add(movie.Id.ToString());
                MovieListView.Items[i].SubItems.Add(movie.Name);
                MovieListView.Items[i].SubItems.Add(movie.ReleaseYear);
                MovieListView.Items[i].SubItems.Add(movie.Director);
                MovieListView.Items[i].SubItems.Add(movie.Note);
                MovieListView.Items[i].SubItems.Add(movie.Review.ToString());
                i++;
            }
        }

        private void DeleteSelectedButton_Click(object sender, EventArgs e)
        {
            if (MovieListView.SelectedItems.Count > 0)
            {
                DB.DeleteMovie(Convert.ToInt32(MovieListView.SelectedItems[0].Text));
                refreshMovieList();
                clearTextBoxes();
                DisableTextBoxes();

            }
        }

        private void MovieListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MovieListView.SelectedItems.Count > 0)
            {
                clearTextBoxes();
                EnableTextBoxes();
                IDLabel.Text = MovieListView.SelectedItems[0].Text;
                nameBox.Text = MovieListView.SelectedItems[0].SubItems[1].Text;
                yearBox.Text = MovieListView.SelectedItems[0].SubItems[2].Text;
                directorBox.Text = MovieListView.SelectedItems[0].SubItems[3].Text;
                noteBox.Text = MovieListView.SelectedItems[0].SubItems[4].Text;
                reviewBox.Text = MovieListView.SelectedItems[0].SubItems[5].Text;
            }
            else {
                DisableTextBoxes();
            }
        }

        private void clearTextBoxes() {
            IDLabel.Text = "";
            nameBox.Text = "";
            yearBox.Text = "";
            directorBox.Text = "";
            noteBox.Text = "";
            reviewBox.Text = "";
        }

        private void ChangeSelectedButton_Click(object sender, EventArgs e)

            {
                if (MovieListView.SelectedItems.Count > 0)
                {

                    int review;
                    bool reviewOk = Int32.TryParse(reviewBox.Text, out review);
                    if (nameBox.Text != "" & reviewBox.Text != "" & reviewOk)
                    {
                        if (review > 0 & review < 6)
                        {
                            if (yearBox.Text == "" | yearBox.Text.Length == 4)
                            {
                                if (noteBox.Text.Length < 200)
                                {
                                    Movie movie = new Movie();
                                    movie.Id = Convert.ToInt32(IDLabel.Text);
                                    movie.Name = nameBox.Text;
                                    movie.ReleaseYear = yearBox.Text;
                                    movie.Director = directorBox.Text;
                                    movie.Note = noteBox.Text;
                                    movie.Review = Convert.ToInt32(reviewBox.Text);
                                    DB.UpdateMovie(movie);
                                    refreshMovieList();
                                    clearTextBoxes();
                                    DisableTextBoxes();
                                    helpLabel.Text = "";
                                }
                                else
                                {
                                    helpLabel.Text = "Note limit is 200 characters.";
                                }
                            }
                            else
                            {
                                helpLabel.Text = "Year number must be 4 digits long";
                            }
                        }
                        else
                        {
                            helpLabel.Text = "Review value must be 1, 2, 3, 4 or 5.";
                        }
                    }
                    else
                    {
                        helpLabel.Text = "Some of the required fields are empty.";
                    }
                }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
