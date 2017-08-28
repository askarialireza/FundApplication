namespace Infrastructure
{
    public static class BlurElementExtension
    {
        public static void BlurApply(this System.Windows.UIElement element, double blurRadius)
        {
            System.Windows.Media.Effects.BlurEffect blur =
                new System.Windows.Media.Effects.BlurEffect() { Radius = blurRadius };

            element.Effect = blur;

        }

        public static void BlurDisable(this System.Windows.UIElement element)
        {
            System.Windows.Media.Effects.BlurEffect blur =
                new System.Windows.Media.Effects.BlurEffect() { Radius = 0 };

            element.Effect = blur;
        }
    }
}