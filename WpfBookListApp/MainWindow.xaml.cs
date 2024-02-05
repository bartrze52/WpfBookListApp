using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace WpfBookListApp
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<Book> bookList;
        private int colorIndex;

        public MainWindow()
        {
            InitializeComponent();
            bookList = new ObservableCollection<Book>();
            colorIndex = 0;
            LoadBooks();
            RefreshListView();
        }

        private void LoadBooks()
        {
            try
            {
                string filePath = "bookList.txt";
                if (File.Exists(filePath))
                {
                    string[] lines = File.ReadAllLines(filePath);
                    foreach (var line in lines)
                    {
                        string[] data = line.Split(',');
                        if (data.Length == 3)
                        {
                            Book book = new Book
                            {
                                Title = data[0],
                                Author = data[1],
                                Class = data[2]
                            };
                            bookList.Add(book);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas wczytywania danych z pliku: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveBooks()
        {
            try
            {
                string filePath = "bookList.txt";
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    foreach (var book in bookList)
                    {
                        sw.WriteLine($"{book.Title},{book.Author},{book.Class}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas zapisywania danych do pliku: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshListView()
        {
            BookListView.ItemsSource = bookList;
            BookListView.Items.Refresh();
        }

        private void AddBookButton_Click(object sender, RoutedEventArgs e)
        {
            AddBookWindow addBookWindow = new AddBookWindow();
            if (addBookWindow.ShowDialog() == true)
            {
                Book newBook = addBookWindow.NewBook;
                bookList.Add(newBook);
                RefreshListView();
            }
        }

        private void BookListView_LoadingRow(object sender, System.Windows.Controls.DataGridRowEventArgs e)
        {
            Color[] colors = { Colors.Blue, Colors.Red, Colors.Yellow };
            if (colorIndex < colors.Length)
            {
                e.Row.Background = new SolidColorBrush(colors[colorIndex]);
                colorIndex++;
            }
            else
            {
                colorIndex = 0;
            }
        }


        private void AddBookMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AddBookWindow addBookWindow = new AddBookWindow();
            if (addBookWindow.ShowDialog() == true)
            {
                Book newBook = addBookWindow.NewBook;
                bookList.Add(newBook);
                RefreshListView();
            }
        }

        private void EditBookMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (BookListView.SelectedItem != null)
            {
                Book selectedBook = (Book)BookListView.SelectedItem;
                AddBookWindow editBookWindow = new AddBookWindow(selectedBook);
                if (editBookWindow.ShowDialog() == true)
                {
                    Book updatedBook = editBookWindow.NewBook;
                    int selectedIndex = bookList.IndexOf(selectedBook);
                    bookList[selectedIndex] = updatedBook;
                    RefreshListView();
                }
            }
            else
            {
                MessageBox.Show("Wybierz książkę do edycji.", "Uwaga!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteBookMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (BookListView.SelectedItem != null)
            {
                Book selectedBook = (Book)BookListView.SelectedItem;
                if (MessageBox.Show($"Czy jesteś pewien, że chcesz usunąć tę książkę '{selectedBook.Title}'?", "Potwierdzenie",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    bookList.Remove(selectedBook);
                    RefreshListView();
                }
            }
            else
            {
                MessageBox.Show("Wybierz książkę do usunięcia.", "Uwaga!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Do you want to save changes before closing?", "Confirmation",
               MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                SaveBooks();
            }

        }
    }
}