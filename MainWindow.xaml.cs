using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.IO;
using System.Net;
using System.Windows.Input;
using Microsoft.Win32;
using System.Windows.Controls;

namespace FileDownloader
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<DownloadedFile> FileCollection = new ObservableCollection< DownloadedFile>();

        public MainWindow()
        {
            InitializeComponent();

            ProcessDownloadedFiles();

            ListBoxFiles.ItemsSource = FileCollection;
        }

        //путь к папке, в которую сохраняются все файлы
        private string filePath = @"C:\Users\vandr\Desktop\проекты\FileDownloader\Downloaded\";

        //скачивание файла
        private void downloading()
        {

            WebClient webload = new WebClient();

            webload.DownloadFileAsync(new Uri($"{TextBoxLink.Text}"), filePath + CreateFileName());

            if (IsDownloaded() == true)
            {
                MessageBox.Show("Downloaded successfully");
                AddDownloadedFile();

            } 
            else MessageBox.Show("Download is failed");
        }

        //проверка, скачан ли файл
        private bool IsDownloaded()
        {
            return File.Exists(filePath + CreateFileName());
        }

        //создание имени файла исходя из ссылки на него
        private string CreateFileName()
        {
            string tempName = "";
            char[] array = TextBoxLink.Text.ToCharArray();
            Array.Reverse(array);
            foreach (char ch in array)
            {
                if (ch == '/')
                {
                    break;
                }
                else
                {
                    tempName += ch;
                }
            }
            array = tempName.ToCharArray();
            Array.Reverse(array);
            string fileName = "";
            foreach (char ch in array)
            {
                fileName += ch;
            }
            return fileName;
        }

        //получение расширения для только что загруженного файла
        public string FileExtension()
        {
            string[] words = CreateFileName().Split('.');
            string extension = "." + words[words.Length - 1];
            return extension;
        }

        //создание пути иконки
        public string CreateThumbToUse(string extension, string fullname)
        {
            var thumbToUse = "";

            switch (extension)
            {
                case ".jpg":
                case ".png":
                case ".bmp":
                case ".jpeg":
                case ".gif":
                    thumbToUse = fullname; break;
                case ".txt":
                    thumbToUse = "Images/txt.png"; break;
                case ".doc":
                    thumbToUse = "Images/doc.png"; break;
                case ".rtf":
                    thumbToUse = "Images/rtf.png"; break;
                case ".pdf":
                    thumbToUse = "Images/pdf.png"; break;
                case ".mp4":
                    thumbToUse = "Images/mp4.png"; break;
                case ".avi":
                    thumbToUse = "Images/avi.png"; break;
                case ".mp3":
                    thumbToUse = "Images/mp3.png"; break;
                case ".css":
                    thumbToUse = "Images/css.png"; break;
                case ".dbf":
                    thumbToUse = "Images/dbf.png"; break;
                case ".dwg":
                    thumbToUse = "Images/dwg.png"; break;
                case ".exe":
                    thumbToUse = "Images/exe.png"; break;
                case ".fla":
                    thumbToUse = "Images/fla.png"; break;
                case ".html":
                    thumbToUse = "Images/html.png"; break;
                case ".iso":
                    thumbToUse = "Images/iso.png"; break;
                case ".js":
                    thumbToUse = "Images/javascript.png"; break;
                case ".json":
                    thumbToUse = "Images/json-file.png"; break;
                case ".ppt":
                case ".pptx":
                    thumbToUse = "Images/ppt.png"; break;
                case ".psd":
                    thumbToUse = "Images/psd.png"; break;
                case ".xls":
                case ".xlsm":
                    thumbToUse = "Images/xls.png"; break;
                case ".xml":
                    thumbToUse = "Images/xml.png"; break;
                default:
                    thumbToUse = "Images/file.png"; break;
            }
            return thumbToUse;
        }

        //добавление загруженного файла в коллекцию всех файлов папки 
        public void AddDownloadedFile()
        {

            FileCollection.Add(new DownloadedFile()
            {
                filePath = filePath + CreateFileName(),
                fileName = CreateFileName(),
                fileThumbnail = CreateThumbToUse(FileExtension(), filePath + CreateFileName())
            });
        }

        //создание коллекции из файлов, которые уже находятся в папке
        public void ProcessDownloadedFiles()
        {
            var Folder = new DirectoryInfo(filePath.Remove(filePath.Length-1));
            var downloadedFiles = Folder.GetFiles("*");

            foreach (var file in downloadedFiles)
            {
                FileCollection.Add(new DownloadedFile()
                {
                    filePath = file.FullName,
                    fileName = file.Name,
                    fileThumbnail = CreateThumbToUse(file.Extension, file.FullName)
                });
            }
        }

        //открытие файла с помощью двойного нажатия
        public void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 2)
            {
                StackPanel temp = (StackPanel)sender;
                DownloadedFile newfile = (DownloadedFile)temp.DataContext;
                Process.Start(newfile.filePath);
            }
        }

        //кнопка "Скачать"
        private void Button_Download(object sender, RoutedEventArgs e)
        {
            try
            {
                downloading();
            }
            catch
            {
                MessageBox.Show("Something went wrong");
            }
        }

        private void ListBoxFiles_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}
