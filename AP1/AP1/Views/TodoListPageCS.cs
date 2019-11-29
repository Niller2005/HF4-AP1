using Xamarin.Forms;

namespace AP1
{
    public class TodoListPageCs : ContentPage
    {
        private readonly ListView listView;

        public TodoListPageCs()
        {
            Title = "Todo";

            var toolbarItem = new ToolbarItem
            {
                Text = "+",
                IconImageSource = Device.RuntimePlatform == Device.iOS ? null : "plus.png"
            };
            toolbarItem.Clicked += async (sender, e) =>
            {
                await Navigation.PushAsync(new TodoItemPageCs
                {
                    BindingContext = new TodoItem()
                });
            };
            ToolbarItems.Add(toolbarItem);

            listView = new ListView
            {
                Margin = new Thickness(20),
                ItemTemplate = new DataTemplate(() =>
                {
                    var label = new Label
                    {
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalOptions = LayoutOptions.StartAndExpand
                    };
                    label.SetBinding(Label.TextProperty, "Name");

                    var tick = new Image
                    {
                        Source = ImageSource.FromFile("check.png"),
                        HorizontalOptions = LayoutOptions.End
                    };
                    tick.SetBinding(IsVisibleProperty, "Done");

                    var stackLayout = new StackLayout
                    {
                        Margin = new Thickness(20, 0, 0, 0),
                        Orientation = StackOrientation.Horizontal,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Children = {label, tick}
                    };

                    return new ViewCell {View = stackLayout};
                })
            };
            listView.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem != null)
                    await Navigation.PushAsync(new TodoItemPageCs
                    {
                        BindingContext = e.SelectedItem as TodoItem
                    });
            };

            Content = listView;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            ((App) Application.Current).ResumeAtTodoId = -1;
            listView.ItemsSource = await App.Database.GetItemsAsync();
        }
    }
}