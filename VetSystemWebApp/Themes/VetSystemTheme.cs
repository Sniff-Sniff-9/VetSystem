using MudBlazor;

namespace VetSystemWebApp.Themes
{
    public static class VetSystemTheme
    {
        public static MudTheme Default => new MudTheme()
        {
            PaletteLight = new PaletteLight()
            {
                Primary = "#03A9F4",           // LightBlue 500 — как в твоём WPF
                Secondary = "#00BCD4",         // Cyan 500
                Tertiary = "#009688",          // Teal 500

                PrimaryContrastText = "#FFFFFF",
                SecondaryContrastText = "#FFFFFF",
                TertiaryContrastText = "#FFFFFF",

                TextPrimary = "#1F1F1F",
                TextSecondary = "#757575",

                Background = "#FAFAFA",
                Surface = "#FFFFFF",

                // Цвет верхней панели (AppBar)
                AppbarBackground = "#03A9F4",
                AppbarText = "#FFFFFF",

                Error = "#F44336",
                Warning = "#FF9800",
                Info = "#2196F3",
                Success = "#4CAF50",

                LinesDefault = "#BDBDBD",
                LinesInputs = "#9E9E9E"
            },

            PaletteDark = new PaletteDark()
            {
                Primary = "#4FC3F7",
                Secondary = "#4DD0E1",
                Tertiary = "#26A69A",

                PrimaryContrastText = "#000000",
                SecondaryContrastText = "#000000",
                TextPrimary = "#FFFFFF",
                TextSecondary = "#B0B0B0",

                Background = "#121212",
                Surface = "#1E1E1E",

                AppbarBackground = "#1E1E1E",
                AppbarText = "#FFFFFF"
            },

            LayoutProperties = new LayoutProperties()
            {
                DefaultBorderRadius = "8px",
                DrawerWidthLeft = "280px",
                AppbarHeight = "80px"
            },

            Typography = new Typography()
            {
                Default = new DefaultTypography()
                {
                    FontFamily = new[] { "Roboto", "Segoe UI", "sans-serif" },
                    FontSize = "1rem",
                    FontWeight = "400"
                },
                H5 = new H5Typography() { FontWeight = "500" },
                Button = new ButtonTypography()
                {
                    FontWeight = "500",
                    TextTransform = "none"
                }
            }
        };
    }
}