using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;

namespace SymblinkShorthand
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void xamlPickTargetPath_Clicked(object sender, RoutedEventArgs args)
        {
            try
            {
                TopLevel topLevel = TopLevel.GetTopLevel(this);

                if (xamlTargetIsFile.IsChecked.Value)
                {
                    IReadOnlyList<IStorageFile> files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
                    {
                        Title = "Select a file",
                        AllowMultiple = false
                    });

                    if (files.Count >= 1)
                    {
                        xamlTargetPath.Text = Uri.UnescapeDataString(files[0].Path.AbsolutePath);
                    }
                }
                else if (xamlTargetIsDir.IsChecked.Value)
                {
                    IReadOnlyList<IStorageFolder> files = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
                    {
                        Title="Select target directory",
                        AllowMultiple = false
                    });
                    xamlTargetPath.Text = Uri.UnescapeDataString(files[0].Path.AbsolutePath);
                    xamlTargetIsDir.IsEnabled = false;
                    xamlTargetIsFile.IsEnabled = false;
                }
                
            } catch (Exception e)
            {
                xamlStatus.Text = e.Message;
            }
            
        }

        private async void xamlPickDestPath_Clicked(object sender, RoutedEventArgs args)
        {
            try
            {
                TopLevel topLevel = TopLevel.GetTopLevel(this);

                IReadOnlyList<IStorageFolder> files = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
                {
                    Title = "Select target directory",
                    AllowMultiple = false
                });
                xamlTargetDestPath.Text = Uri.UnescapeDataString(files[0].Path.AbsolutePath);

            }
            catch (Exception e)
            {
                xamlStatus.Text = e.Message;
            }

        }

        private void xamlLinkTargets_Clicked(object sender, RoutedEventArgs args)
        {
            try
            {
                if (xamlTargetDestName.Text != "")
                {
                    if (Directory.Exists(xamlTargetPath.Text))
                    {
                        Directory.CreateSymbolicLink(xamlTargetDestPath.Text + xamlTargetDestName.Text, xamlTargetPath.Text);
                    }
                    else if (File.Exists(xamlTargetPath.Text))
                    {
                        File.CreateSymbolicLink(xamlTargetDestPath.Text + xamlTargetDestName.Text, xamlTargetPath.Text);
                    }
                    else
                    {
                        xamlStatus.Text = "Target path does not exist";
                    }
                    xamlTargetIsDir.IsEnabled = true;
                    xamlTargetIsFile.IsEnabled = true;
                }
                else
                {
                    xamlStatus.Text = "The destination name is required";
                }
                
            } catch (Exception e) { 
                xamlStatus.Text = e.Message;
            }
        }
    }
}