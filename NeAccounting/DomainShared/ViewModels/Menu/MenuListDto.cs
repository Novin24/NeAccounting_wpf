using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace DomainShared.ViewModels.Menu
{
    public partial class MenuListDto : ObservableObject
    {
        public Guid Id { get; set; }
        public bool IsParent { get; set; }

        [JsonIgnore]
        public MenuListDto? Parent { get; set; }

        [ObservableProperty]
        private bool? _isChecked = false;
        public string Name { get; set; } = default!;
        public List<MenuListDto> Children { get; set; } = [];

        public void SetChecked(bool isChecked)
        {
            IsChecked = isChecked;

            if (Children == null || Children.Count == 0)
                return;

            foreach (var child in Children)
            {
                child.SetChecked(isChecked);
            }
        }

        public void UpdateParentStatus()
        {
            if (Parent == null) return;

            if (Parent.Children.All(c => c.IsChecked == true))
            {
                Parent.IsChecked = true;
            }
            else if (Parent.Children.All(c => c.IsChecked == false))
            {
                Parent.IsChecked = false;
            }
            else
            {
                Parent.IsChecked = null;
            }
            Parent.UpdateParentStatus();
        }
    }
}
