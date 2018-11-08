﻿//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************
using System;
using System.Collections.Generic;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace AppUIBasics.ControlPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RichEditBoxPage : Page
    {
        public RichEditBoxPage()
        {
            this.InitializeComponent();

            if (!ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 6))
            {
                enableContactsButton.Visibility = Visibility.Collapsed;
                enablePlacesButton.Visibility = Visibility.Collapsed;
            }
        }

        private async void OpenButton_Click(object sender, RoutedEventArgs e)
        {
#if NETFX_CORE // UNO TODO
          // Open a text file.
            Windows.Storage.Pickers.FileOpenPicker open =
                new Windows.Storage.Pickers.FileOpenPicker();
            open.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            open.FileTypeFilter.Add(".rtf");

            Windows.Storage.StorageFile file = await open.PickSingleFileAsync();

            if (file != null)
            {
                using (Windows.Storage.Streams.IRandomAccessStream randAccStream =
                    await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {
                    // Load the file into the Document property of the RichEditBox.
                    editor.Document.LoadFromStream(Windows.UI.Text.TextSetOptions.FormatRtf, randAccStream);
                }
            }
#endif
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
#if NETFX_CORE // UNO TODO
           FileSavePicker savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;

            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Rich Text", new List<string>() { ".rtf" });

            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = "New Document";

            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                // Prevent updates to the remote version of the file until we 
                // finish making changes and call CompleteUpdatesAsync.
                CachedFileManager.DeferUpdates(file);
                // write to file
                using (Windows.Storage.Streams.IRandomAccessStream randAccStream =
                    await file.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite))
                {
                    editor.Document.SaveToStream(Windows.UI.Text.TextGetOptions.FormatRtf, randAccStream);
                }

                // Let Windows know that we're finished changing the file so the 
                // other app can update the remote version of the file.
                FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                if (status != FileUpdateStatus.Complete)
                {
                    Windows.UI.Popups.MessageDialog errorBox =
                        new Windows.UI.Popups.MessageDialog("File " + file.Name + " couldn't be saved.");
                    await errorBox.ShowAsync();
                }
            }
#endif
        }

        private void BoldButton_Click(object sender, RoutedEventArgs e)
        {
#if NETFX_CORE // UNO TODO
            editor.Document.Selection.CharacterFormat.Bold = FormatEffect.Toggle;
#endif
        }

        private void ItalicButton_Click(object sender, RoutedEventArgs e)
        {
#if NETFX_CORE // UNO TODO
            editor.Document.Selection.CharacterFormat.Italic = FormatEffect.Toggle;
#endif
		}

        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
#if NETFX_CORE // UNO TODO
            // Extract the color of the button that was clicked.
            Button clickedColor = (Button)sender;
            var rectangle = (Windows.UI.Xaml.Shapes.Rectangle)clickedColor.Content;
            var color = ((Windows.UI.Xaml.Media.SolidColorBrush)rectangle.Fill).Color;

            editor.Document.Selection.CharacterFormat.ForegroundColor = color;

            fontColorButton.Flyout.Hide();

			editor.Focus(Windows.UI.Xaml.FocusState.Keyboard);
#endif
        }

        private void FindBoxHighlightMatches()
        {
#if NETFX_CORE // UNO TODO
            FindBoxRemoveHighlights();

            string textToFind = findBox.Text;
            if (textToFind != null)
            {
                ITextRange searchRange = editor.Document.GetRange(0, 0);
                while (searchRange.FindText(textToFind, TextConstants.MaxUnitCount, FindOptions.None) > 0)
                {
                    searchRange.CharacterFormat.BackgroundColor = Colors.Yellow;
                }
            }
#endif
        }

        private void FindBoxRemoveHighlights()
        {
#if NETFX_CORE // UNO TODO
            ITextRange documentRange = editor.Document.GetRange(0, TextConstants.MaxUnitCount);
            documentRange.CharacterFormat.BackgroundColor = Colors.Transparent;
#endif
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
#if NETFX_CORE // UNO TODO
           if (e.NewSize.Width <= 768)
            {
                editor.Width = e.NewSize.Width - 20;
            }
            else
            {
                editor.Width = e.NewSize.Width - 100;
            }
#endif
        }

        //TODO: Check for version of the build

        Windows.UI.Xaml.Documents.ContactContentLinkProvider contactContentLinkProvider;
        private void EnableContactsButton_Click(object sender, RoutedEventArgs e)
        {
#if NETFX_CORE // UNO TODO
            if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 6))
            {

                if ((bool)enableContactsButton.IsChecked)
                {
                    if (editor.ContentLinkProviders == null)
                    {
                        editor.ContentLinkProviders = new Windows.UI.Xaml.Documents.ContentLinkProviderCollection();
                    }
                    if (contactContentLinkProvider == null)
                    {
                        contactContentLinkProvider = new Windows.UI.Xaml.Documents.ContactContentLinkProvider();
                    }
                    editor.ContentLinkProviders.Add(contactContentLinkProvider);

                }
                else
                {
                    bool b = editor.ContentLinkProviders.Remove(contactContentLinkProvider);
                }
            }
#endif
        }

        Windows.UI.Xaml.Documents.PlaceContentLinkProvider placeContentLinkProvider;
        private void enablePlacesButton_Click(object sender, RoutedEventArgs e)
        {
#if NETFX_CORE // UNO TODO
            if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 6))
            {

                if ((bool)enablePlacesButton.IsChecked)
                {
                    if (editor.ContentLinkProviders == null)
                    {
                        editor.ContentLinkProviders = new Windows.UI.Xaml.Documents.ContentLinkProviderCollection();

                    }
                    if (placeContentLinkProvider == null)
                    {
                        placeContentLinkProvider = new Windows.UI.Xaml.Documents.PlaceContentLinkProvider();
                    }

                    editor.ContentLinkProviders.Add(placeContentLinkProvider);
                }
                else
                {
                    bool b = editor.ContentLinkProviders.Remove(placeContentLinkProvider);
                }
            }
#endif
        }
    }
}