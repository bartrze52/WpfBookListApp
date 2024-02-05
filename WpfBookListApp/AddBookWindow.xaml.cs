using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfBookListApp
{
    public partial class AddBookWindow : Window
    {
        public Book NewBook { get; private set; }
        public AddBookWindow()
        {
            InitializeComponent();
            NewBook = new Book();
        }
        public AddBookWindow(Book existingBook)
        {
            InitializeComponent();
            NewBook = new Book
            {
                Title = existingBook.Title,
                Author = existingBook.Author,
                Class = existingBook.Class
            };

            TitleTextBox.Text = existingBook.Title;
            AuthorTextBox.Text = existingBook.Author;
            ClassTextBox.Text = existingBook.Class;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            NewBook.Title = TitleTextBox.Text;
            NewBook.Author = AuthorTextBox.Text;
            NewBook.Class = ClassTextBox.Text;
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}