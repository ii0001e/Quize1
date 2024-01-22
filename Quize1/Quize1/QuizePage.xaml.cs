using Android.Content.Res;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Quize1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuizePage : ContentPage
    {
        private List<string> englishTranslation;
        private List<string> russianWords;
        private int currentQuestionIndex;
        private Label questionLabel;
        private List<string> translation;
        private List<string> buttonLabels;

        Button start;
        Button button;
        Button finish;
        Label name;
        bool help = false;

        public QuizePage()
        {
            englishTranslation = new List<string>();
            russianWords = new List<string>();

            LoadWordsFromFiles();

            name = new Label
            {
                Text = "",
                FontSize = 24,
                TextColor = Color.Gainsboro,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.End,
                Margin = 5
            };

            start = new Button
            {
                Text = "Start!",
                FontSize = 20,
                TextColor = Color.Red,
                BorderColor = Color.MistyRose,
                CornerRadius = 10,
                WidthRequest = 150,
                HeightRequest = 55,
                BorderWidth = 2,
                BackgroundColor = Color.YellowGreen,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Center
            };

            start.Clicked += Start_Clicked;

            finish = new Button
            {
                Text = "quit",
                FontSize = 20,
                TextColor = Color.Red,
                BorderColor = Color.MistyRose,
                CornerRadius = 10,
                WidthRequest = 280,
                HeightRequest = 55,
                BorderWidth = 2,
                BackgroundColor = Color.YellowGreen,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Center
            };

            finish.IsVisible = false;
            finish.Clicked += Finish_ClickedAsync;

            Grid grid = new Grid
            {
                RowDefinitions =
            {
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
            },
                ColumnDefinitions =
            {
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
            }
            };



            // Create and add buttons dynamically based on the number of expressions
            for (int i = 0; i < Math.Min(russianWords.Count, 15); i++)
            {
                string buttonLabel = russianWords[i];

                button = new Button
                {
                    Text = buttonLabel,
                    FontSize = 15,
                    TextColor = Color.Firebrick,
                    BorderColor = Color.DeepPink,
                    CornerRadius = 5,
                    HeightRequest = 100,
                    WidthRequest = 200,
                    BorderWidth = 2,
                    BackgroundColor = Color.Pink,
                };

                grid.Children.Add(button, i % 3, i / 3);
                button.Clicked += Button_ClickedAsync;
            }





            Content = new StackLayout { Children = { name, start, grid, finish } };

        }

        private async void Finish_ClickedAsync(object sender, EventArgs e)
        {
            Button sample = sender as Button;
            await Navigation.PopAsync();
        }


        private async void Button_ClickedAsync(object sender, EventArgs e)
        {
            if (help)
            {
                Button button = (Button)sender;
                string result = await DisplayPromptAsync("Задание:", $"Переведи выражения на Английский '{button.Text}': ", "OK", keyboard: Keyboard.Chat);

                // Find the index of the clicked button in the list
                int buttonIndex = russianWords.IndexOf(button.Text);

                // Check if the index is valid
                if (buttonIndex >= 0 && buttonIndex < englishTranslation.Count)
                {
                    // Compare the user input with the correct Russian translation (case-insensitive)
                    if (result.Trim().Equals(englishTranslation[buttonIndex].Trim(), StringComparison.OrdinalIgnoreCase))
                    {

                        await DisplayAlert("Результат:", "Верно! Молодец!", "OK");
                        return;

                    }
                }

                // If no match is found, handle incorrect answer
                button.BackgroundColor = Color.Honeydew;
                DisplayAlert("Результат:", "Ого! Вам надо еще учиться!", "OK");
            }
            else
            {
                DisplayAlert("Внимание!!", "Нажмите 'Start'", "OK");
            }
        }

        private void LoadWordsFromFiles()
        {
            englishTranslation = new List<string>();
            russianWords = new List<string>();
            translation = new List<string>();
            buttonLabels = new List<string>();

            // Read English words from asset file
            AssetManager assets = Android.App.Application.Context.Assets;
            using (StreamReader sr = new StreamReader(assets.Open("english_words.txt")))
            {
                string englishTranslationFileContent = sr.ReadToEnd();
                englishTranslation = new List<string>(englishTranslationFileContent.Split('\n').Select(line => line.Trim()));
            }

            // Read Russian translations from asset file
            using (StreamReader sr = new StreamReader(assets.Open("russian_words.txt")))
            {
                string russianWordsFileContent = sr.ReadToEnd();
                russianWords = new List<string>(russianWordsFileContent.Split('\n').Select(line => line.Trim()));
            }

            // Ensure both lists have the same number of elements
            if (russianWords.Count == englishTranslation.Count)
            {
                // Populate translation list
                translation.AddRange(englishTranslation);

                // Populate buttonLabels list (using a simple index)
                for (int i = 0; i < russianWords.Count; i++)
                {
                    buttonLabels.Add($"button {i + 1}");
                }
            }
            else
            {

            }
        }
        private async void Start_Clicked(object sender, EventArgs e)
        {
            help = true;
            string Name = await DisplayPromptAsync("Вопрос", "Введите ваше имя: ", "OK", keyboard: Keyboard.Chat);
            name.Text = "Здравствуй, " + Name + "!";
            start.BackgroundColor = Color.Black;
            start.IsVisible = false;
            if (start.IsVisible == false)
            {
                finish.IsVisible = true;
            }
        }

    }

}