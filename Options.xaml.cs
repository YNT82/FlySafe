using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace FlySafe
{
    public partial class NewWindow : Window
    {
        public NewWindow()
        {
            InitializeComponent();
        }

        // Обработчик для левого клика мыши по окну (для закрытия окна)
        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Закрытие окна при клике
            this.Close();
        }

        // Обработчик для события "Чекбокс отмечен"
        private void AlwaysOnTopCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Галочка становится видимой при установке чекбокса в состояние "отмечено"
            var checkMark = (Path)((CheckBox)sender).Template.FindName("CheckMark", (FrameworkElement)sender);
            checkMark.Opacity = 1;  // Показываем галочку
        }

        // Обработчик для события "Чекбокс снят"
        private void AlwaysOnTopCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // Галочка исчезает при снятии чекбокса
            var checkMark = (Path)((CheckBox)sender).Template.FindName("CheckMark", (FrameworkElement)sender);
            checkMark.Opacity = 0;  // Скрываем галочку
        }
    }
}
